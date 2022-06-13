using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace RichPackage.WeightedProbabilities
{
    /// <summary>
    /// Utilities for dealing with collections of <see cref="AWeightedProbability"/>.
    /// </summary>
    public static class WeightedProbabilityUtilities
    {
        /// <summary>
        /// Get a random element using Weighted algorithm.
        /// </summary>
        public static int GetWeightedIndex<T>(
            this IList<T> items)
            where T : AWeightedProbability
        {
            int totalWeight = GetTotalWeight(items);
            int randomValue = Random.Range(0, totalWeight) + 1;
            return GetWeightedIndex(items, randomValue);
        }

        /// <summary>
        /// Get a random element using Weighted algorithm.
        /// </summary>
        /// <param name="randomValue">The weighted random value (ticket).</param>
        public static int GetWeightedIndex<T>(
            this IList<T> items, int randomValue)
            where T : AWeightedProbability
        {
            int index = 0;
            AWeightedProbability result;

            while (randomValue > 0)
            {
                result = items[index++];
                randomValue -= result.Weight;
            }

            return index - 1;
        }

        /// <summary>
        /// Get a random element using Weighted algorithm.
        /// </summary>
        public static T GetWeightedRandomElement<T>(
            this IList<AWeightedProbability<T>> items)
		{
            return GetWeightedRandomElement<T, AWeightedProbability<T>>(items);
		}

        /// <summary>
        /// Get a random element using Weighted algorithm.
        /// </summary>
        public static T GetWeightedRandomElement<T, U>(
            this IList<U> items)
            where U : AWeightedProbability<T>
        {
            return items[GetWeightedIndex(items)].Value;
        }

        /// <summary>
        /// Sum the <see cref="AWeightedProbability.Weight"/>s.
        /// </summary>
        public static int GetTotalWeight<T>(this IList<T>
            probabilityTemplates)
            where T : AWeightedProbability
        {
            return probabilityTemplates.Sum((prob) => prob.Weight);
        }
    }
}
