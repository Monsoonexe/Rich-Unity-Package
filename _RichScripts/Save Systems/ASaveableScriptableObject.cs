using UnityEngine;

namespace RichPackage.SaveSystem
{
    public abstract class ASaveableScriptableObject<TMemento> : ASaveableScriptableObject, ISaveable
        where TMemento : class, new()
    {
        [SerializeField]
        private TMemento memento = new TMemento();

        public virtual TMemento Value { get => memento; set => memento = value; }

        public override void LoadState(ISaveStore saveFile)
        {
            if (saveFile.KeyExists(SaveID))
            {
                saveFile.LoadInto(SaveID, memento);
            }
        }

        public override void SaveState(ISaveStore saveFile)
        {
            saveFile.Save(SaveID, memento);
        }

        public static implicit operator TMemento(ASaveableScriptableObject<TMemento> a) => a.Value;
    }

    public abstract class ASaveableScriptableObject : RichScriptableObject, ISaveable
    {
        [SerializeField]
        private UniqueID id;

        public UniqueID SaveID { get => id; protected set => id = value; }

        public void DeleteState(ISaveStore saveFile) => saveFile.Delete(id);

        public virtual void LoadState(ISaveStore saveFile)
        {
            if (saveFile.KeyExists(SaveID))
            {
                saveFile.LoadInto(SaveID, this);
            }
        }

        public virtual void SaveState(ISaveStore saveFile)
        {
            saveFile.Save(SaveID, this);
        }
    }
}
