using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract representation of a deck of something (cards)
/// </summary>
public abstract class ADeck<T> : RichScriptableObject
{
    [SerializeField]
    private List<T> manifest = new List<T>();
    public List<T> Manifest { get => manifest; }

    public readonly List<T> unusedCards = new List<T>();
    public readonly List<T> usedCards = new List<T>();

    public int CardsRemaining { get => unusedCards.Count; }
    public int TotalCardCount { get => manifest.Count; }

    /// <summary>
    /// Recombines un/used cards and shuffles entire deck.
    /// </summary>
    public void Shuffle()
    {
        usedCards.Clear();
        unusedCards.Clear();

        //temporarily use used cards array (stage)
        for (var i = 0; i < TotalCardCount; ++i)
            usedCards.Add(manifest[i]);

        //randomly fill deck (shuffle)
        for (var i = 0; i < TotalCardCount; ++i)
            unusedCards.Add(usedCards.RemoveRandomElement());
    }

    /// <summary>
    /// Shuffles only remaining cards.
    /// </summary>
    public void ShuffleRemaining()
    {
        var startingIndex = usedCards.Count;
        var count = unusedCards.Count; //cache cards remaining
        //add to "temp array"
        while (unusedCards.Count > 0)
            usedCards.Add(unusedCards.GetRemoveLast());

        //randomly add back to unused list
        while (unusedCards.Count < count)
        {
            var randIndex = Random.Range(startingIndex, usedCards.Count);
            unusedCards.Add(usedCards.GetRemoveAt(randIndex));//get a random element within temp array bounds
        }
    }

    public T Draw()
    {
        //draw from highest slot to avoid shifting all elements
        var card = unusedCards.GetRemoveAt(unusedCards.LastIndex());
        usedCards.Add(card);
        return card;
    }

    public U Draw<U>() where U : T => (U)Draw();

    /// <summary>
    /// 0 is top of the deck.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public T PeekAt(int i) => unusedCards[unusedCards.LastIndex() - i];

}
