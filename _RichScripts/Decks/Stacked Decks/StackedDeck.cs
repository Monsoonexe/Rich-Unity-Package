using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Not every card has the same probability of being drawn.
/// </summary>
public class StackedDeck<TContainer, TValue> : Deck <TValue>
    where TContainer : AWeightedProbability<TValue>
{
    [SerializeField]
    [Expandable]
    protected Deck<TContainer> stackedDeck = null;

    public override int CardsRemaining { get => stackedDeck.unusedCards.Count; }

    public override TValue Draw()
    {
        var deck = stackedDeck.unusedCards;
        if (deck.Count == 0) return default;

        var iCard = GetWeightedIndex(deck);
        var cardAt = deck[iCard];
        stackedDeck.MoveCardToDiscard(iCard);
        return cardAt.Value;
    }

    public override void ReloadDeck()
    {
        stackedDeck.ReloadDeck();
    }

    public override void Shuffle()
    {
        stackedDeck.Shuffle();
    }

    public override void ShuffleRemaining()
    {
        stackedDeck.ShuffleRemaining();
    }

    protected static int GetTotalWeight(IList<TContainer>
        probabilityTemplates)
    {
        var totalWeight = 0;

        for (var i = 0; i < probabilityTemplates.Count; ++i)
        {
            totalWeight += probabilityTemplates[i].Weight;
        }

        return totalWeight;
    }

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
