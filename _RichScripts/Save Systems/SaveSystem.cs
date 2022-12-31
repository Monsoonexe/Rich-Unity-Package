using System;
using System.Collections.Generic;
using UnityEngine;
using RichPackage.Events.Signals;
using RichPackage.SaveSystem.Signals;
using Sirenix.OdinInspector;
using RichPackage.Debugging;
//using RichPackage.GuardClauses;

/*
 * Cache Save/Load signals to reduce dictionary lookups
 * Support saving to different save slots (Bethesda style)
 */ 

namespace RichPackage.SaveSystem
{ 
	/// <summary>
	/// I facilitate saving and manage save files.
	/// </summary>
	public class SaveSystem : ASaveableMonoBehaviour<SaveSystem.SaveSystemData>
	{
		#region Constants

		private static readonly string HAS_SAVE_DATA_KEY = "HasData";
		private static readonly string DEFAULT_SAVE_FILE_NAME = "Save.es3";

		#endregion	

		//singleton
		private static SaveSystem instance;
		public static SaveSystem Instance 
		{
			get => instance;
			private set => instance = value;
		}

		[Serializable]
		public class SaveSystemData : AState
		{
			[SerializeField]
			private string[] saveFiles;

			public string[] SaveFiles 
			{ 
				get => saveFiles; 
				set
				{
					saveFiles = value;
					IsDirty = true;
				}
			}

			public SaveSystemData() : this(new string[0]) { }

			public SaveSystemData(string[] saveFiles) : base()
			{
				this.saveFiles = saveFiles;
				IsDirty = true;
			}
		}

		[SerializeField]
		private List<ES3SerializableSettings> gameSaveFiles = new List<ES3SerializableSettings>
				{ new ES3SerializableSettings(DEFAULT_SAVE_FILE_NAME) };

		[Title("System Save Data")]
		[SerializeField, Tooltip("File that stores meta information about the actual save files themselves.")]
		private ES3SerializableSettings saveDataMetaInformation;

		[Title("Settings")]
		public bool deleteOnPlay = false;
		public bool loadOnStart = false;

		[SerializeField, Min(0)]
		private int saveGameSlotIndex = 0;

		[SerializeField, Min(0)]
		private int maxSaveFiles = 3;

		public int MaxSaveFiles { get => maxSaveFiles; }

		/// <summary>
		/// Save file which is currently loaded. If it's null, it needs to be loaded from disk.
		/// </summary>
		private ES3File currentSaveFile;

		/// <summary>
		/// True if there exists some amount of SaveData which can be loaded.
		/// </summary>
		public static bool HasSaveData
		{
			get => instance.SaveFile.Load<bool>(HAS_SAVE_DATA_KEY, false);
		}

		/// <summary>
		/// Gets the current save file that is loaded.
		/// </summary>
		private ES3File SaveFile
		{
			get
			{
				//work
				if (currentSaveFile == null)
					LoadFile(saveGameSlotIndex);
				return currentSaveFile;
			}
		}

		#region Unity Messages

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("I facilitate saving and manage save files.");

			//first slot is free
			gameSaveFiles = gameSaveFiles ?? new List<ES3SerializableSettings>
				{ new ES3SerializableSettings(DEFAULT_SAVE_FILE_NAME) };
		}

		protected override void Awake()
		{
			base.Awake();

			if (!Singleton.TakeOrDestroy(this, ref instance,
                dontDestroyOnLoad: true))
			{
                // quit here if this was the duplicate that should be destroyed
                return;
			}

			if (deleteOnPlay)
				DeleteSave();

			LoadMetaInformation();
		}

		private void OnDestroy()
		{
			SaveMetaInformation(); //just to be safe
			Singleton.Release(this, ref instance);
		}

		private void Start()
		{
			if (loadOnStart)
				Load();
		}

		protected override void OnEnable()
		{
			//subscribe to events
			GlobalSignals.Get<SaveGameSignal>().AddListener(Save);
			GlobalSignals.Get<ScenePreUnloadSignal>().AddListener(Save);
			GlobalSignals.Get<SceneLoadedSignal>().AddListener(Load);
			GlobalSignals.Get<SaveObjectStateSignal>().AddListener(SaveMe);
		}

		protected override void OnDisable()
		{
			//unsubscribe from events
			GlobalSignals.Get<SaveGameSignal>().RemoveListener(Save);
			GlobalSignals.Get<ScenePreUnloadSignal>().RemoveListener(Save);
			GlobalSignals.Get<SceneLoadedSignal>().RemoveListener(Load);
			GlobalSignals.Get<SaveObjectStateSignal>().RemoveListener(SaveMe);
		}

		#endregion Unity Messages

		private void LoadFile(int slot)
		{
			//validate
			//GuardAgainst.IndexOutOfRange(gameSaveFiles, slot);

			//work
			currentSaveFile = new ES3File(gameSaveFiles[slot]);
		}

		/// <summary>
		/// Triggered by dispatching <see cref="SaveGameSignal"/>.
		/// </summary>
		[Button, DisableInEditorMode]
		public void Save()
		{
			SaveFile.Save(HAS_SAVE_DATA_KEY, value: true);//flag to indicate there is indeed some save data
			GlobalSignals.Get<SaveStateToFileSignal>().Dispatch(SaveFile);//broadcast save command
			SaveFile.Sync();//save from RAM to Disk
			RichDebug.EditorLog($"Saved file: {SaveFile.settings.path}.");
		}

		[Button, HorizontalGroup("Delete")]
		public void DeleteSave() => DeleteSave(saveGameSlotIndex);

		[Button, HorizontalGroup("Delete", 0.5f)]
		public void DeleteSave(int slot)
		{
			//validate
			//gameSaveFiles.AssertValidIndex(slot);

			//work
			ES3File saveFile = new ES3File(gameSaveFiles[slot]); //load into memory
			saveFile.Clear();
			saveFile.Sync(); //needed?

			Debug.Log($"Deletedfile: {saveFile.settings.path}.");
		}

		[Button, DisableInEditorMode, HorizontalGroup("Load")]
		public void Load()
		{
			currentSaveFile?.Sync(); //flush cache to disk before switching save files.
			LoadFile(saveGameSlotIndex);

			//broadcast load command
			GlobalSignals.Get<LoadStateFromFileSignal>().Dispatch(SaveFile);

			//load player inventory
			RichDebug.EditorLog($"Loaded file: {SaveFile.settings.path}.");
		}

		[Button, DisableInEditorMode, HorizontalGroup("Load", 0.5f)]
		public void Load(int slot)
		{
			saveGameSlotIndex = slot;
			Load();
		}

		#region Meta Save Data

		[Button, HorizontalGroup("META")]
		public void SaveMetaInformation() => SaveState(new ES3File(saveDataMetaInformation));

		[Button, HorizontalGroup("META")]
		public void LoadMetaInformation() => LoadState(new ES3File(saveDataMetaInformation));

		[Button, HorizontalGroup("META")]
		public void DeleteMetaInformation() => (new ES3File(saveDataMetaInformation)).Clear();

		public override void SaveState(ES3File saveFile)
		{
			//always update state
			SaveData.SaveFiles = gameSaveFiles.ToSubArray((file) => file.FullPath);

			//save to raw default file
			if (SaveData.IsDirty)
				saveFile.Save(SaveID, SaveData);
			SaveData.IsDirty = false;
			saveFile.Sync();
		}

		public override void LoadState(ES3File saveFile)
		{
			currentSaveFile?.Sync();
			if (saveFile.KeyExists(SaveID))
			{
				saveFile.LoadInto(SaveID, SaveData);
				gameSaveFiles.Clear();
				//load existing files
				foreach (var f in SaveData.SaveFiles)
					gameSaveFiles.Add(new ES3SerializableSettings(f));
				currentSaveFile = null;
			}
			//initialize
		}

		#endregion Meta Save Data

		/// <summary>
		/// Checks to see if there is any save data in the active file.
		/// </summary>
		/// <returns>True if the save file has any data in it, and false if it's unused.</returns>
		public bool FileHasSaveData() => SaveFile.Load(HAS_SAVE_DATA_KEY, defaultValue: false);

		/// <summary>
		/// Checks to see if there is any save data in the file at the given slot.
		/// </summary>
		/// <returns>True if the save file has any data in it, and false if it's unused.</returns>
		public bool FileHasSaveData(int slot)
		{
			Debug.Assert(slot < maxSaveFiles && slot < gameSaveFiles.Count);
			saveGameSlotIndex = slot;
			return SaveFile != null && SaveFile.Load(HAS_SAVE_DATA_KEY, defaultValue: false);
		}

		#region ISaveable Consumers

		/// <summary>
		/// Save <paramref name="item"/> to the currently active <see cref="SaveFile"/>.
		/// </summary>
		public void SaveMe(ISaveable item) => item.SaveState(SaveFile);

		/// <summary>
		/// Load <paramref name="item"/> from the currently active <see cref="SaveFile"/>.
		/// </summary>
		public void LoadMe(ISaveable item) => item.LoadState(SaveFile);

		/// <summary>
		/// Delete <paramref name="item"/> from the currently active <see cref="SaveFile"/>.
		/// </summary>
		public void DeleteMe(ISaveable item) => item.LoadState(SaveFile);

		#endregion ISaveable Consumers

		[QFSW.QC.Command("openSaveFile"), Button]
		public static void OpenSaveFileInVSCode()
		{
			SaveSystem ins = Instance != null ? Instance : FindObjectOfType<SaveSystem>();
			if (ins == null)
			{
				Debug.LogWarning("No SaveSystem in Scene.");
				return;
			}

			ins.LoadFile(ins.saveGameSlotIndex);
			string arg = $"\"code '\"{ins.SaveFile.settings.FullPath}\"'\""; //wrap in double-quotes to pass string with spaces as single argument
			var options = new System.Diagnostics.ProcessStartInfo()
			{
				FileName = "powershell",
				Arguments = arg,
				UseShellExecute = false,
			};
			var process = System.Diagnostics.Process.Start(options);
			process.EnableRaisingEvents = true;
			process.Exited += (obj, ctx) => ((System.Diagnostics.Process)obj).Dispose();
		}
	}
}
