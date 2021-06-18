using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// A deck of something (cards).
/// </summary>
public class Deck<T> : RichScriptableObject
{
    [SerializeField]
    private List<T> manifest = new List<T>();
    public List<T> Manifest { get => manifest; }

    public readonly List<T> unusedCards = new List<T>(); //face-down deck
    public readonly List<T> usedCards = new List<T>(); //discard pile

    [AllowNesting]
    [ShowNativeProperty]
    public int CardsRemaining { get => unusedCards.Count; }
    public int TotalCardCount { get => manifest.Count; }

    #region Constructors

    public Deck<T>(List<T> newManifest)
    {
        manifest = newManifest;
    }

    public Deck<T>(T[] newManifest)
    {
        manifest = new List<T>(newManifest);
    }

    #endregion

    /// <summary>
    /// Adds an item to the deck manifest, but it won't be included in deck until shuffled.
    /// </summary>
    public void AddToManifest<T>(T newItem)
        => manifest.Add(newItem);

    /// <summary>
    /// Adds an item to be drawn next.
    /// </summary>
    public void AddToDeckTop<T>(T newItem)
        => AddToDeck(newItem, unusedCards.Count);

    /// <summary>
    /// Adds an item to be drawn last.
    /// </summary>
    public void AddToDeckBottom<T>(T newItem)
        => AddToDeck(newItem, 0);

    /// <summary>
    /// Adds an item into the deck at a random location.
    /// </summary>
    public void AddToDeckRandom<T>(T newItem)
        => AddToDeck(newItem, Random.Range(0, unusedCards.Count + 1));//include top of deck

    /// <summary>
    /// Adds an item into the unused portion of the deck.
    /// </summary>
    /// <param name="position">How many cards deep should it go. '0' is the top (next) card. 
    ///  Negative indicates spaces from the bottom (last).</param>
    public void AddToDeck<T>(T newItem, int position)
    {
        manifest.Add(newItem);

        //negative means "from the end".
        if(position < 0)
            position = unusedCards.LastIndex() + position;

        unusedCards.Insert(position, newItem);
    }

    /// <summary>
    /// Adds an item into the deck and places it in the discard pile.
    /// </summary>
    public void AddToDiscardPile<T>(T newItem)
    {
        manifest.Add(newItem);
        usedCards.Add(newItem);
    }

    /// <summary>
    /// Remove item from deck at given index and place it on top of discard pile.
    /// </summary>
    public void MoveCardToDiscard<T>(int index)
        => usedCards.Add(unusedCards.GetRemoveAt(index));

    /// <summary>
    /// Recombines decks without shuffling (drawn in order of Manifest).
    /// </summary>
    public void ReloadDeck()
    {
        usedCards.Clear();
        unusedCards.Clear();

        manifest.ForEachBackwards(unusedCards.Add);
    }

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
        if (unusedCards.Count == 0) return default;//index out of range failsafe.
        
        //draw from highest slot to avoid shifting all elements
        var card = unusedCards.GetRemoveAt(unusedCards.LastIndex());
        usedCards.Add(card);
        return card;
    }

    public U Draw<U>() where U : T => (U)Draw();

    /// <summary>
    /// Look at an item without moving it.
    /// </summary>
    /// <param name="i">0 is top of the deck.</param>
    /// <returns></returns>
    public T PeekAt(int i)
    {
        unusedCards.AssertValidIndex(i);
        unusedCards[unusedCards.LastIndex() - i];
    } 

    /// <summary>
    /// Look at next item without moving it.
    /// </summary>
    /// <returns></returns>
    public T PeekAtTop()
        => PeekAt(0);

    /// <summary>
    /// Look at last item without moving it.
    /// </summary>
    /// <returns></returns>
    public T PeekAtBottom()
        => PeekAt(unusedCards.LastIndex());
}
