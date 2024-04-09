/* If I had this to do over again... 
 * Do NOT make the Unique ID part of the save data itself. this makes inheritance really difficult. Appending save data is a pain. Serializing the nested data is a bit of a pain, too
 * Don't have the data backed by the memento at runtime. Use Memento.Instance during the save/load process. Data therefore isn't restricted to the memento
 * I would also leverage the ES3Type system more. It's really easy to use and 
 * probably more intuitive than the memento system (especially good with scriptable objects and 'ReadInto') 
 */ 

using System;
using System.Diagnostics;
using UnityEngine;
using Sirenix.OdinInspector;
using RichPackage.Events.Signals;
using RichPackage.SaveSystem.Signals;
using Debug = UnityEngine.Debug;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Base class with everything you need to save some persistent data.
    /// </summary>
    /// <seealso cref="SaveSystem"/>
    public abstract class ASaveableMonoBehaviour<TState> : ASaveableMonoBehaviour
        where TState : ASaveableMonoBehaviour.AState, new()
    {	
        [SerializeField]
        protected TState saveData = new TState();

        public TState SaveData
        {
            get => saveData;
            protected set => saveData = value;
        }

        #region ISaveable

        /// <summary>
        /// A persistent, unique string identifier.
        /// </summary>
        [ShowInInspector]
        [PropertyOrder(-1)]
        [DelayedProperty]
        [Title("Saving")]
        [CustomContextMenu("Set to Name", nameof(SetDefaultSaveID))]
        [CustomContextMenu("Set to Scene-Name", nameof(SetSaveIDToSceneGameObjectName))]
        [CustomContextMenu("Complain if not unique", nameof(Editor_PrintIDIsNotUnique))]
        [ValidateInput("@GameObject_Extensions.IsPrefab(gameObject) || IsSaveIDUnique(this)", "ID collision. Regenerate.", InfoMessageType.Warning)]
        public override UniqueID SaveID 
        { 
            get => saveData.saveID; 
            protected set => saveData.saveID = value;
        }

        /// <summary>
        /// Saves <see cref="SaveData"/> to saveFile.
        /// </summary>
        public override void SaveState(ISaveStore saveFile)
        {
            SaveStateInternal();
            saveFile.Save(SaveID, saveData);
        }

        /// <summary>
        /// Loads <see cref="SaveData"/> state from saveFile.
        /// </summary>
        public override void LoadState(ISaveStore saveFile)
        {
            if (saveFile.KeyExists(SaveID))
            {
                saveFile.LoadInto(SaveID, SaveData);
                LoadStateInternal();
            }
        }

        /// <summary>
        /// The deriving class should load the state from <see cref="SaveData"/>.
        /// </summary>
        /// <remarks>Only called if there was state to load.</remarks>
        protected abstract void LoadStateInternal();

        /// <summary>
        /// The deriving class should save the current state into <see cref="SaveData"/>.
        /// </summary>
        protected abstract void SaveStateInternal();

        #endregion ISaveable
    }

    /// <summary>
    /// Base class for all things with default saving behaviour that responds to events.
    /// </summary>
    public abstract class ASaveableMonoBehaviour : RichMonoBehaviour,
        ISaveable, IEquatable<ISaveable>
    {
        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetSaveIDToSceneGameObjectName();
        }

        protected virtual void OnEnable()
        {
            // subscribe to save events
            GlobalSignals.Get<SaveStateToFileSignal>().AddListener(SaveState);
            GlobalSignals.Get<LoadStateFromFileSignal>().AddListener(LoadState);
        }

        protected virtual void OnDisable()
        {
            // unsubscribe from save events
            GlobalSignals.Get<SaveStateToFileSignal>().RemoveListener(SaveState);
            GlobalSignals.Get<LoadStateFromFileSignal>().RemoveListener(LoadState);
        }

        #endregion Unity Messages

        #region ISaveable Implementation

        public abstract UniqueID SaveID { get; protected set; }

        /// <summary>
        /// Saves <see cref="AState"/> to persistent storage.
        /// </summary>
        public abstract void SaveState(ISaveStore saveFile);

        /// <summary>
        /// Loads <see cref="AState"/> state from persistent storage.
        /// </summary>
        public abstract void LoadState(ISaveStore saveFile);

        #endregion ISaveable

        #region Save / Load Helpers

        /// <summary>
        /// Saves this object's state to persistent storage.
        /// </summary>
        [ContextMenu(nameof(SaveState))]
        public void SaveState() => GlobalSignals.Get<SaveObjectStateSignal>().Dispatch(this);

        /// <summary>
        /// Saves <paramref name="value"/> to this object's memento with the <paramref name="key"/>.
        /// </summary>
        protected void SaveValue<T>(string key, T value)
        {
            string fullKey = SaveID + key;
            SaveSystem.Instance.Data.Save(fullKey, value);
        }

        /// <summary>
        /// Load data from this object's memento with the <paramref name="key"/>.
        /// </summary>
        protected T LoadValue<T>(string key)
        {
            string fullKey = SaveID + key;
            return SaveSystem.Instance.Data.Load<T>(fullKey);
        }

        /// <summary>
        /// Load data from this object's memento with the <paramref name="key"/> or returns 
        /// <paramref name="defaultValue"/> if the key doesn't exist.
        /// </summary>
        protected T LoadValue<T>(string key, T defaultValue)
        {
            string fullKey = SaveID + key;
            return SaveSystem.Instance.Data.Load(fullKey, defaultValue);
        }

        protected void DeleteValue(string key)
        {
            string fullKey = SaveID + key;
            SaveSystem.Instance.Data.Delete(fullKey);
        }

        /// <summary>
        /// Erases save data from file.
        /// </summary>
        public void DeleteState(ISaveStore saveFile) => saveFile.Delete(SaveID);

        public bool HasSaveData(ISaveStore saveFile) => saveFile.KeyExists(SaveID);

        /// <summary>
        /// Saveables are equal if their data will be saved to the same entry (has same key).
        /// </summary>
        /// <returns>True if saves to either objects will write to the same entry (key collision).</returns>
        public bool Equals(ISaveable other) => this.SaveID == other.SaveID;

        #endregion Save / Load Helpers

        [Conditional(ConstStrings.UNITY_EDITOR)]
        protected void EnsureValid(ref UniqueID id, UniqueID fallback)
        {
            if (!UniqueID.EnsureValid(ref id, fallback))
            {
                Debug.LogWarning($"Id not set. Using fallback '{fallback}' Please set this id!.", this);
            }
        }

        public static bool IsSaveIDUnique(ASaveableMonoBehaviour query)
            => IsSaveIDUnique(query, out _);

        public static bool IsSaveIDUnique(ASaveableMonoBehaviour query, 
            out ASaveableMonoBehaviour other)
        {
            //TODO - cache this cuz it's terribly slow
            var allSaveables = FindObjectsOfType<ASaveableMonoBehaviour>();
            bool isUnique = true; //return value
            other = default;
            foreach (var saveable in allSaveables)
            {
                if (saveable.Equals(query) //matching keys
                    && saveable != query) //check is not self
                {
                    isUnique = false;
                    other = saveable;
                    break;
                }
            }
            return isUnique;
        }

        [HorizontalGroup("SetGUID"), Button("Complain if ID taken")]
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public void Editor_PrintIDIsNotUnique()
        {
            // ignore uniqueness check for prefabs
            if (gameObject.IsPrefab())
                return;

            if (!IsSaveIDUnique(this, out var other))
            {
                Debug.LogWarning($"{nameof(SaveID)} name collision! The uniqueID <{SaveID}> " +
                    $"is already taken by \"{other.name}\". " +
                    $"{Environment.NewLine} This means you might encounter " +
                    $"problems when attempting to save data with this key.", other);
            }
        }

        #region Editor: Set Save ID Helper Functions

        public virtual void SetDefaultSaveID()
        {
            SaveID = UniqueID.FromString(gameObject.name);
            Editor_MarkDirty();
            Editor_PrintIDIsNotUnique();
        }

        public void SetRandomSaveID()
        {
            SaveID = UniqueID.New;
            Editor_MarkDirty();
            Editor_PrintIDIsNotUnique();
        }

        public void SetSaveIDToSceneGameObjectName()
        {
            SaveID = UniqueID.FromString(gameObject.GetNameWithScene());
            Editor_MarkDirty();
            Editor_PrintIDIsNotUnique();
        }

        public void SetSaveIdToFullScenePath()
        {
            SaveID = UniqueID.FromString(gameObject.GetFullyQualifiedName());
            Editor_MarkDirty();
            Editor_PrintIDIsNotUnique();
        }

        #endregion Editor: Set Save ID Helper Functions

        /// <summary>
        /// Contains all the data that needs to be saved. The object's memento.
        /// </summary>
        /// <remarks>This MUST be tagged with the <see cref="SerializableAttribute"/>.</remarks>
        [Serializable]
        public abstract class AState : IEquatable<AState>
        {
            public AState()
            {
                saveID = UniqueID.New;
            }

            [HideInInspector]
            [ES3NonSerializable] //don't save it to file
            [Tooltip("Must be unique to all other saveables!")]
            public UniqueID saveID; // TODO - move this

            //more fields....

            public bool Equals(AState other) => this.saveID == other.saveID;

            public override string ToString() => saveID + $" ({GetType()})";
        }
    }
}
