using System;
using UnityEngine;

namespace RichPackage.WeightedProbabilities
{
    /// <summary>
    /// Base class shouldn't be generic.
    /// </summary>
    /// <remarks>Done this way so base class is serializeable</remarks>
    public abstract class AWeightedProbability
    {
        [SerializeField]
        protected int weight;

        public int Weight { get => weight; } // readonly

    }

    /// <summary>
    /// Weight and thing as a pair.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class AWeightedProbability<T> : AWeightedProbability
    {
        [SerializeField]
        protected T value;

        public T Value { get => value; } // readonly 

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public AWeightedProbability()
        {
            weight = 1;
            value = default;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="value"></param>
        public AWeightedProbability(int weight, T value)
        {
            this.weight = weight;
            this.value = value;
        }

        #endregion Constructors

        /// <summary>
        /// Value: xxx | Weight: xxx
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return  string.Format(
                "Value: {0} | Weight: {1}", value, weight);
        }
    }

    public static class AWeightedProbability_Extensions
    {   //array shortcuts -> array.GetTotalyWeight()

    }
}
