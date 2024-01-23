
namespace RichPackage.SaveSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class Es3FileAdapter : ISaveStore
    {
        public ES3File target;

        public void Clear() => target.Clear();
        public bool KeyExists(string key) => target.KeyExists(key);
        public void Delete(string key) => target.DeleteKey(key);
        public T Load<T>(string key) => target.Load<T>(key);
        public T Load<T>(string key, T @default) => target.Load(key, @default);
        public void LoadInto<T>(string key, T memento) where T : class => target.LoadInto(key, memento);
        public void Save<T>(string key, T memento) => target.Save(key, memento);
    }
}
