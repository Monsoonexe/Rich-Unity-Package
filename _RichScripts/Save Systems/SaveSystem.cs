using UnityEngine;
using Sirenix.OdinInspector;
using RichPackage.Databases;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// I facilitate saving and manage save files backed by Newtonsoft.Json.
    /// </summary>
    public abstract class SaveSystem : RichMonoBehaviour, ISaveSystem
    {
        [SerializeField, Required]
        private SimpleDatabaseAsset database;

        public void Clear()
        {
            database.Clear();
        }

        public bool Contains(string key)
        {
            return database.Contains(key);
        }

        public void Delete(string key)
        {
            database.Delete(key);
        }

        public T Load<T>(string key)
        {
            return database.Get<T>(key);
        }

        public T Load<T>(string key, T @default)
        {
            if (!database.Contains(key))
                return @default;
            return database.Get<T>(key);
        }

        public void Save<T>(string key, T memento)
        {
            database.Set(key, memento);
        }
    }
}
