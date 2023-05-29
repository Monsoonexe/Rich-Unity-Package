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

namespace RichPackage.SaveSystem
{
	/// <summary>
	/// I facilitate saving and manage save files.
	/// </summary>
	public class SaveSystem : ASaveableMonoBehaviour<SaveSystem.SaveSystemData>,
		ISaveSystem
	{
		#region Constants

		private const string HAS_SAVE_DATA_KEY = "HasData";
		private const string DEFAULT_SAVE_FILE_NAME = "Save.es3";

		#endregion	

		// singleton
		private static SaveSystem s_instance;
		public static SaveSystem Instance
		{
			get => s_instance;
			private set => s_instance = value;
		}

		[SerializeField]
		private List<ES3SerializableSettings> gameSaveFiles = new List<ES3SerializableSettings>();

		[Title("System Save Data")]
		[SerializeField, Tooltip("File that stores meta information about the actual save files themselves.")]
		private ES3SerializableSettings saveDataMetaInformation;

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

			//first slot is free
			gameSaveFiles = gameSaveFiles ?? new List<ES3SerializableSettings>
			{
				CreateNewSettings()
			};
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

			if (deleteOnPlay)
				DeleteSave();

			LoadMetaInformation();
		}

		private void OnDestroy()
		{
			SaveMetaInformation(); //just to be safe
			Singleton.Release(this, ref s_instance);
		}

		private void Start()
		{
			if (loadOnStart)
				Load();
		}

		protected override void OnEnable()
		{
			// subscribe to events
			GlobalSignals.Get<SaveGameSignal>().AddListener(Save);
			GlobalSignals.Get<OnLevelLoadedSignal>().AddListener(Load);
			GlobalSignals.Get<SaveObjectStateSignal>().AddListener(Save);
		}

		protected override void OnDisable()
		{
			// unsubscribe from events
			GlobalSignals.Get<SaveGameSignal>().RemoveListener(Save);
			GlobalSignals.Get<OnLevelLoadedSignal>().RemoveListener(Load);
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

		private void LoadFile(int slot)
		{
			// validate
			GuardAgainst.IndexOutOfRange(gameSaveFiles, slot);

			// operate
			currentSaveFile = CreateSaveFileObject(gameSaveFiles[slot]);

			if (debug)
				Debug.Log($"Loaded file at slot {slot} '{currentSaveFile.settings.path}'.");
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
			// validate
			// gameSaveFiles.AssertValidIndex(slot);

			// operate
			var saveFile = CreateSaveFileObject(gameSaveFiles[slot]); //load into memory
			saveFile.Clear();
			saveFile.Sync(); //needed?

            if (debug)
                Debug.Log($"Deletedfile: {saveFile.settings.path}.");
		}

		[Button, DisableInEditorMode, HorizontalGroup("Load")]
		public void Load()
		{
			currentSaveFile?.Sync(); // flush cache to disk before switching save files.
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

        [Button, DisableInEditorMode]
        public void Sync() => SaveFile.Sync();

        #region Meta Save Data

        [Button, HorizontalGroup("META")]
		public void SaveMetaInformation() => SaveState(CreateSaveFileObject(saveDataMetaInformation));

		[Button, HorizontalGroup("META")]
		public void LoadMetaInformation() => LoadState(CreateSaveFileObject(saveDataMetaInformation));

        [Button, HorizontalGroup("META")]
		public void DeleteMetaInformation() => (CreateSaveFileObject(saveDataMetaInformation)).Clear();

		private ES3File CreateSaveFileObject(ES3SerializableSettings settings)
        {
            try
            {
                return new ES3File(settings);
            }
            catch
            {
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

        protected override void LoadStateInternal()
        {
            gameSaveFiles.Clear();
            //load existing files
            foreach (var f in SaveData.SaveFiles)
                gameSaveFiles.Add(CreateNewSettings(f));
            currentSaveFile = null;
        }

        protected override void SaveStateInternal()
        {
            SaveData.SaveFiles = gameSaveFiles.ToSubArray((file) => file.FullPath);
        }

        public override void SaveState(ES3File saveFile)
		{
			// save to raw default file
			base.SaveState(saveFile);
			saveFile.Sync(); // flush cache
		}

		public override void LoadState(ES3File saveFile)
		{
			currentSaveFile?.Sync();
			base.LoadState(saveFile);
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

        public void Save<T>(string key, T memento) => SaveFile.Save(key, memento);
        public T Load<T>(string key) => SaveFile.Load<T>(key);
        public T Load<T>(string key, T @default) => SaveFile.Load<T>(key, @default);
		public void LoadInto<T>(string key, T memento) where T : class 
			=> SaveFile.LoadInto(key, memento);
        public bool Contains(string key) => SaveFile.KeyExists(key);
        public void Delete(string key) => SaveFile.DeleteKey(key);
		public void Clear() => SaveFile.Clear();

        #endregion ISaveSystem

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
                }
            }

            public SaveSystemData() : this(new string[0]) { }

            public SaveSystemData(string[] saveFiles) : base()
            {
                this.saveFiles = saveFiles;
            }
        }

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
