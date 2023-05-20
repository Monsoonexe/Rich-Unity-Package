namespace RichPackage.SaveSystem
{
    public class RememberTransform : ASaveableMonoBehaviour<RememberTransform.Memento>
    {
        protected override void LoadStateInternal()
        {
            SaveData.properties.Store(myTransform); // from memento into object
        }

        protected override void SaveStateInternal()
        {
            SaveData.properties.Load(myTransform); // from object to memento
        }

        [System.Serializable]
        public class Memento : AState
        {
            public TransformProperties properties;
        }
    }
}
