/* https://docs.moodkie.com/product/easy-save-3/ 
 *  TODO - separate out save file management from game state saving
 *  
 *  Notes:
 *  Always explicitly state whether the string contains an extension or not.
 *  This can be ui-facing, so all public methods should return file names without extensions unless explicit.
 *  
 *  Resist the urge to use QuickEquals in here
 *  
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
using UnityEngine.Assertions;
using RichPackage.YieldInstructions;
using System.Collections;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// I facilitate saving and manage save files.
    /// </summary>
    public class SaveSystem : RichMonoBehaviour, ISaveStore
    {
        #region Constants

        // there is a line in ES3 that deletes files from disk if the file is Sync'ed with 0 keys.
        private const string HAS_SAVE_DATA_KEY = "HasData";
        private const string DEFAULT_SAVE_FILE_NAME = "Save.es3";
        private const string SaveFileExtension = ".es3";

        #endregion Constants

        // singleton
        private static SaveSystem s_instance;
        public static SaveSystem Instance
        {
            get => s_instance;
            private set => s_instance = value;
        }

        [SerializeField]
        private ES3SerializableSettings settings; // always clone this

        /// <summary>
        /// All the known save files with extensions.
        /// </summary>
        [ShowInInspector,
            ListDrawerSettings(IsReadOnly = true),
            InlineButton(nameof(RescanForSaveFiles), "Scan"),
            CustomContextMenu("Sort", nameof(SortSaveFileNames)),
            Tooltip("Don't edit these.")]
        private readonly List<string> saveFileNames = new List<string>();

        [Title("Settings")]
        public bool debug = false;
        public bool syncOnQuit = true;
        public EStartBehaviour startBehaviour = EStartBehaviour.LoadGameOnStart;

        [Title("Save File Settings")]
        public ES3.EncryptionType encryptionType;
        public string encryptionPassword = "ScaryPassword";

        /// <summary>
        /// Use the property instead of this directly.
        /// </summary>
        private string _saveFileDirectory = null;

        /// <summary>
        /// The directory path for save files. Creates the path if it doesn't exist. Ends in a backslash.
        /// </summary>
        public string SaveFileDirectory
        {
            // need to lazy-fetch because of Unity API
            get
            {
                if (_saveFileDirectory.IsNullOrEmpty()) // sometimes unity serializes this to empty string :/
                    SaveFileDirectory = Application.persistentDataPath;
                return _saveFileDirectory;
            }
            set
            {
                GuardAgainst.ArgumentIsNull(value, nameof(value));
                if (Path.HasExtension(value))
                    throw new InvalidOperationException($"Expected a directory but saw a file path. '{value}'");

                if (_saveFileDirectory == value)
                    return;

                // ensure the folder exists
                EnsureFilePathExists(value + "x.txt"); // this method expects a file path, so give a dummy file name
                Assert.IsTrue(Directory.Exists(value)); // sanity check

                // ensure the directory ends in a backslash
                if (value.Last() is not '/' or '\\')
                    value += '/';

                _saveFileDirectory = value;
            }
        }

        public bool LoadFileOnAwake => startBehaviour == EStartBehaviour.LoadFileOnAwake || LoadGameOnStart;
        public bool LoadGameOnStart => startBehaviour == EStartBehaviour.LoadGameOnStart;
        public bool ClearFileOnAwake => startBehaviour == EStartBehaviour.ClearFileOnAwake;
        public int SaveFileCount => saveFileNames.Count;
        public bool IsFileLoaded => saveFile != null;

        /// <summary>
        /// The name of the currently loaded save file with its file extension.
        /// </summary>
        /// <seealso cref="SaveFileExtension"/>
        public string SaveFileNameWithExtension => Path.GetFileName(SaveFile.settings.path);

        /// <summary>
        /// The name of the currently loaded save file.
        /// </summary>
        [ShowInInspector]
        public string SaveFileName
        {
            get
            {
                if (!IsFileLoaded)
                    return "-none-";
                return Path.GetFileNameWithoutExtension(SaveFileNameWithExtension);
            }
        }

        /// <summary>
        /// Fulll, absolute path to the currently loaded file.
        /// </summary>
        public string SaveFilePath => SaveFile.settings.FullPath;

        public IReadOnlyList<string> AllSaveFileNames
        {
            get
            {
                return saveFileNames
                    .Select(path => Path.GetFileNameWithoutExtension(path))
                    .ToArray();
            }
        }

        /// <summary>
        /// Save file which is currently loaded. If it's null, it needs to be loaded from disk.
        /// </summary>
        private ES3File saveFile;

        /// <summary>
        /// Gets the current save file that is loaded.
        /// </summary>
        private ES3File SaveFile
        {
            get
            {
                if (saveFile == null)
                    throw new InvalidOperationException("No save file is loaded. Please load or create a file first.");
                return saveFile;
            }
        }

        /// <summary>
        /// The stored data inside the save file.
        /// </summary>
        public ISaveStore Data => this;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I facilitate saving and manage save files.");
        }

        protected override void Awake()
        {
            if (!Singleton.TakeOrDestroy(this, ref s_instance,
                dontDestroyOnLoad: true))
            {
                // quit here if this was the duplicate that should be destroyed
                return;
            }

            int count = ScanForSaveFiles().Count;

            if (ClearFileOnAwake && count > 0)
                DeleteFile();

            if (LoadFileOnAwake)
            {
                // ensure there is at least one save file
                if (count == 0)
                    CreateFile();

                LoadFile();
            }
        }

        private IEnumerator Start()
        {
            if (LoadGameOnStart)
            {
                // ensure all Awake, OnEnable, and Start calls have been made
                yield return CommonYieldInstructions.WaitForEndOfFrame;
                LoadGame();
            }
        }

        private void OnDestroy()
        {
            if (syncOnQuit && App.IsQuitting && IsFileLoaded)
                SaveToFile();

            Singleton.Release(this, ref s_instance);
        }

        protected void OnEnable()
        {
            // subscribe to events
            GlobalSignals.Get<SaveGameSignal>().AddListener(SaveGame);
            GlobalSignals.Get<SaveObjectStateSignal>().AddListener(Save);
        }

        protected void OnDisable()
        {
            // unsubscribe from events
            GlobalSignals.Get<SaveGameSignal>().RemoveListener(SaveGame);
            GlobalSignals.Get<SaveObjectStateSignal>().RemoveListener(Save);
        }

        #endregion Unity Messages

        #region Path Helpers

        public void ValidateFilePath(ref string fileName)
        {
            GuardAgainst.ArgumentIsNull(fileName, nameof(fileName));
            EnsureProperFileExtension(ref fileName);
        }

        public string EnsureProperFileExtension(ref string fileName)
        {
            // ensure it has the proper extension
            if (Path.HasExtension(fileName))
            {
                string fileExtension = Path.GetExtension(fileName);
                // check for bad extension
                if (fileExtension != SaveFileExtension)
                    throw new Exception($"File extension must be '{SaveFileExtension}' but is '{fileExtension}'.");
            }
            else
            {
                // append .es3
                fileName += SaveFileExtension;
            }

            return fileName;
        }

        private string GetFullPath(string fileName)
        {
            EnsureProperFileExtension(ref fileName);
            return SaveFileDirectory + fileName;
        }

        #endregion Path Helpers

        #region File Management

        /// <summary>
        /// Generates a new, unique file name without file extension.
        /// </summary>
        public string GenerateFileName()
        {
            string defaultFileName = DEFAULT_SAVE_FILE_NAME;

            // check for default file name
            if (!FileExists(defaultFileName))
                return Path.GetFileNameWithoutExtension(defaultFileName);

            // it already exists, so append numbers to it
            string fileNameBase = Path.GetFileNameWithoutExtension(defaultFileName);
            int num = 1;
            string fileName;

            do
            {
                string numString = num.ToStringCached();
                num++;
                fileName = $"{fileNameBase} {numString}{SaveFileExtension}";

            } while (FileExists(fileName));

            // remove the .es3
            return Path.GetFileNameWithoutExtension(fileName);
        }

        /// <summary>
        /// Load all the save file paths from the disk and store them.
        /// </summary>
        public void RescanForSaveFiles() => ScanForSaveFiles();

        /// <summary>
        /// Load all the save file paths from the disk and store them.
        /// </summary>
        private List<string> ScanForSaveFiles()
        {
            saveFileNames.Clear();
            saveFileNames.AddRange(EnumerateSaveFileNames());

            if (debug)
                Debug.Log($"Found {saveFileNames.Count} save files.", this);

            return saveFileNames;
        }

        /// <summary>
        /// Iterates through the files on disk.
        /// </summary>
        public IEnumerable<string> EnumerateSaveFilePaths()
        {
            // look for all of the save files in the persistent data location
            return Directory.EnumerateFiles(SaveFileDirectory)
                .Where(filePath => Path.GetExtension(filePath) == SaveFileExtension);
        }

        /// <summary>
        /// Iterates through the files on disk and provides their names with extensions.
        /// </summary>
        public IEnumerable<string> EnumerateSaveFileNames()
        {
            return EnumerateSaveFilePaths()
                .Select(path => Path.GetFileName(path));
        }

        /// <summary>
        /// Iterates through the files on disk and provides their names without extensions.
        /// </summary>
        public IEnumerable<string> EnumerateSaveFileNamesWithoutExtension()
        {
            return EnumerateSaveFilePaths()
                .Select(path => Path.GetFileNameWithoutExtension(path));
        }

        /// <param name="fileName">Extension optional, but correct extension required.</param>
        /// <returns><see langword="true"/> if <paramref name="fileName"/> is a known save file.</returns>
        public bool FileExists(string fileName)
        {
            EnsureProperFileExtension(ref fileName);
            Assert.AreEqual(saveFileNames.Contains(fileName), File.Exists(GetFullPath(fileName)), "Known files in RAM and on disk are different. Most likely this is because the save files were manually modified. If this is the case, please rescan for files.");
            return saveFileNames.Contains(fileName);
        }

        /// <summary>
        /// Loads the default file from disk to memory.
        /// </summary>
        [Button, EnableIf("@SaveFileCount > 0")]
        public void LoadFile()
        {
            // check and double-check
            if (saveFileNames.IsEmpty() && ScanForSaveFiles().IsEmpty())
                throw new InvalidOperationException("There are no save files to load. Create one first.");

            LoadFileInternal(saveFileNames.First());
        }

        /// <summary>
        /// Loads the file at <paramref name="fileName"/> from disk to memory.
        /// </summary>
        /// <param name="fileName">File extension optional.</param>
        public void LoadFile(string fileName)
        {
            if (!FileExists(fileName) // it isn't in RAM, maybe we missed it
                && ScanForSaveFiles().Count > 0 && !FileExists(fileName)) // double-check the file system
                throw new InvalidOperationException($"The file '{fileName}' cannot be loaded because it doesn't exist.");

            // the save file is valid. Load it.
            LoadFileInternal(fileName);
        }

        private void LoadFileInternal(string nextFilePath)
        {
            if (saveFile != null)
                SaveToFile();
            saveFile = CreateSaveFileObject(nextFilePath, sync: true);

            if (debug)
                Debug.Log($"Loaded '{SaveFileNameWithExtension}'.", this);
        }

        /// <summary>
        /// Generates a new save file and returns the name.
        /// </summary>
        [Button(DrawResult = false)]
        public string CreateFile()
        {
            string newName = GenerateFileName();
            CreateFile(newName);
            return newName;
        }

        /// <summary>
        /// Creates a new save file on disk. Does not allow overwriting existing files.
        /// </summary>
        /// <param name="fileName">The name of the file. Extension is optional.</param>
        /// <exception cref="InvalidOperationException"></exception>
        [Button]
        public void CreateFile(string fileName) => CreateFile(fileName, allowOverwriting: false);

        /// <summary>
        /// Creates a new save file on disk.
        /// </summary>
        /// <param name="fileName">The name of the file. Extension is optional.</param>
        /// <param name="allowOverwriting">Allow overwriting any existing files with the same name.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public void CreateFile(string fileName, bool allowOverwriting)
        {
            ValidateFilePath(ref fileName);

            // check for duplicates
            if (FileExists(fileName))
            {
                // check if we are allowing overwriting
                if (!allowOverwriting)
                    throw new InvalidOperationException($"Cannot create file at '{fileName}' because it already exists. Delete it or choose a new name.");
            }
            else
            {
                saveFileNames.Add(fileName);
            }

            // if we got this far, create the actual file
            ES3File file = CreateSaveFileObject(fileName, sync: false);

            // put something on disk
            file.Save(HAS_SAVE_DATA_KEY, true);
            file.Sync(); // writes to disk

            if (debug)
                Debug.Log($"Created '{fileName}'.", this);

            // sanity check
            Assert.AreEqual(file.GetKeys().Length, 1, "Expected the save file to be empty.");
        }

        /// <summary>
        /// Saves RAM to Disk.
        /// </summary>
        [Button("Save to Disk"), EnableIf(nameof(IsFileLoaded))]
        public void SaveToFile()
        {
            SaveFile.Sync();

            if (debug)
                Debug.Log($"Saved '{SaveFileName}'.", this);
        }

        /// <summary>
        /// Saves the current file to a new location on disk.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void SaveToFile(string newFileName, bool allowOverwriting = false)
        {
            ValidateFilePath(ref newFileName);

            if (saveFile == null)
                throw new InvalidOperationException("No save file is loaded. Please load or create one first.");

            // guard overwriting
            bool fileExists = FileExists(newFileName);
            if (fileExists)
            {
                // throw if saving over self
                if (newFileName == SaveFileNameWithExtension)
                    throw new InvalidOperationException("Destination path is the same as source path. This is redundant. Is it deliberate? Maybe you just want SaveToFile().");

                // throw if overwriting is not allowed
                if (!allowOverwriting)
                    throw new InvalidOperationException($"Can't save to the file at '{newFileName}' because it already exists. Delete the file or choose a new name.");
            }
            else
            {
                saveFileNames.Add(newFileName);
            }

            SaveFile.Sync(GetSettings(newFileName));

            if (debug)
                Debug.Log($"Saved '{newFileName}'.", this);
        }

        /// <summary>
        /// Deletes the currently loaded save file from disk.
        /// </summary>
        [Button, EnableIf(nameof(IsFileLoaded))]
        public void DeleteFile()
        {
            if (SaveFileCount == 0 && ScanForSaveFiles().Count == 0) // double-check
                throw new InvalidOperationException("Cannot delete save file because there are none.");
            DeleteFile(SaveFileNameWithExtension);
        }

        public void DeleteFile(string fileName)
        {
            if (SaveFileCount == 0 && ScanForSaveFiles().Count == 0) // double-check
                throw new InvalidOperationException("Cannot delete save file because there are none.");
            ValidateFilePath(ref fileName);

            if (!FileExists(fileName))
                throw new InvalidOperationException($"Cannot delete file '{fileName}' because it doesn't exist!");

            ES3File fileToDelete;
            // are we trying to delete the current file?
            if (saveFile != null && fileName == SaveFileNameWithExtension)
            {
                fileToDelete = saveFile;
                saveFile = null;
            }
            else
            {
                fileToDelete = CreateSaveFileObject(fileName, sync: false); //load into memory
            }

            // operate
            fileToDelete.Clear(); // clear any lingering data
            fileToDelete.Sync(); // deletes file on disk
            saveFileNames.Remove(fileName); // preserve the order

            if (debug)
                Debug.Log($"Deleted '{fileName}'.", this);
        }

        [Button]
        public void DeleteAll()
        {
            if (SaveFileCount == 0)
            {
                if (debug)
                    Debug.Log("There are no save files to delete.", this);
                return;
            }

#if UNITY_EDITOR
            if (Application.isEditor)
            {
                string title = "Are you sure?";
                string prompt = $"All the files with the extension '{SaveFileExtension}' at '{SaveFileDirectory}' will be deleted";
                if (!UnityEditor.EditorUtility.DisplayDialog(title, prompt, "Yes", "No"))
                    return;
            }
#endif
            int count = SaveFileCount;
            // re-use this list to avoid an allocation
            saveFileNames.Clear();
            foreach (string file in EnumerateSaveFilePaths())
                File.Delete(file);
            saveFile = null;

            if (debug)
                Debug.Log($"Deleted {count} save files.", this);
        }

        #endregion File Management

        #region Game State

        /// <summary>
        /// Load the world.
        /// </summary>
        [Button, DisableInEditorMode] // [EnableIf(nameof(IsFileLoaded))] // enables in editor mode :/
        public void LoadGame()
        {
            // broadcast load command
            GlobalSignals.Get<LoadStateFromFileSignal>().Dispatch(this);

            if (debug)
                Debug.Log($"Loaded game state from: '{SaveFileNameWithExtension}'.", this);
        }

        /// <summary>
        /// Save the world.
        /// </summary>
        [Button("Save the World"), DisableInEditorMode] // [EnableIf(nameof(IsFileLoaded))] // enables in editor mode :/
        public void SaveGame() => SaveGame(toDisk: true);

        /// <summary>
        /// Save the world.
        /// </summary>
        public void SaveGame(bool toDisk)
        {
            GlobalSignals.Get<SaveStateToFileSignal>().Dispatch(this); // save the world

            if (toDisk)
                SaveToFile();

            if (debug)
                Debug.Log($"Saved game state to: {SaveFileName}.", this);
        }

        #endregion Game State

        /// <summary>
        /// Checks to see if there is any save data in the active file.
        /// </summary>
        /// <returns>True if the save file has any data in it, and false if it's unused.</returns>
        public bool FileHasSaveData() => SaveFile.Load(HAS_SAVE_DATA_KEY, defaultValue: false);

        public void SortSaveFileNames() => saveFileNames.Sort();

        #region ES3 Factories

        private ES3Settings GetSettings(string path)
        {
            var newSettings = (ES3Settings)settings.Clone();
            newSettings.path = path;
            return newSettings;
        }

        private ES3File CreateSaveFileObject(string filePath, bool sync)
        {
            return CreateSaveFileObject(GetSettings(filePath), sync); // be usre to clone the settings
        }

        private ES3File CreateSaveFileObject(ES3Settings settings, bool sync)
        {
            try
            {
                return new ES3File(settings, sync);
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
                        if (filePath.Contains(SaveFileDirectory) && File.Exists(filePath))
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

        #endregion  ES3 Factories

        #region ISaveable Consumers

        /// <summary>
        /// Save <paramref name="item"/> to the currently active <see cref="SaveFile"/>.
        /// </summary>
        public void Save(ISaveable item) => item.SaveState(this);

        /// <summary>
        /// Load <paramref name="item"/> from the currently active <see cref="SaveFile"/>.
        /// </summary>
        public void Load(ISaveable item) => item.LoadState(this);

        /// <summary>
        /// Delete <paramref name="item"/> from the currently active <see cref="SaveFile"/>.
        /// </summary>
        public void Delete(ISaveable item) => item.DeleteState(this);

        #endregion ISaveable Consumers

        #region ISaveStore

        void ISaveStore.Save<T>(string key, T memento) => SaveFile.Save(key, memento);
        T ISaveStore.Load<T>(string key) => SaveFile.Load<T>(key);
        T ISaveStore.Load<T>(string key, T @default) => SaveFile.Load<T>(key, @default);
        void ISaveStore.LoadInto<T>(string key, T memento) where T : class
            => SaveFile.LoadInto(key, memento);
        bool ISaveStore.KeyExists(string key) => SaveFile.KeyExists(key);
        void ISaveStore.Delete(string key) => SaveFile.DeleteKey(key);
        void ISaveStore.Clear() => SaveFile.Clear();

        #endregion ISaveStore

        #region File IO

        [Button, EnableIf(nameof(IsFileLoaded))]
        public void OpenSaveFile()
        {
            System.Diagnostics.Process.Start(SaveFilePath);
        }

        public static void EnsureFilePathExists(string filePath)
        {
            GuardAgainst.ArgumentIsNull(filePath, nameof(filePath));

            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                CreateDirectoriesRecursively(directoryPath);
            }
        }

        private static void CreateDirectoriesRecursively(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                CreateDirectoriesRecursively(Path.GetDirectoryName(directoryPath));
                Directory.CreateDirectory(directoryPath);
            }
        }

        #endregion File IO

        public enum EStartBehaviour
        {
            /// <summary>
            /// No automatic behaviour.
            /// </summary>
            Nothing = 0,

            /// <summary>
            /// Load the file on start.
            /// </summary>
            LoadFileOnAwake = 1,

            /// <summary>
            /// Load the game state on start.
            /// </summary>
            LoadGameOnStart = 2,

            /// <summary>
            /// Clear save state on start.
            /// </summary>
            ClearFileOnAwake = 3,
        }
    }
}
