namespace RichPackage.SaveSystem
{
    public class RememberTransform : ASaveableMonoBehaviour<RememberTransform.Memento>
    {
        public override void SaveState(ES3File saveFile)
        {
            SaveData.IsDirty = true;
            SaveData.properties.Load(myTransform);
            base.SaveState(saveFile);
        }

        public override void LoadState(ES3File saveFile)
        {
            if (!saveFile.KeyExists(SaveID))
                return;

            base.LoadState(saveFile);
        }

        [System.Serializable]
        public class Memento : AState
        {
            public TransformProperties properties;
        }
    }
}
