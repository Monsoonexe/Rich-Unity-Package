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
        public static int GetWeightedIndex(
            this IList<AWeightedProbability> items)
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
            var totalWeight = GetTotalWeight((IList<AWeightedProbability>)items);
            var randomValue = Random.Range(0, totalWeight) + 1;
            var index = 0;
            AWeightedProbability<T> result = null;

            while (randomValue > 0)
            {
                result = items[index++];
                randomValue -= result.Weight;
            }

            return result.Value;
        }

        /// <summary>
        /// Sum the <see cref="AWeightedProbability.Weight"/>.
        /// </summary>
        public static int GetTotalWeight(this IList<AWeightedProbability>
            probabilityTemplates)
        {
            var totalWeight = 0;

            for (var i = 0; i < probabilityTemplates.Count; ++i)
            {
                totalWeight += probabilityTemplates[i].Weight;
            }

            return totalWeight;
        }
    }
}
