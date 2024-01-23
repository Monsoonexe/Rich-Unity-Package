/* Dependencies should not access these until OnLateLevelLoadedSignal.
 * 
 */ 

using UnityEngine;

namespace RichPackage.SaveSystem
{
    public abstract class ASaveableScriptableObject<TMemento> : RichScriptableObject, ISaveable
        where TMemento : class, new()
    {
        [SerializeField]
        protected UniqueID id;

        [SerializeField]
        protected TMemento memento = new TMemento();

        public virtual TMemento Value { get => memento; set => memento = value; }

        public UniqueID SaveID => id;

        public void DeleteState(ISaveStore saveFile) => saveFile.Delete(id);

        public void LoadState(ISaveStore saveFile)
        {
            if (saveFile.KeyExists(id))
            {
                saveFile.LoadInto(id, memento);
            }
        }

        public void SaveState(ISaveStore saveFile)
        {
            saveFile.Save(id, memento);
        }

        public static implicit operator TMemento(ASaveableScriptableObject<TMemento> a) => a.Value;
    }
}
