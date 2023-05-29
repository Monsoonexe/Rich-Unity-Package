/* Dependencies should not access these until OnLateLevelLoadedSignal.
 * 
 */ 

using RichPackage.Events.Signals;
using RichPackage.SaveSystem.Signals;
using Sirenix.OdinInspector;
using System.Diagnostics;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    public abstract class ASaveableScriptableObject<T> : SerializedScriptableObject, ISaveable
        where T : class, new()
    {
        #region Dev Description

#if UNITY_EDITOR
        [SerializeField, TextArea]
        [PropertyOrder(-5)]
#pragma warning disable IDE0052 // Remove unread private members
        private string developerDescription = "Describe this data.";
#pragma warning restore IDE0052 // Remove unread private members
#endif

        /// <summary>
        /// This call will be stripped out of Builds. Use anywhere.
        /// </summary>
        /// <param name="newDes"></param>
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public void SetDevDescription(string newDes)
        {
#if UNITY_EDITOR
            developerDescription = newDes;
#endif
        }

        #endregion Dev Description

        [SerializeField]
        private UniqueID saveId;

        [Title("Save Data")]
        [SerializeField]
        private T memento = new T();

        #region Unity Messages

        protected virtual void Reset()
        {
            saveId = new UniqueID(name);
            memento = new T();
        }

        protected virtual void OnEnable()
        {
            HookToSaveSystem();
        }

        protected virtual void OnApplicationQuit()
        {
            SaveState();
            UnhookFromSaveSystem();
        }

        #endregion Unity Messages

        public void HookToSaveSystem()
        {
            Application.quitting += OnApplicationQuit;
            GlobalSignals.Get<SaveStateToFileSignal>().AddListener(((ISaveable)this).SaveState);
            GlobalSignals.Get<LoadStateFromFileSignal>().AddListener(((ISaveable)this).LoadState);
        }

        public void UnhookFromSaveSystem()
        {
            Application.quitting -= OnApplicationQuit;
            GlobalSignals.Get<SaveStateToFileSignal>().RemoveListener(((ISaveable)this).SaveState);
            GlobalSignals.Get<LoadStateFromFileSignal>().RemoveListener(((ISaveable)this).LoadState);
        }

        /// <summary>
        /// Saves this object's state to persistent storage.
        /// </summary>
        [Button, DisableInEditorMode, HorizontalGroup("B")]
        public void SaveState() => SaveSystem.Instance.Save(this);

        [Button, DisableInEditorMode, HorizontalGroup("B")]
        public void LoadState() => SaveSystem.Instance.Load(this);

        #region ISaveable

        UniqueID ISaveable.SaveID => saveId;

        void ISaveable.DeleteState(ES3File saveFile) => saveFile.DeleteKey(saveId);

        void ISaveable.LoadState(ES3File saveFile)
        {
            if (saveFile.KeyExists(saveId))
                saveFile.LoadInto(saveId, memento);
        }

        void ISaveable.SaveState(ES3File saveFile) => saveFile.Save(saveId, memento);

        #endregion ISaveable
    }
}
