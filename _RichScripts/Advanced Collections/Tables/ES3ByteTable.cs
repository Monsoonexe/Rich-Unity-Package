using RichPackage.SaveSystem;
using System.Collections.Generic;

namespace RichPackage.Collections
{
    /// <summary>
    /// A table of values that are serialized using ES3.
    /// </summary>
    /// <see cref="JsonTable"/>
    public class ES3ByteTable : Dictionary<string, byte[]>, ISaveStore
    {
        public static ES3Settings settings = ES3Settings.defaultSettings;

        public void Set<T>(string key, T value) => this[key] = Serialize(value);
        public T Get<T>(string key) => Deserialize<T>(this[key]);
        public T GetOrDefault<T>(string key, T @default = default)
        {
            return TryGetValue(key, out byte[] value)
                ? Deserialize<T>(value)
                : @default;
        }

        private byte[] Serialize<T>(T value) => ES3.Serialize(value, settings);

        private T Deserialize<T>(byte[] bytes) => ES3.Deserialize<T>(bytes, settings);

        #region ISaveStore

        void ISaveStore.Save<T>(string key, T memento) => Set(key, memento);
        T ISaveStore.Load<T>(string key) => Get<T>(key);
#if UNITY_2020_OR_NEWER
        T ISaveStore.Load<T>(string key, T @default) => GetOrDefault(key, @default);
#endif
        void ISaveStore.LoadInto<T>(string key, T memento) => ES3.DeserializeInto<T>(this[key], memento, settings);
        bool ISaveStore.KeyExists(string key) => ContainsKey(key);
        void ISaveStore.Delete(string key) => Remove(key);

#endregion ISaveStore
    }
}
