using RichPackage.Assertions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.Decks
{
    /// <summary>
    /// A deck of something (cards).
    /// </summary>
    /// <seealso cref="SequentialDeck{T}"/>
    /// <seealso cref="StackedDeck{TContainer, TValue}"/>
    /// <seealso cref="CardGenerator{TContainer, TValue}"/>
    [InfoBox("Standard Deck: A standard deck of cards with no replacement.")]
    public class Deck<T> : ADeck<T>
    {
        public readonly List<T> unusedCards = new List<T>(16); //face-down deck
        public readonly List<T> usedCards = new List<T>(16); //discard pile

        public override int CardsRemaining { get => unusedCards.Count; }

        protected virtual void Reset()
        {
            SetDevDescription("Standard Deck: A standard deck of cards with no replacement.");
        }

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
        public void AddToDeck(T newItem, int index)
        {
            unusedCards.InsertWrapped(index, newItem);
        }

        /// <summary>
        /// Adds an item into the discard pile.
        /// </summary>
        public void AddToDiscardPile(T newItem)
        {
            usedCards.Add(newItem);
        }

        public override T Draw()
        {
            // index out of range failsafe.
            if (unusedCards.Count == 0)
                return default;

            // draw from highest slot to avoid shifting all elements
            T card = unusedCards.GetRemoveLast();
            usedCards.Add(card);
            return card;
        }

        /// <summary>
        /// Remove item from deck at given index and place it on top of discard pile.
        /// </summary>
        public void MoveCardToDiscard(int index)
            => usedCards.Add(unusedCards.GetRemoveAt(index));

        /// <summary>
        /// Recombines decks without shuffling (drawn in order of Manifest).
        /// </summary>
        public override void Reload()
        {
            usedCards.Clear();
            unusedCards.Clear();

            //add all cards to unused pile.
            manifest.ForEachBackwards(unusedCards.Add);
        }

        /// <summary>
        /// Recombines un/used cards and shuffles entire deck.
        /// </summary>
        public override void Shuffle()
        {
            usedCards.Clear();
            unusedCards.Clear();
            int deckSize = TotalDeckSize;

            //temporarily use used cards array (stage)
            for (int i = 0; i < deckSize; ++i)
                usedCards.Add(manifest[i]);

            //randomly fill deck (shuffle)
            for (int i = 0; i < deckSize; ++i)
                unusedCards.Add(usedCards.GetRemoveRandomElement());
        }

        /// <summary>
        /// Shuffles only remaining cards.
        /// </summary>
        public override void ShuffleRemaining()
        {
            int startingIndex = usedCards.Count;
            int count = unusedCards.Count; //cache cards remaining
            //add to "temp array"
            while (unusedCards.Count > 0)
                usedCards.Add(unusedCards.GetRemoveLast());

            //randomly add back to unused list
            while (unusedCards.Count < count)
            {
                int randIndex = Random.Range(startingIndex, usedCards.Count);
                unusedCards.Add(usedCards.GetRemoveAt(randIndex));//get a random element within temp array bounds
            }
        }

        /// <summary>
        /// Look at an item without moving it.
        /// </summary>
        /// <param name="i">0 is top of the deck.</param>
        /// <returns></returns>
        public T PeekAt(int i)
        {
            unusedCards.IndexShouldBeInRange(i);
            return unusedCards[unusedCards.LastIndex() - i];
        }

        /// <summary>
        /// Look at next item without moving it.
        /// </summary>
        /// <returns></returns>
        public T PeekAtTop() => PeekAt(0);

        /// <summary>
        /// Look at last item without moving it.
        /// </summary>
        /// <returns></returns>
        public T PeekAtBottom()
            => PeekAt(unusedCards.LastIndex());
    }
}
