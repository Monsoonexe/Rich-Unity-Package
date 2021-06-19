using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

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

    #endregion

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


    /// <summary>
    /// Get a random index using Weighted algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns>Scales of weights do not affect performance. Guarantees 1 or less iteration. </returns>
    public static int GetWeightedIndex(this IList<int> items)
    {
        var totalWeight = GetTotalWeight(items);
        var randomValue = Random.Range(0, totalWeight) + 1;
        var index = 0;

        while (randomValue > 0)
            randomValue -= items[index++];

        return index - 1;
    }

    /// <summary>
    /// Get a random element using Weighted algorithm.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns>Scales of weights do not affect performance. Guarantees 1 or less iteration. </returns>
    public static int GetWeightedIndex(
        this IList<AWeightedProbability> items)
    {
        var totalWeight = GetTotalWeight(items);
        var randomValue = Random.Range(0, totalWeight) + 1;
        var index = 0;
        AWeightedProbability result = null;

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
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns>Scales of weights do not affect performance. Guarantees 1 or less iteration. </returns>
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
    /// Get a random element using Weidghted algorithm.
    /// </summary>
    /// <typeparam name="TContainer"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="generator"></param>
    /// <returns></returns>
    public static TValue GetWeightedRandomElement<TContainer, TValue>(
        this ARandomGeneratorBase<TContainer, TValue> generator)
        where TContainer : AWeightedProbability<TValue>
    {
        var totalWeight = generator.TotalWeight;
        var items = generator.Pool;
        var randomValue = Random.Range(0, totalWeight) + 1;
        var index = 0;
        AWeightedProbability<TValue> result = null;

        while (randomValue > 0)
        {
            result = items[index++];
            randomValue -= result.Weight;
        }

        return result.Value;
    }
    /// <summary>
    /// Sum up Weights in given collection
    /// </summary>
    /// <param name="probabilityTemplates"></param>
    /// <returns></returns>
    public static int GetTotalWeight(this IList<int> weights)
    {
        var total = 0;

        for (var i = weights.Count; i >= 0; --i)
            total += weights[i];

        return total;
    }

    /// <summary>
    /// Sum up Weights in given collection
    /// </summary>
    /// <param name="probabilityTemplates"></param>
    /// <returns></returns>
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
