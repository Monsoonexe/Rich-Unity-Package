namespace RichPackage.SaveSystem
{
    public class RememberTransform : ASaveableMonoBehaviour<RememberTransform.Memento>
    {
        public override void SaveState(ES3File saveFile)
        {
            SaveData.IsDirty = true;
            SaveData.properties.Load(myTransform); // move live data to memento
            base.SaveState(saveFile);
        }

        public override void LoadState(ES3File saveFile)
        {
            // use current state if no save data
            if (!saveFile.KeyExists(SaveID))
                return;

            base.LoadState(saveFile);
            SaveData.properties.Store(myTransform); // set live data from memento
        }

        [System.Serializable]
        public class Memento : AState
        {
            public TransformProperties properties;
        }
    }
}
