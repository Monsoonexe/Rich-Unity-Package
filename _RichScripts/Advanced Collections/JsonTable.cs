using Newtonsoft.Json;
using RichPackage.SaveSystem;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RichPackage.Collections
{
    /// <summary>
    /// A table of values that are serialized to JSON.
    /// </summary>
    /// <seealso cref="OdinByteTable"/>
    [System.Serializable]
    public class JsonTable : Dictionary<string, string>, ISaveStore
    {
        // use this method so we can take advantage of any project-wide customization
        private readonly JsonSerializer converter = JsonSerializer.CreateDefault(); // new JsonSerializer();
        
        public Formatting Formatting
        {
            get => converter.Formatting;
            set => converter.Formatting = value;
        }

        public void Set<T>(string key, T value) => this[key] = Serialize(value);

        public T Get<T>(string key) => Deserialize<T>(this[key]);

        public T GetOrDefault<T>(string key, T @default = default)
        {
            return TryGetValue(key, out string value)
                ? Deserialize<T>(value)
                : @default;
        }

        private string Serialize<T>(T value)
        {
            using (StringBuilderCache.Rent(out var sb))
            using (var sWriter = new StringWriter(sb))
            using (var jWriter = new JsonTextWriter(sWriter))
            {
                converter.Serialize(jWriter, value, typeof(T));
                return sWriter.ToString();
            }
        }

        private T Deserialize<T>(string json)
        {
            using (var sReader = new StringReader(json))
            using (var jReader = new JsonTextReader(sReader))
            {
                return (T)converter.Deserialize(jReader, typeof(T));
            }
        }

        #region ISaveStore

        void ISaveStore.Save<T>(string key, T memento) => Set(key, memento);
        T ISaveStore.Load<T>(string key) => Get<T>(key);
        T ISaveStore.Load<T>(string key, T @default) => GetOrDefault(key, @default);
        void ISaveStore.LoadInto<T>(string key, T memento) where T : class
        {
            using (var sReader = new StringReader(this[key]))
            using (var jReader = new JsonTextReader(sReader))
            {
                converter.Populate(jReader, memento);
            }
        }
        bool ISaveStore.KeyExists(string key) => ContainsKey(key);
        void ISaveStore.Delete(string key) => Remove(key);

        #endregion ISaveStore
    }
}
