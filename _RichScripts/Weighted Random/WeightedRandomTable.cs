using RichPackage.RNG;
using System;
using System.Collections.Generic;

namespace RichPackage.WeightedProbabilities
{
    public class WeightedRandomTable<T> : List<AWeightedProbability<T>>
    {
        public WeightedRandomTable() : base()
        {
            // Constructor
        }
        public WeightedRandomTable(int capacity) : base(capacity)
        {
            // Constructor
        }
        public WeightedRandomTable(IList<AWeightedProbability<T>> collection) : base(collection)
        {
            // Constructor
        }
        public WeightedRandomTable(IEnumerable<AWeightedProbability<T>> collection) : base(collection)
        {
            // Constructor
        }
    }

    public static class WeightedRandomTableExtensions
    {
        /// <summary>
        /// Weighted random.
        /// </summary>
        public static int GetTotalWeight<T>(this IList<T> table)
            where T : AWeightedProbability
        {
            int totalWeight = 0;
            int count = table.Count;
            for (int i = 0; i < count; i++)
            {
                totalWeight += table[i].Weight;
            }

            return totalWeight;
        }

        /// <summary>
        /// Weighted random.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static T GetRandomItem<T>(this IList<AWeightedProbability<T>> table)
        {
            return GetRandomItem<AWeightedProbability<T>, T>(table, Rng.Current);
        }

        /// <summary>
        /// Weighted random.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static U GetRandomItem<T, U>(this IList<T> table)
            where T : AWeightedProbability<U>
        {
            return GetRandomItem<T, U>(table, Rng.Current);
        }

        /// <summary>
        /// Weighted random.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static U GetRandomItem<T, U>(this IList<T> table, IRandomNumberGenerator random)
            where T : AWeightedProbability<U>
        {
            int count = table.Count;
            int totalWeight = GetTotalWeight(table);
            int randomValue = random.Range(0, totalWeight);
            int cumulativeWeight = 0;

            for (int i = 0; i < count; i++)
            {
                T kvp = table[i];
                cumulativeWeight += kvp.Weight;
                if (randomValue < cumulativeWeight)
                {
                    return kvp.Value; // hit
                }
            }

            throw new InvalidOperationException("Failed to select a weighted random choice.");
        }
    }
}
