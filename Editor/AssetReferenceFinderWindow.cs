using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RichPackage.Editor
{
	public class AssetReferenceFinderWindow : OdinEditorWindow
	{
		#region Constants

		private const string LogPrefix = "[Asset Finder]";
		private const string DatabaseButtonGroup = "DB";

		/// <summary>
		/// How often the ui should repaint during an async operation.
		/// </summary>
		private const float RepaintInterval = 0.1f;

		#endregion Constants

		[SerializeField,
			ProgressBar(0, 100, ValueLabelAlignment = TextAlignment.Center, DrawValueLabel = true),
			ReadOnly,
			ShowIf(nameof(AsyncOperationInProgress))]
		private float progressBar = 0;

		[SerializeField, ReadOnly, Tooltip("The last log printed to the console.")]
		private string log;

		[ShowInInspector, PropertyOrder(10)]
		private int CachedFilesCount => sceneFiles.Count + assetFiles.Count;

		[Space(5)]

		[Title("Settings")]
		[Min(1), Tooltip("Background worker tasks to use.")]
		public int workerThreadCount = 4;

		[SerializeField, Tooltip("Where should we look for references."),
			DisableIf(nameof(AsyncOperationInProgress))]
		private ESearch searchQuery = ESearch.Assets;

		[SerializeField, OnValueChanged(nameof(ClearBatchList)),
			DisableIf(nameof(AsyncOperationInProgress))]
		private EMode mode = EMode.Single;

		[Title("Query")]
		[ShowIf("@mode == EMode.Single"),
			Required(InfoMessageType.Warning)]
		public Object target;

		[Title("Queries")]
		[ShowIf("@mode == EMode.Batch"), InlineButton(nameof(ClearBatchList), "Clear"),
			Required(InfoMessageType.Warning)]
		public Object[] targets;

		private readonly List<AssetInfo> sceneFiles = new List<AssetInfo>();
		private readonly List<AssetInfo> assetFiles = new List<AssetInfo>();

		private static string assetsFolder;
		private static string projectFolder;
		private CancellationTokenSource cts;
		private DateTime asyncOperationStartTime;
		private bool AsyncOperationInProgress { get; set; }

		[MenuItem(RichEditorUtility.WindowMenuName + "Asset Reference Finder")]
		public static void OpenWindow()
		{
			GetWindow<AssetReferenceFinderWindow>();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			// cache important folder paths
			if (assetsFolder.IsNullOrEmpty())
			{
				assetsFolder = Application.dataPath; // cache so we can use it off the main thread
				projectFolder = assetsFolder.Remove("Assets");
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			CancelOperation();
			if (cts != null)
			{
				cts.Dispose();
				cts = null;
			}

			log = null;
		}

		[Button, EnableIf(nameof(AsyncOperationInProgress))]
		private void CancelOperation()
		{
			if (cts != null)
			{
				Log("Cancelling...");
				cts.Cancel();
			}

			AsyncOperationInProgress = false;
		}

		#region Logging

		private void Log(string message) => Log(message, this);

		private void Log(string message, Object context)
		{
			log = message;
			Debug.Log(LogPrefix + message, context);
		}

		#endregion Logging

		#region Search Helpers

		private bool IsSearchingAssets => searchQuery.HasFlag(ESearch.Assets);
		private bool IsSearchingScenes => searchQuery.HasFlag(ESearch.Scenes);

		#endregion Search Helpers

		#region Cache Management

		[Button, DisableIf(nameof(AsyncOperationInProgress)),
			HorizontalGroup(DatabaseButtonGroup)]
		private void ClearCache()
		{
			if (CachedFilesCount > 0)
			{
				assetFiles.Clear();
				sceneFiles.Clear();
				Log("Cache cleared");
			}
		}

		private async UniTask CheckCacheNeedsBuilding()
		{
			bool needsBuilding = (IsSearchingAssets && assetFiles.IsEmpty())
				|| (IsSearchingScenes && sceneFiles.IsEmpty());

			if (needsBuilding)
				await BuildDatabaseAsync(searchQuery);
		}

		#endregion Cache Management

		/// <remarks>Must be called from the main thread.</remarks>
		private void BeginAsyncOperation()
		{
			AsyncOperationInProgress = true;
			progressBar = 0;
			asyncOperationStartTime = DateTime.Now;

			// init the cancellation token
			if (cts != null)
			{
				cts.Cancel();
				cts.Dispose();
			}
			cts = new CancellationTokenSource();

			UpdateProgressBar();
			Repaint();
		}

		/// <remarks>Must be called from the main thread.</remarks>
		private void EndAsyncOperation(string log)
		{
			AsyncOperationInProgress = false;
			cts = null;
			progressBar = 0;

			// calculate operation duration
			TimeSpan duration = DateTime.Now - asyncOperationStartTime;
			float seconds = (float)duration.TotalSeconds;

			Log($"{log} ({seconds:n2}s)");
			Repaint();
		}

		#region File Readers

		[Button, DisableIf(nameof(AsyncOperationInProgress)),
			HorizontalGroup(DatabaseButtonGroup)]
		private void BuildDatabase()
		{
			RunBuildDatabaseAsync().Forget();
		}

		private async UniTaskVoid RunBuildDatabaseAsync()
		{
			BeginAsyncOperation();

			try
			{
				await BuildDatabaseAsync(searchQuery);
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, this);
			}

			await UniTask.SwitchToMainThread();
			EndAsyncOperation($"Cached {CachedFilesCount:n0} asset files.");
		}

		private async UniTask ReadAssetFilesAsync(string[] assetFilePaths, List<AssetInfo> results)
		{
			using (var _lock = new SemaphoreSlim(1, 1))
			{
				int index = -1;

				async UniTask DoWork()
				{
					while (!cts.IsCancellationRequested)
					{
						int myIndex = Interlocked.Increment(ref index);

						// exit condition
						if (myIndex >= assetFilePaths.Length)
							break;

						// update bar
						string path = assetFilePaths[myIndex];
						var info = new AssetInfo(path, File.ReadAllText(path));

						// concurrently add it to the list
						await _lock.WaitAsync(cts.Token);
						results.Add(info);
						_lock.Release();

						progressBar = (myIndex / (float)assetFilePaths.Length) * 100;
					}

					cts.Token.ThrowIfCancellationRequested();
				}

				// run the workers
				await RunJobs(DoWork);
			}
		}

		private async UniTask BuildDatabaseAsync(ESearch search)
		{
			ClearCache();

			Log("Building file database...");

			// are we searching assets?
			if (search.HasFlag(ESearch.Assets))
			{
				string[] files = await GetAllAssetFilesAsync();
				await ReadAssetFilesAsync(files, assetFiles);
			}

			// are we searching scenes?
			if (search.HasFlag(ESearch.Scenes))
			{
				string[] files = Directory.GetFiles(assetsFolder + "\\Scenes");
				await ReadAssetFilesAsync(files, sceneFiles);
			}
		}

		private static UniTask<string[]> GetAllSceneAssetFilesAsync()
		{
			return UniTask.RunOnThreadPool(() =>
			{
				string sceneFolder = assetsFolder + "\\Scenes";
				return Directory.GetFiles(sceneFolder, "*.*", SearchOption.AllDirectories)
					.Where((f) => f.QuickEndsWith(".unity"))
					.ToArray();
			});
		}

		private UniTask<string[]> GetAllAssetFilesAsync()
		{
			return UniTask.RunOnThreadPool(() =>
			{
				return Directory.GetFiles(assetsFolder, "*.*", SearchOption.AllDirectories)
					.Where((f) => IsAssetFileWeCareAbout(f))
					.ToArray();
			}, configureAwait: false, cts.Token);
		}

		private static bool IsAssetFileWeCareAbout(string path)
		{
			// explicit exclusions
			if (path.QuickEndsWith(".meta") // ignore all meta files (early-exit, since more than half of all files are metas)
				|| path.ContainsOrdinal("Scenes") // ignore all scenes (use 'Scenes' flag)
				|| path.ContainsOrdinal("AddressableAssetsData") // ignore addressable assets
				|| path.QuickEndsWith(".unity") // scene files (tracked separately)
				|| path.QuickEndsWith("LightingData.asset") // ignore light bakes
				|| path.QuickEndsWith("NavMesh.asset") // ignore nav mes
				|| path.QuickEndsWith("OcclusionCullingData.asset"))
				return false;

			// inclusions
			return path.QuickEndsWith(".prefab") // prefabs
				|| path.QuickEndsWith(".controller") // animation controllers
				|| path.QuickEndsWith(".anim") // unity-made animations
				|| path.QuickEndsWith(".asset") // scriptable objects
				|| path.QuickEndsWith(".mat"); // materials
		}

		#endregion File Readers

		private UniTask RunJobs(Func<UniTask> job)
		{
			var tasks = new List<UniTask>(workerThreadCount);
			for (int i = 0; i < workerThreadCount; ++i)
				tasks.Add(UniTask.RunOnThreadPool(job, configureAwait: false, cts.Token));

			return UniTask.WhenAll(tasks);
		}

		private UniTask RunJobs(Action job)
		{
			var tasks = new List<UniTask>(workerThreadCount);
			for (int i = 0; i < workerThreadCount; ++i)
				tasks.Add(UniTask.RunOnThreadPool(job, configureAwait: false, cts.Token));

			return UniTask.WhenAll(tasks);
		}

		#region Searching

		private static bool CheckForGuidInFile(string guidQuery, string sourceFileContents)
		{
			return sourceFileContents.ContainsOrdinal(guidQuery);
		}

		[Button(ButtonSizes.Large), DisableIf(nameof(AsyncOperationInProgress))]
		private void SearchForAssetReferences()
		{
			switch (mode)
			{
				case EMode.Single:
					SearchForAssetReferencesSingleMode();
					break;
				case EMode.Batch:
					SearchForAssetReferencesBatchMode();
					break;
				default:
					throw ExceptionUtilities.GetInvalidEnumCaseException(mode);
			}
		}

		#region Batch Mode

		private void SearchForAssetReferencesBatchMode()
		{
			// validate
			if (targets.IsEmpty())
			{
				Debug.LogError($"{nameof(targets)} is empty. Please assign at least one asset to look for.");
				return;
			}

			// operate
			SearchForAssetReferencesBatchModeAsync().Forget();
		}

		private async UniTaskVoid SearchForAssetReferencesBatchModeAsync()
		{
			Log("~~~~~Start batch mode~~~~~");

			int totalCount = 0;
			foreach (Object target in targets)
			{
				if (target == null)
					continue;

				totalCount += await SearchForAssetReferencesAsync(target);
			}

			// report
			string note = totalCount > 0 ? " (see logs)" : "";
			Log($"Total references found: {totalCount}{note}");
			Log("~~~~~Batch complete~~~~~");
		}

		private void ClearBatchList() => targets = Array.Empty<Object>();

		#endregion Batch Mode

		private void SearchForAssetReferencesSingleMode()
		{
			// validate
			if (target == null)
			{
				log = "Target is null. Assign a value first.";
				Debug.LogError(LogPrefix + log, this);
				return;
			}

			SearchForAssetReferencesAsync(target).Forget();
		}

		private async UniTask<int> SearchForAssetReferencesAsync(Object target)
		{
			string assetPath = AssetDatabase.GetAssetPath(target);
			string guid = AssetDatabase.AssetPathToGUID(assetPath);
			Log($"Searching for references to {target.name} ({guid})...", target);

			BeginAsyncOperation();
			int count = 0;

			try
			{
				await CheckCacheNeedsBuilding();
				count = await SearchAsync(guid, EnumerateAssetInfos(searchQuery));
			}
			catch (Exception ex)
			{
				Debug.LogException(ex, this);
			}

			await UniTask.SwitchToMainThread();
			EndAsyncOperation(BuildReportLog(target, count));

			return count;
		}

		private string BuildReportLog(Object target, int count)
		{
			string assetName = target.name;

			// use colors to make log easier to eyeball
			string quantityString = count.ToStringCached();
			string countString = count == 0
				? quantityString.Orange() // make 0 stand out
				: quantityString.LightBlue();

			return $"Search complete. Found {countString} references to `{assetName}`.";
		}

		private IEnumerable<AssetInfo> EnumerateAssetInfos(ESearch search)
		{
			// janky hack to get progress updating
			int count = 0;

			// are we searching assets
			if (search.HasFlag(ESearch.Assets))
			{
				// generate asset files
				foreach (AssetInfo asset in assetFiles)
				{
					yield return asset;
					progressBar = ++count / (float)CachedFilesCount * 100;
				}
			}

			// are we searching scenes
			if (search.HasFlag(ESearch.Scenes))
			{
				// generate scene files
				foreach (AssetInfo scene in sceneFiles)
				{
					yield return scene;
					progressBar = count++ / (float)CachedFilesCount * 100;
				}
			}
		}

		private async UniTask<int> SearchAsync(string guidQuery, IEnumerable<AssetInfo> info)
		{
			int count = 0; // result

			// look inside every file contents for guid threaded
			using (IEnumerator<AssetInfo> enumerator = info.GetEnumerator())
			using (var _lock = new SemaphoreSlim(1, 1))
			{
				async UniTask DoWork()
				{
					AssetInfo item;

					while (true)
					{
						// wait at the gate to the critical section
						await _lock.WaitAsync(cts.Token);

						try
						{
							// try to get the next item
							if (!enumerator.MoveNext())
								break;

							// we got it, so do work
							item = enumerator.Current;
						}
						finally
						{
							// always release the semiphore
							_lock.Release();
						}

						// check for a reference inside the file contents
						if (CheckForGuidInFile(guidQuery, item.Contents))
						{
							LogReferenceFound(item).Forget(); // we don't need to wait for it to log
							Interlocked.Increment(ref count);
						}
					}
				}

				await RunJobs(DoWork);
			}

			return count;
		}

		private async UniTaskVoid LogReferenceFound(AssetInfo item)
		{
			await UniTask.SwitchToMainThread();
			string assetPath = GetRelativeAssetPath(item.Path);
			Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
			string typeName = asset.GetType().Name;
			Log($"Referenced by {asset.name} ({typeName})".Green(), asset);
		}

		#endregion Searching

		private string GetRelativeAssetPath(string sourcePath)
		{
			return sourcePath.Remove(projectFolder);
		}

		private void UpdateProgressBar()
		{
            UniTaskExtensions2.LoopAsync(Repaint, () => AsyncOperationInProgress, RepaintInterval)
				.Forget();
		}

		public readonly struct AssetInfo
		{
			public readonly string Path;
			public readonly string Contents;

			public AssetInfo(string filePath, string contents)
			{
				Path = filePath;
				Contents = contents;
			}
		}

		[Flags]
		private enum ESearch
		{
			/// <summary>
			/// Search everything.
			/// </summary>
			Assets = 1 << 1,

			/// <summary>
			/// Only search scenes.
			/// </summary>
			Scenes = 1 << 2,
			// ...
		}

		private enum EMode
		{
			/// <summary>
			/// Search for a single asset.
			/// </summary>
			Single = 0,

			/// <summary>
			/// Search for a group of assets.
			/// </summary>
			Batch = 1,
		}
	}
}
