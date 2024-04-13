using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace RichPackage.RandomExtensions
{
    /// <summary>
    /// Random extension methods for ilists.
    /// </summary>
    public static class IListExtensions
    {
        /// <summary>
        /// Returns a random element from array, or default if collection is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetRandomElement<T>(this IList<T> collection)
            => collection[Random.Range(0, collection.Count)];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetRandomIndex(this IList list)
            => Random.Range(0, list.Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetRandomIndex<T>(this IList<T> list)
            => Random.Range(0, list.Count);

        /// <summary>
        /// Get a random element from Collection that is not in usedCollection. 
        /// Up to caller to store this value in usedCollection
        /// </summary>
        public static T GetRandomUnusedElement<T>(this IList<T> totalCollection,
            IList<T> usedCollection)
        {
            // build a pool of indices that have not been used.  
            using (ListPool<int>.Get(out List<int> possibleIndices))
            {
                int totalCount = totalCollection.Count;
                for (int i = 0; i < totalCount; ++i)
                {
                    if (!usedCollection.Contains(totalCollection[i]))
                        possibleIndices.Add(i);//this index is safe to choose from
                }

                // sanity check
                if (possibleIndices.Count == 0)
                {
                    Debug.Log($"Every index used in collection. Count: {totalCollection.Count}");
                    return default;
                }

                return totalCollection[possibleIndices.GetRandomElement()];
            }
        }

        /// <summary>
        /// Shuffle elements in the collection.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Shuffle<T>(this IList<T> list)
        {
            int count = list.Count; // cache for less function overhead on every iteration
            for (int i = 0; i < count; ++i)
            {
                int randomIndex = Random.Range(0, count);
                list.Swap(i, randomIndex);
            }
        }

        /// <summary>
        /// Shuffle elements in the collection n times.
        /// 7 has been proven to be a good amount.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, int repeat)
        {
            for (int i = 0; i < repeat; ++i)
                list.Shuffle();
        }
    }
}