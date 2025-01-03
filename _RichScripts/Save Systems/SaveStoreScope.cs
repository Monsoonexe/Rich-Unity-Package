using RichPackage.GuardClauses;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Wraps a save store and applies scoping to all keys save/loaded to it.
    /// </summary>
    public class SaveStoreScope : ISaveStore
    {
        public readonly string Scope;
        private readonly ISaveStore saveStore;

        public SaveStoreScope(ISaveStore saveStore, string scope)
        {
            GuardAgainst.ArgumentIsNull(saveStore, nameof(saveStore));

            this.saveStore = saveStore;
            this.Scope = scope;
        }

        public string ApplyScope(string key)
        {
            string scope = Scope;

            if (scope != null)
                key = scope + key;

            return key;
        }

        public void Clear()
        {
            saveStore.Clear();
        }

        public void Delete(string key)
        {
            key = ApplyScope(key);
            saveStore.Delete(key);
        }

        public bool KeyExists(string key)
        {
            key = ApplyScope(key);
            return saveStore.KeyExists(key);
        }

        public T Load<T>(string key)
        {
            key = ApplyScope(key);
            return saveStore.Load<T>(key);
        }

        public void LoadInto<T>(string key, T memento)
            where T : class
        {
            key = ApplyScope(key);
            saveStore.LoadInto(key, memento);
        }

        public void Save<T>(string key, T memento)
        {
            key = ApplyScope(key);
            saveStore.Save(key, memento);
        }
    }
}
