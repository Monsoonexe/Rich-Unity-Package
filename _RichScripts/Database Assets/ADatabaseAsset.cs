using RichPackage.RandomExtensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RichPackage.Databases
{
    public abstract class ADatabaseAsset : RichScriptableObject
    {
        // common base class to differentiate these classes
    }

    /// <summary>
    /// An asset that holds references to <see cref="Object"/>s and can look them up by keys.
    /// Useful for propagating known entities through the network.
    /// </summary>
    /// <typeparam name="TData">The type of data that actually gets held in the database.</typeparam>
    public abstract class ADatabaseAsset<TData> : ADatabaseAsset, IEnumerable<TData>
        where TData : UnityEngine.Object, IDatabaseMember
    {
        [Title("Settings")]
        public bool autoAssignIds = false;

        [Title("Database")]
        [Tooltip("Index all the replay objects here. Be sure their replay entity script has this index set in the inspector!")]
        [SerializeField, AssetsOnly, Required, LabelWidth(50),
            ListDrawerSettings(NumberOfItemsPerPage = 40, ShowIndexLabels = true)]
#if UNITY_EDITOR
        [InfoBox("$" + nameof(editorErrorMessage), InfoMessageType.Error, VisibleIf = nameof(EditorHasError))]
#endif
        protected TData[] items = Array.Empty<TData>(); // helps prevent null-refs

        protected readonly Dictionary<int, TData> lookupTable = new Dictionary<int, TData>();

        /// <summary>
        /// All of the items in the database.
        /// </summary>
        public IReadOnlyCollection<TData> All => items;

        #region Unity Messages

        protected virtual void Reset()
        {
            SetDevDescription($"A database for {typeof(TData).Name}.");
        }

        protected void OnEnable()
        {
            BuildTable();
        }

        #endregion Unity Messages

        protected void BuildTable()
        {
            lookupTable.Clear();
            foreach (TData item in items)
            {
#if UNITY_EDITOR
                try
                {
                    lookupTable.Add(item.Key, item);
                }
                catch
                {
                    Debug.LogError($"Duplicate key: {item.Key}.", item);
                }
#else
				lookupTable.Add(item.Key, item);
#endif
            }
        }

        /// <summary>
        /// Retrieve an item from the database.
        /// </summary>
        [Button]
        public TData Get(int key)
        {
            if (lookupTable.TryGetValue(key, out TData value))
                return value;

            throw new KeyNotFoundException($"Key {key} not found in {this}.");
        }

        public TData Get(Predicate<TData> query)
        {
            foreach (TData i in items)
            {
                if (query(i))
                    return i;
            }

            return null;
        }

        public TData GetOrDefault(int key, TData @default = default)
        {
            return lookupTable.TryGetValue(key, out TData value) ? value : @default;
        }

        public bool TryGet(int key, out TData value)
        {
            return lookupTable.TryGetValue(key, out value);
        }

        /// <summary>
        /// Retrieve a random element from the database.
        /// </summary>
        public TData GetRandom() => items.GetRandomElement();

        public bool Contains(TData data) => lookupTable.ContainsValue(data);

        public bool Contains(int key) => lookupTable.ContainsKey(key);

        protected virtual int GenerateNewKey() => UnityEngine.Random.Range(int.MinValue + 1, int.MaxValue);

        #region IEnumerable

        IEnumerator<TData> IEnumerable<TData>.GetEnumerator() => All.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => All.GetEnumerator();

        #endregion IEnumerable

        #region Editor
#if UNITY_EDITOR

        protected string editorErrorMessage;

        protected bool EditorHasError { get => !editorErrorMessage.IsNullOrEmpty(); }

        /// <summary>
        /// Validate database and assign keys.
        /// </summary>
        [Button, HideInPlayMode]
        protected virtual void OnValidate()
        {
            editorErrorMessage = null;

            if (items.Length != lookupTable.Count)
                BuildTable();

            if (!VerifyNoNullEntries())
                return;

            if (!VerifyCollectionHasNoDuplicateEntries())
                return;

            if (autoAssignIds)
                EnsureEntriesHaveUniqueKeys();
        }

        protected void EnsureEntriesHaveUniqueKeys()
        {
            var table = new Dictionary<int, TData>();

            foreach (TData item in items)
            {
                // check for uninitialized keys
                if (item.Key == 0)
                {
                    AssignUniqueKey(item);
                }
                // check for key collision
                else if (table.TryGetValue(item.Key, out TData existingItem))
                {
                    AssignUniqueKey(item);
                    Debug.LogWarning($"{item} has a key collision with {existingItem}" +
                        $" ({existingItem.Key}) in {name}." +
                        $" Assigned new key to {item} ({item.Key}).", item);
                }

                table.Add(item.Key, item);
            }
        }

        protected bool VerifyCollectionHasNoDuplicateEntries()
        {
            bool hasDuplicate = false;
            var hashSet = new HashSet<TData>();

            foreach (TData item in items)
            {
                if (!hashSet.Add(item))
                {
                    hasDuplicate = true;
                    editorErrorMessage =
                        $"There are duplicate <{item}> entities in the database. " +
                        "Please remove duplicates and leave only 1.";
                    Debug.LogError(editorErrorMessage, this);
                }
            }

            return !hasDuplicate;
        }

        protected bool VerifyNoNullEntries()
        {
            bool valid = true;
            foreach ((TData Item, int Index) in items.ForEachWithIndex())
            {
                if (Item == null)
                {
                    valid = false;
                    Debug.LogError(editorErrorMessage = $"{this} has an item at index {Index} that is null. Please fix this.", this);
                }
            }

            return valid;
        }

        protected bool IsKeyUnique(TData query)
        {
            foreach (TData item in items)
            {
                if (item.Key == query.Key && item != query)
                    return false;
            }

            return true;
        }

        // TODO - move this to a class anywhere can access, not just the editor assembly
        /// <param name="assets">Collection to add the assets to.</param>
        public static void FindAllAssets<TAsset>(List<TAsset> assets)
            where TAsset : UnityEngine.Object
        {
            // get all the guids
            foreach (string guid in UnityEditor.AssetDatabase.FindAssets("t:" + typeof(TAsset).Name))
            {
                // find the path
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);

                // find the asset 
                TAsset asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TAsset>(path);

                // add to list
                assets.Add(asset);
            }
        }

        protected void AssignUniqueKey(TData data)
        {
            int newKey;
            do
                newKey = GenerateNewKey(); // assign a newly generated key
            while (Contains(newKey)); // check for key collision
            data.Key = newKey;
            UnityEditor.EditorUtility.SetDirty(data);
        }

#endif
        #endregion Editor
    }
}
