using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RichPackage.Databases
{
    /// <summary>
    /// A simple database asset that stores serialized objects with a key.
    /// </summary>
    public class SimpleDatabaseAsset : RichScriptableObject
    {
        /// <summary>
        /// The backing store for the data.
        /// </summary>
        [SerializeField]
        private List<DatabaseEntry> entries;

        /// <summary>
        /// Lookup table for faster operations.
        /// </summary>
        private Dictionary<string, DatabaseEntry> entryMap;

        #region Unity Messages

        protected virtual void Reset()
        {
            SetDevDescription($"A database object for storing serialized data.");
        }

        private void OnEnable()
        {
            RefreshEntryMap();
        }

        private void OnValidate()
        {
            // refresh if entries were manually edited
            if (entries.Count != entryMap?.Count)
                RefreshEntryMap();
        }

        #endregion Unity Messages

        [Button]
        private void RefreshEntryMap()
        {
            // create or reset
            if (entryMap == null)
                entryMap = new Dictionary<string, DatabaseEntry>(entries.Count);
            else
                entryMap.Clear();

            // load list into table
            foreach (DatabaseEntry entry in entries)
            {
                entryMap[entry.Key] = entry;
            }
        }

        private DatabaseEntry GetOrCreateEntry(string key)
        {
            if (!entryMap.TryGetValue(key, out DatabaseEntry entry))
            {
                entry = new DatabaseEntry(key);
                entries.Add(entry);
                entryMap.Add(key, entry);
            }
            return entry;
        }

        #region CRUD

        public void Delete(string key)
        {
            if (entryMap.TryGetValue(key, out DatabaseEntry entry))
            {
                entryMap.Remove(key);
                entries.QuickRemove(entry);
            }
        }

        public T Get<T>(string key, T @default = default)
        {
            return entryMap.TryGetValue(key, out DatabaseEntry entry) ? entry.GetValue<T>() : default;
        }

        public void Set<T>(string key, T value)
        {
            GetOrCreateEntry(key).SetValue(value);
        }

        public void Clear()
        {
            entryMap.Clear();
            entries.Clear();
        }

        #endregion CRUD

        public bool Contains(string key) => entryMap.ContainsKey(key);

        #region File IO

        public void SaveToFile(string filePath)
        {
            string json = JsonConvert.SerializeObject(entries);
            File.WriteAllText(filePath, json);
        }

        public void LoadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"File '{filePath}' doesn't exist.", this);
                Clear();
                return;
            }

            var serializer = new JsonSerializer();
            using (var sr = new StreamReader(File.OpenRead(filePath)))
            using (var jsonTextReader = new Newtonsoft.Json.JsonTextReader(sr))
            {
                entries = (List<DatabaseEntry>)serializer.Deserialize(jsonTextReader, typeof(List<DatabaseEntry>));
            }
        }

        #endregion File IO

        #region Json

        public string ToJson()
        {
            using (var stream = new StringWriter())
            using (var  writer = new JsonTextWriter(stream))
            {
                writer.WriteStartObject();

                foreach (var prop in entries)
                {
                    writer.WritePropertyName(prop.Key);
                    writer.WriteValue(prop.RawValue);
                }

                writer.WriteEndObject();
                return writer.ToString();
            }
        }

        public void FromJson(string json)
        {
            if (entries == null)
                entries = new List<DatabaseEntry>();

            using (var stream = new StringReader(json))
            using (var reader = new JsonTextReader(stream))
            {
                while (reader.Read())
                {
                    string key = (string)reader.Value;

                    if (!reader.Read())
                        throw new Exception("Invalid json!");

                    string rawValue = (string)reader.Value;

                    entries.Add(new DatabaseEntry(key, rawValue));
                }
            }

            RefreshEntryMap();
        }

        #endregion Json

        [Serializable]
        private class DatabaseEntry
        {
            [SerializeField]
            private string key;

            [SerializeField]
            private string value;

            public string Key { get => key; private set => key = value; }
            public string RawValue { get => value; }

            #region Constructors

            public DatabaseEntry(string key)
            {
                Key = key;
            }

            public DatabaseEntry(string key, string rawValue)
            {
                this.Key = key;
                this.value = rawValue;
            }

            #endregion Constructors

            public void SetValue<T>(T value)
            {
                this.value = JsonConvert.SerializeObject(value);
            }

            public T GetValue<T>()
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }
    }
}
