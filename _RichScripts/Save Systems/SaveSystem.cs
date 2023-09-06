/* https://docs.moodkie.com/product/easy-save-3/ 
 */

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using RichPackage.Events.Signals;
using RichPackage.SaveSystem.Signals;
using Sirenix.OdinInspector;
using RichPackage.GuardClauses;
using System.Linq;

namespace RichPackage.SaveSystem
{
	/// <summary>
	/// I facilitate saving and manage save files.
	/// </summary>
	public class SaveSystem : RichMonoBehaviour,
		ISaveSystem
	{
		#region Constants

		private const string HAS_SAVE_DATA_KEY = "HasData";
		private const string DEFAULT_SAVE_FILE_NAME = "Save.es3";
		private const string SaveFileExtension = ".es3";

		#endregion	

		// singleton
		private static SaveSystem s_instance;
		public static SaveSystem Instance
		{
			get => s_instance;
			private set => s_instance = value;
		}

		[ShowInInspector, HideInEditorMode]
		private readonly List<ES3SerializableSettings> gameSaveFiles = new List<ES3SerializableSettings>();

		[Title("Settings")]
		public bool debug = false;
		public bool deleteOnPlay = false;
		public bool loadOnStart = false;

		[Title("Save File Settings")]
		public ES3.EncryptionType encryptionType;
		public string encryptionPassword = "ScaryPassword";

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
		public static bool SaveDataExists
		{
			get => s_instance.SaveFile.Load(HAS_SAVE_DATA_KEY, false);
		}

		/// <summary>
		/// Gets the current save file that is loaded.
		/// </summary>
		public ES3File SaveFile
		{
			get
			{
				// work
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
		}

		protected override void Awake()
		{
			base.Awake();

			if (!Singleton.TakeOrDestroy(this, ref s_instance,
                dontDestroyOnLoad: true))
			{
                // quit here if this was the duplicate that should be destroyed
                return;
			}

#if UNITY_EDITOR
			// clear the current save for easier iteration
			if (deleteOnPlay)
				DeleteSave();
#endif

            LoadSaveFilePaths();
			Load(DEFAULT_SAVE_FILE_NAME);
		}

		private void OnDestroy()
		{
			Singleton.Release(this, ref s_instance);
		}

		private void Start()
		{
			if (loadOnStart)
				Load();
		}

		protected void OnEnable()
		{
			// subscribe to events
			GlobalSignals.Get<SaveGameSignal>().AddListener(Save);
			// GlobalSignals.Get<OnLevelLoadedSignal>().AddListener(Load);
			GlobalSignals.Get<SaveObjectStateSignal>().AddListener(Save);
		}

		protected void OnDisable()
		{
			// unsubscribe from events
			GlobalSignals.Get<SaveGameSignal>().RemoveListener(Save);
			// GlobalSignals.Get<OnLevelLoadedSignal>().RemoveListener(Load);
			GlobalSignals.Get<SaveObjectStateSignal>().RemoveListener(Save);
		}

        private void OnApplicationQuit()
        {
			Sync();
        }

        #endregion Unity Messages

        private ES3SerializableSettings CreateNewSettings(string fileName = DEFAULT_SAVE_FILE_NAME)
		{
			return new ES3SerializableSettings(fileName)
			{
				encryptionType = encryptionType,
				encryptionPassword = encryptionPassword,
			};
		}

		public void LoadFile(int slot)
		{
			// validate
			GuardAgainst.IndexOutOfRange(gameSaveFiles, slot);

			// operate
			currentSaveFile = CreateSaveFileObject(gameSaveFiles[slot]);

			if (debug)
				Debug.Log($"Loaded file at slot {slot} '{currentSaveFile.settings.path}'.");
        }

		private void Load(ES3SerializableSettings saveFile)
        {
			// operate
			currentSaveFile = CreateSaveFileObject(saveFile);
        }

		/// <param name="fileName">The name of the save file with no extension.</param>
		public void Load(string fileName)
		{
			LoadSaveFilePaths();
            ES3SerializableSettings saveFile = gameSaveFiles
                .Where(settings => Path.GetFileNameWithoutExtension(settings.FullPath).QuickEquals(fileName))
				.FirstOrDefault();

			// if not found
			if (saveFile == null)
			{
				Debug.LogError($"Could not find a save file with the name {saveFile}.");
				return;
			}

			Load(saveFile);
        }

        [Button, DisableInEditorMode, HorizontalGroup("Load")]
        public void Load()
        {
            LoadFile(saveGameSlotIndex);

            // broadcast load command
            GlobalSignals.Get<LoadStateFromFileSignal>().Dispatch(SaveFile);

            if (debug)
                Debug.Log($"Loaded file: {SaveFile.settings.path}.");
        }

        [Button, DisableInEditorMode, HorizontalGroup("Load", 0.5f)]
        public void Load(int slot)
        {
            saveGameSlotIndex = slot;
            Load();
        }

        /// <summary>
        /// Triggered by dispatching <see cref="SaveGameSignal"/>.
        /// </summary>
        [Button, DisableInEditorMode]
		public void Save()
		{
			SaveFile.Save(HAS_SAVE_DATA_KEY, value: true); // flag to indicate there is indeed some save data
			GlobalSignals.Get<SaveStateToFileSignal>().Dispatch(SaveFile); // broadcast save command
			SaveFile.Sync(); // save from RAM to Disk

            if (debug)
                Debug.Log($"Saved file: {SaveFile.settings.path}.");
		}

		[Button, HorizontalGroup("Delete")]
		public void DeleteSave() => DeleteSave(saveGameSlotIndex);

		[Button, HorizontalGroup("Delete", 0.5f)]
		public void DeleteSave(int slot)
		{
			// operate
			var saveFile = CreateSaveFileObject(gameSaveFiles[slot]); //load into memory
			saveFile.Clear(); // clear any lingering data
            saveFile.Sync(); // sync with file system

            if (debug)
                Debug.Log($"Deletedfile: {saveFile.settings.path}.");
		}

        [Button, DisableInEditorMode]
        public void Sync() => SaveFile.Sync();

        #region Meta Save Data

		private ES3File CreateSaveFileObject(ES3SerializableSettings settings)
        {
            try
            {
                return new ES3File(settings);
            }
            catch (Exception ex)
            {
				Debug.LogError($"Encountered a {ex.GetType()} while attempting to read save file. Wiping corrupted save file.");
				Debug.LogException(ex);

                // I encountered a corrupted file issue during a crash that prevented
                // loading the meta info file, which broke the save system entirely.
                // If anything goes wrong and this file can't be loaded,
                // the nuclear option is proposed
                string filePath = settings.FullPath;
                switch (settings.location)
                {
                    case ES3.Location.File:
						// ensure we only delete data in the appropriate folder
                        if (filePath.Contains(Application.persistentDataPath)
							&& File.Exists(filePath))
						{
                            File.Delete(filePath);
						}
                        break;
                    case ES3.Location.PlayerPrefs:
						PlayerPrefs.DeleteKey(filePath);
                        break;
                    case ES3.Location.InternalMS:
                    case ES3.Location.Resources:
                    case ES3.Location.Cache:
						// shouldn't happen
                        break;
                }
            }

            return new ES3File(settings); // if this fails, god help us all
        }

        #region Save/Load

        protected void LoadSaveFilePaths()
        {
            gameSaveFiles.Clear();

            //load existing files
            foreach (string file in EnumerateSaveFiles())
                gameSaveFiles.Add(CreateNewSettings(file));

            currentSaveFile = null;
        }

		private IEnumerable<string> EnumerateSaveFiles()
        {
            // look for all of the save files in the persistent data location
            return Directory.EnumerateFiles(Application.persistentDataPath)
                .Where(filePath => Path.GetExtension(filePath).QuickEquals(SaveFileExtension));
        }

        #endregion Save/Load

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
			return SaveFile != null && FileHasSaveData();
		}

        #region ISaveable Consumers

        /// <summary>
        /// Save <paramref name="item"/> to the currently active <see cref="SaveFile"/>.
        /// </summary>
        public void Save(ISaveable item) => item.SaveState(SaveFile);

		/// <summary>
		/// Load <paramref name="item"/> from the currently active <see cref="SaveFile"/>.
		/// </summary>
		public void Load(ISaveable item) => item.LoadState(SaveFile);

		/// <summary>
		/// Delete <paramref name="item"/> from the currently active <see cref="SaveFile"/>.
		/// </summary>
		public void Delete(ISaveable item) => item.DeleteState(SaveFile);

        #endregion ISaveable Consumers

        #region ISaveSystem

        void ISaveSystem.Save<T>(string key, T memento) => SaveFile.Save(key, memento);
        T ISaveSystem.Load<T>(string key) => SaveFile.Load<T>(key);
        T ISaveSystem.Load<T>(string key, T @default) => SaveFile.Load<T>(key, @default);
		void ISaveSystem.LoadInto<T>(string key, T memento) where T : class 
			=> SaveFile.LoadInto(key, memento);
        bool ISaveSystem.Contains(string key) => SaveFile.KeyExists(key);
        void ISaveSystem.Delete(string key) => SaveFile.DeleteKey(key);
		void ISaveSystem.Clear() => SaveFile.Clear();

        #endregion ISaveSystem

        [QFSW.QC.Command("openSaveFile"), Button]
		public static void OpenSaveFile()
		{
			SaveSystem ins = Instance != null ? Instance : FindObjectOfType<SaveSystem>();
			if (ins == null)
			{
				Debug.LogWarning("No SaveSystem in Scene.");
				return;
			}

			ins.LoadFile(ins.saveGameSlotIndex);
			System.Diagnostics.Process.Start(ins.SaveFile.settings.FullPath);
		}

    }
}
