using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Generators never expend cards -- probability of outcome never changes.
/// </summary>
/// <typeparam name="TContainer">Weight and value.</typeparam>
/// <typeparam name="TValue">The thing you expect to get back.</typeparam>
public class CardGenerator<TContainer, TValue> : ADeck<TValue>
    where TContainer : AWeightedProbability<TValue>
{
    [SerializeField]
    [ReorderableList]
    protected List<TContainer> weightedManifest = new List<TContainer>();

    [ShowNativeProperty]
    public override int CardsRemaining { get => weightedManifest.Count; }

    private void OnValidate()
    {
        manifest.Clear();// reload card values
        var len = weightedManifest.Count;
        for (var i = 0; i < len; ++i)
            manifest.Add(weightedManifest[i].Value); //add card to manifest
    }

    public override TValue Draw()
    {
        var deck = weightedManifest;
        if (deck.Count == 0) return default;

        var iCard = GetWeightedIndex(deck);
        return deck[iCard].Value;         
    }

    /// <summary>
    /// Has no effect but is not an error.
    /// </summary>
    public override void Reload() { } //nada
    public override void Shuffle() { }//nada
    public override void ShuffleRemaining() { }//nada

    //in-line since generics don't play well together
    protected static int GetTotalWeight(IList<TContainer>
        probabilityTemplates)
    {
        var totalWeight = 0;
        var length = probabilityTemplates.Count;

        for (var i = 0; i < length; ++i)
            totalWeight += probabilityTemplates[i].Weight;

        return totalWeight;
    }

    //in-line since generics don't play well together
    protected static int GetWeightedIndex(IList<TContainer> items)
    {
        var totalWeight = GetTotalWeight(items);
        var randomValue = Random.Range(0, totalWeight) + 1;
        var index = 0;
        TContainer result = null;

        while (randomValue > 0)
        {
            result = items[index++];
            randomValue -= result.Weight;
        }

        return index - 1;
    }
}
