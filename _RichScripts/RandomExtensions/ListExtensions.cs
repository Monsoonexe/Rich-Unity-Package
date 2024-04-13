using RichPackage.GuardClauses;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace RichPackage.RandomExtensions
{
    /// <summary>
    /// Random extension methods for lists.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Removes and returns a random element from <paramref name="list"/>.
        /// </summary>
        public static T TakeRandomElement<T>(this List<T> list)
        {
            GuardAgainst.ArgumentIsNull(list, nameof(list));
            Assert.IsTrue(list.IsNotEmpty(), "No items in " + nameof(list));

            int randomIndex = Random.Range(0, list.Count);
            T randomElement = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomElement;
        }

        /// <summary>
        /// Remove a random element in [start, end).
        /// </summary>
        public static T TakeRandomElement<T>(this List<T> list,
            int start, int end = int.MaxValue)
        {
            GuardAgainst.ArgumentIsNull(list, nameof(list));
            Assert.IsTrue(list.IsNotEmpty(), "No items in " + nameof(list));

            int count = list.Count;

            // validate
            start = Mathf.Clamp(start, 0, count - 1); // start [0, count - 1]
            end = Mathf.Clamp(end, 1, count); // end [1, count]

            // operate
            int randomIndex = Random.Range(start, end);
            T randomElement = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomElement;
        }

        /// <param name="count">At least this many items to take.</param>
        public static T[] TakeRandomElements<T>(this List<T> list, int count)
        {
            count = Mathf.Clamp(count, 0, list.Count);
            var result = new T[count];

            for (int i = 0; i < count; ++i)
                result[i] = list.TakeRandomElement();

            return result;
        }
    }
}
