using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace RichPackage
{
	public static class Dictionary_Extensions
    {
        /// <returns><see langword="true"/> if the <paramref name="value"/> was added to 
        /// <paramref name="dic"/>, otherwise <see langword="false"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetRemove<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            TKey key, out TValue value)
        {
            bool found; // return value
            if (found = dic.TryGetValue(key, out value))
                dic.Remove(key);
            return found;
        }

        /// <summary>
        /// Removes and returns either the <paramref name="defaultValue"/> or the value 
        /// keyed by <paramref name="key"/> if it exists.
        /// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetRemoveOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            TKey key, TValue defaultValue = default)
		{
            return dic.TryGetValue(key, out TValue value) ? value : defaultValue;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => dictionary.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
            => dictionary.Count != 0;
    }
}
