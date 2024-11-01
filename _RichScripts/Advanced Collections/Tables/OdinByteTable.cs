using RichPackage.SaveSystem;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace RichPackage.Collections
{
    /// <summary>
    /// A table of values that are serialized to bytes.
    /// </summary>
    /// <seealso cref="JsonTable"/>
    [System.Serializable]
    public class OdinByteTable : Dictionary<string, byte[]>, ISaveStore
    {
        public void Set<T>(string key, T value) => this[key] = Serialize(value);
        public T Get<T>(string key) => Deserialize<T>(this[key]);
        public T GetOrDefault<T>(string key, T @default = default)
        {
            return TryGetValue(key, out byte[] value)
                ? Deserialize<T>(value)
                : @default;
        }

        private byte[] Serialize<T>(T value)
            => SerializationUtility.SerializeValue(value, DataFormat.Binary);

        private T Deserialize<T>(byte[] bytes)
            => SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);

        #region ISaveStore

        void ISaveStore.Save<T>(string key, T memento) => Set(key, memento);
        T ISaveStore.Load<T>(string key) => Get<T>(key);
        T ISaveStore.Load<T>(string key, T @default) => GetOrDefault(key, @default);
        void ISaveStore.LoadInto<T>(string key, T memento) where T : class
        {
            // maybe use reflection to get all the members of each?
            throw new System.NotImplementedException();
        }
        bool ISaveStore.KeyExists(string key) => ContainsKey(key);
        void ISaveStore.Delete(string key) => Remove(key);

        #endregion ISaveStore
    }
}
