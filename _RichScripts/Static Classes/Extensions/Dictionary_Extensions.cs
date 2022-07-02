
using System.Collections.Generic;

namespace RichPackage
{
	public static class Dictionary_Extensions
    {
        /// <returns><see langword="true"/> if the <paramref name="value"/> was added to 
        /// <paramref name="dic"/>, otherwise <see langword="false"/>.</returns>
        public static bool AddIfNew<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            TKey key, TValue value)
		{
            bool wasAdded;
            if (wasAdded = !dic.ContainsKey(key))
                dic.Add(key, value);
            return wasAdded;
		}

        /// <summary>
        /// Returns <see langword="true"/> if the dictionary contains the given key,
        /// which indicates the value was removed.
        /// Otherwise, returns <see langword="false"/>. Will not throw an exception if key does not exist.
        /// </summary>
        public static bool GetRemove<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            TKey key, out TValue value)
        {
            var found = dic.TryGetValue(key, out value);
            if (found) dic.Remove(key);
            return found;
        }

        /// <summary>
        /// Removes and returns either the <paramref name="defaultValue"/> or the value 
        /// keyed by <paramref name="key"/> if it exists.
        /// </summary>
        public static TValue GetRemoveOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            TKey key, TValue defaultValue = default)
		{
            if (dic.TryGetValue(key, out TValue value))
                return value;
            else
                return defaultValue;
		}

        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => dictionary.Count == 0;

        public static bool IsNotEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => dictionary.Count != 0;
    }
}
