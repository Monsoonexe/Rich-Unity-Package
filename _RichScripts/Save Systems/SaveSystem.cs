﻿using System.Collections.Generic;
using UnityEngine;
using Signals;
using RichPackage.SaveSystem.Signals;
using Sirenix.OdinInspector;
using UnityConsole;

namespace RichPackage.SaveSystem
{ 
	/// <summary>
	/// I facilitate saving and manage save files.
	/// </summary>
	public class SaveSystem : RichMonoBehaviour
	{
		#region Constants
		private static readonly string HAS_SAVE_DATA_KEY = "HasSave";
		private static readonly string DEFAULT_SAVE_FILE_NAME = "Save.es3";
		#endregion	

		//singleton
		private static SaveSystem instance;
		public static SaveSystem Instance { get => instance; }

		[System.Serializable]
		private class SaveGameSlot
		{
			public string fileName;
			public ES3File file;

			#region Constructors
			public SaveGameSlot(string fileName)
			{
				this.fileName = fileName;
				file = null;
			}

			public SaveGameSlot(string fileName, ES3File file)
			{
				this.fileName = fileName;
				this.file = file;
			}
			#endregion
		}

		[SerializeField]
		private List<SaveGameSlot> gameSaveFiles = new List<SaveGameSlot>
				{ new SaveGameSlot(DEFAULT_SAVE_FILE_NAME) };

		[Title("Settings")]
		public bool deleteOnPlay = false;
		public bool loadOnStart = false;

		[SerializeField]
		private int saveGameSlotIndex = 0;

		[SerializeField]
		private int maxSaveFiles = 3;

		public int MaxSaveFiles { get => maxSaveFiles; }

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
			get => gameSaveFiles[saveGameSlotIndex].fileName;
		}

		private ES3File SaveFile
		{
			get => gameSaveFiles[saveGameSlotIndex].file;
			set => gameSaveFiles[saveGameSlotIndex].file = value;
		}

		private void Reset()
		{
			SetDevDescription("I facilitate saving and manage save files.");

			//first slot is free
			gameSaveFiles = gameSaveFiles ?? new List<SaveGameSlot>
				{ new SaveGameSlot(DEFAULT_SAVE_FILE_NAME) };
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

		/// <summary>
		/// Change source save file.
		/// </summary>
		/// <param name="fileName"></param>
		private void LoadSaveFile(string fileName)
		{
			SaveFile = new ES3File(fileName);
		}

		public void DeleteSave(int slot)
		{
			Debug.Assert(slot < maxSaveFiles && slot < gameSaveFiles.Count);
			saveGameSlotIndex = slot;
			DeleteSave();
		}

		[Button]
		public void DeleteSave()
		{
			if (SaveFile == null)
				LoadSaveFile(SaveFileName);
			SaveFile.Clear();
			Debug.Log("Deleted save file.");
		}

		[Button, DisableInEditorMode]
		public void Save(int slot)
		{
			Debug.Assert(slot < maxSaveFiles && slot < gameSaveFiles.Count);
			saveGameSlotIndex = slot;
			Save();
		}

		[Button, DisableInEditorMode]
		private void Save()
		{
			if (SaveFile == null)
				LoadSaveFile(SaveFileName);

			//flag to indicate there is indeed some save data
			SaveFile.Save(HAS_SAVE_DATA_KEY, true);

			//broadcast save command
			GlobalSignals.Get<SaveStateToFile>().Dispatch(SaveFile);

			SaveFile.Sync();//synchronize file (like stamping)
			Debug.Log("Saved");
		}

		[Button, DisableInEditorMode]
		public void Load(int slot)
		{
			Debug.Assert(slot < maxSaveFiles && slot < gameSaveFiles.Count);
			saveGameSlotIndex = slot;
			Load();
		}

		[Button, DisableInEditorMode]
		public void Load()
		{
			if (SaveFile == null)
				LoadSaveFile(SaveFileName);

			SaveFile.Sync();//make sure up to date

			//broadcast load command
			GlobalSignals.Get<LoadStateFromFile>().Dispatch(SaveFile);

			//load player inventory
			Debug.Log("Loaded");
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
			return SaveFile.Load(HAS_SAVE_DATA_KEY, defaultValue: false);
		}
		
		#region Console Commands

		[ConsoleCommand("save")]
		public static void Save_()
		{
			if (Instance)
				Instance.Save();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[ConsoleCommand("saveSlot")]
		public static void Save_(int slot)
		{
			if (Instance)
				Instance.Save(slot);
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

		#endregion
	}
}
