﻿using System.Collections.Generic;
using UnityEngine;
using Signals;
using RichPackage.SaveSystem.Signals;
using Sirenix.OdinInspector;
using UnityConsole;
using RichPackage.Debugging;

/*
 * Cache Save/Load signals to reduce dictionary lookups
 * Support saving to different save slots (Bethesda style)
 */ 

namespace RichPackage.SaveSystem
{ 
	/// <summary>
	/// I facilitate saving and manage save files.
	/// </summary>
	public class SaveSystem : RichMonoBehaviour
	{
		#region Constants
		private static readonly string HAS_SAVE_DATA_KEY = "HasData";
		private static readonly string DEFAULT_SAVE_FILE_NAME = "Save.es3";
		#endregion	

		//singleton
		private static SaveSystem instance;
		public static SaveSystem Instance { get => instance; }

		[SerializeField]
		private List<ES3SerializableSettings> gameSaveFiles = new List<ES3SerializableSettings>
				{ new ES3SerializableSettings(DEFAULT_SAVE_FILE_NAME) };

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
			get => instance.SaveFile.KeyExists(HAS_SAVE_DATA_KEY) 
				&& instance.SaveFile.Load<bool>(HAS_SAVE_DATA_KEY);
		}

		private string SaveFileName
		{
			get => null;// gameSaveFiles[saveGameSlotIndex].fileName;
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

		private void Reset()
		{
			SetDevDescription("I facilitate saving and manage save files.");

			//first slot is free
			gameSaveFiles = gameSaveFiles ?? new List<ES3SerializableSettings>
				{ new ES3SerializableSettings(DEFAULT_SAVE_FILE_NAME) };
		}

		protected override void Awake()
		{
			base.Awake();
			//singleton pattern
			var singletonWorked = InitSingleton(this, ref instance, 
				dontDestroyOnLoad: true); //ensure only one of these exists

			if (!singletonWorked)
			{
				Destroy(this);
				return; //quit here if this was the duplicate that should be destroyed
			}

			if (deleteOnPlay)
				DeleteSave();
		}

		private void Start()
		{
			if (loadOnStart)
				Load();
		}

		private void OnEnable()
		{
			//subscribe to events
			GlobalSignals.Get<SaveGame>().AddListener(Save);
			GlobalSignals.Get<SceneLoadedSignal>().AddListener(Load);
		}

		private void OnDisable()
		{
			//unsubscribe from events
			GlobalSignals.Get<SaveGame>().RemoveListener(Save);
			GlobalSignals.Get<SceneLoadedSignal>().RemoveListener(Load);
		}

		#endregion

		private void LoadFile(int slot)
		{
			//validate
			gameSaveFiles.AssertValidIndex(slot);

			//work
			if (currentSaveFile == null)
				currentSaveFile = new ES3File(gameSaveFiles[slot]);
		}

		/// <summary>
		/// Triggered by dispatching <see cref="SaveGame"/>.
		/// </summary>
		[Button, DisableInEditorMode]
		public void Save()
		{
			SaveFile.Save(HAS_SAVE_DATA_KEY, value: true);//flag to indicate there is indeed some save data
			GlobalSignals.Get<SaveStateToFile>().Dispatch(SaveFile);//broadcast save command
			SaveFile.Sync();//save from RAM to Disk
			RichDebug.EditorLog($"Saved file: {SaveFile.settings.path}.");
		}

		[Button, HorizontalGroup("Delete")]
		public void DeleteSave() => DeleteSave(saveGameSlotIndex);

		[Button, HorizontalGroup("Delete")]
		public void DeleteSave(int slot)
		{
			//validate
			gameSaveFiles.AssertValidIndex(slot);

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
			GlobalSignals.Get<LoadStateFromFile>().Dispatch(SaveFile);

			//load player inventory
			RichDebug.EditorLog($"Loaded file: {SaveFile.settings.path}.");
		}

		[Button, DisableInEditorMode, HorizontalGroup("Load")]
		public void Load(int slot)
		{
			saveGameSlotIndex = slot;
			Load();
		}

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

		#endregion

		#region Console Commands

		[ConsoleCommand("save")]
		public static void Save_()
		{
			if (Instance)
				Instance.Save();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[ConsoleCommand("load")]
		public static void Load_()
		{
			if(Instance)
				Instance.Load();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[ConsoleCommand("loadSlot")]
		public static void Load_(int slot)
		{
			if(Instance)
				Instance.Load(slot);
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[ConsoleCommand("deleteSave")]
		public static void DeleteSave_()
		{
			if (Instance)
				Instance.DeleteSave();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[ConsoleCommand("deleteSaveSlot")]
		public static void DeleteSave_(int slot)
		{
			if (Instance)
				Instance.DeleteSave(slot);
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[ConsoleCommand("openSaveFile"), Button, DisableInEditorMode]
		public static void OpenSaveFileInVSCode()
		{
			string arg = $"\"code '\"{Instance.SaveFile.settings.FullPath}\"'\""; //wrap in double-quotes to pass string with spaces as single argument
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

		#endregion
	}
}
