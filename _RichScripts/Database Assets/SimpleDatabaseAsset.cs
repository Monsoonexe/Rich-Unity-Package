using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
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
            if (entries.Count != entryMap.Count)
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
            foreach (var entry in entries)
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
            if (entryMap.TryGetValue(key, out DatabaseEntry entry))
                return entry.GetValue<T>();
            return default;
		}

        public void Set<T>(string key, T value)
		{
            GetOrCreateEntry(key).SetValue(value);
		}

		#endregion CRUD

		#region File IO

        // TODO - save / load from file

		#endregion File IO

		[Serializable]
        private class DatabaseEntry
		{
            [SerializeField]
            private string key;

            [SerializeField]
            private string value;

            public string Key { get => key; private set => key = value; }

			#region Constructors

			public DatabaseEntry(string key)
            {
                Key = key;
            }

			#endregion Constructors

			public void SetValue<T>(T value)
			{
                this.value = JsonUtility.ToJson(value);
			}

            public T GetValue<T>()
			{
                return JsonUtility.FromJson<T>(value);
			}
        }
    }
}
