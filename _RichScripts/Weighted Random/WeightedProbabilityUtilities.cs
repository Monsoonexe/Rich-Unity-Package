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
        /// <returns>Scales of weights do not affect performance.
        /// Guarantees 1 or less iteration.</returns>
        public static int GetWeightedIndex<T>(
            this IList<T> items)
            where T : AWeightedProbability
        {
            var totalWeight = GetTotalWeight(items);
            var randomValue = Random.Range(0, totalWeight) + 1;
            var index = 0;
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
        /// <returns>Scales of weights do not affect performance
        /// Guarantees 1 or less iteration.</returns>
        public static T GetWeightedRandomElement<T>(
            this IList<AWeightedProbability<T>> items)
		{
            return GetWeightedRandomElement<T, AWeightedProbability<T>>(items);
		}

        /// <summary>
        /// Get a random element using Weighted algorithm.
        /// </summary>
        /// <returns>Scales of weights do not affect performance
        /// Guarantees 1 or less iteration.</returns>
        public static T GetWeightedRandomElement<T, U>(
            this IList<U> items)
            where U : AWeightedProbability<T>
        {
            return items[GetWeightedIndex(items)].Value;
        }

        /// <summary>
        /// Sum the <see cref="AWeightedProbability.Weight"/>.
        /// </summary>
        public static int GetTotalWeight<T>(this IList<T>
            probabilityTemplates)
            where T : AWeightedProbability
        {
            return probabilityTemplates.Sum((prob) => prob.Weight);
        }
    }
}
