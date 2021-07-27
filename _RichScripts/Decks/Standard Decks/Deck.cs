using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// A deck of something (cards).
/// </summary>
public class Deck<T> : RichScriptableObject
{
    [SerializeField]
    [ReorderableList]
    protected List<T> manifest = new List<T>();
    public List<T> Manifest { get => manifest; }

    public readonly List<T> unusedCards = new List<T>(); //face-down deck
    public readonly List<T> usedCards = new List<T>(); //discard pile

    [ShowNativeProperty]
    public virtual int CardsRemaining { get => unusedCards.Count; }
    public int TotalCardCount { get => manifest.Count; }

    /// <summary>
    /// Adds an item to the deck manifest, but it won't be included in deck until shuffled.
    /// </summary>
    public void AddToManifest(T newItem)
        => manifest.Add(newItem);

    /// <summary>
    /// Adds an item to be drawn next.
    /// </summary>
    public void AddToDeckTop(T newItem)
        => AddToDeck(newItem, unusedCards.Count);

    /// <summary>
    /// Adds an item to be drawn last.
    /// </summary>
    public void AddToDeckBottom(T newItem)
        => AddToDeck(newItem, 0);

    /// <summary>
    /// Adds an item into the deck at a random location.
    /// </summary>
    public void AddToDeckRandom(T newItem)
        => AddToDeck(newItem, Random.Range(0, unusedCards.Count + 1));//include top of deck

    /// <summary>
    /// Adds an item into the unused portion of the deck.
    /// </summary>
    /// <param name="position">How many cards deep should it go. '0' is the top (next) card. 
    ///  Negative indicates spaces from the bottom (last).</param>
    public void AddToDeck(T newItem, int position)
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
    public void AddToDiscardPile(T newItem)
    {
        manifest.Add(newItem);
        usedCards.Add(newItem);
    }

    /// <summary>
    /// Remove item from deck at given index and place it on top of discard pile.
    /// </summary>
    public void MoveCardToDiscard(int index)
        => usedCards.Add(unusedCards.GetRemoveAt(index));

    /// <summary>
    /// Recombines decks without shuffling (drawn in order of Manifest).
    /// </summary>
    public virtual void ReloadDeck()
    {
        usedCards.Clear();
        unusedCards.Clear();

        //add all cards to unused pile.
        manifest.ForEachBackwards(unusedCards.Add);
    }

    /// <summary>
    /// Recombines un/used cards and shuffles entire deck.
    /// </summary>
    [Button] 
    public virtual void Shuffle()
    {
        usedCards.Clear();
        unusedCards.Clear();

        //temporarily use used cards array (stage)
        for (var i = 0; i < TotalCardCount; ++i)
            usedCards.Add(manifest[i]);

        //randomly fill deck (shuffle)
        for (var i = 0; i < TotalCardCount; ++i)
            unusedCards.Add(usedCards.GetRemoveRandomElement());
    }

    /// <summary>
    /// Shuffles only remaining cards.
    /// </summary>
    public virtual void ShuffleRemaining()
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

    [Button]
    public virtual void TestDraw()
    {
        var card = Draw();
        Debug.Log(card);
    }

    public virtual T Draw()
    {
        if (unusedCards.Count == 0) return default;//index out of range failsafe.

        //draw from highest slot to avoid shifting all elements
        var removeIndex = unusedCards.LastIndex();
        var card = unusedCards.GetRemoveAt(removeIndex);
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
        return unusedCards[unusedCards.LastIndex() - i];
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
