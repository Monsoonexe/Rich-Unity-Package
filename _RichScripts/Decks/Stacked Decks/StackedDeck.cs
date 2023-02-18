using RichPackage.WeightedProbabilities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.Decks
{
    /// <summary>
    /// A <see cref="ADeck{TValue}"/> where not every card has the same probability of being drawn.
    /// </summary>
    [InfoBox("StackedDeck: A deck where not every card has the same " +
        "probability of being drawn and is not replaced.")]
    public class StackedDeck<TContainer, TValue> : ADeck<TValue>
        where TContainer : AWeightedProbability<TValue>
    {
        [SerializeField, Tooltip("All cards that are included in the deck.")]
        protected List<TContainer> weightedManifest = new List<TContainer>(16);

        /// <summary>
        /// face-down deck
        /// </summary>
        public readonly List<TContainer> unusedCards = new List<TContainer>(16);

        /// <summary>
        /// discard pile
        /// </summary>
        public readonly List<TContainer> usedCards = new List<TContainer>(16);

        public override int CardsRemaining { get => unusedCards.Count; }

        #region Unity Messages

        private void Reset()
        {
            SetDevDescription("StackedDeck: A deck where not every card " +
                "has the same probability of being drawn and is not replaced");
        }

        private void OnValidate()
        {
            manifest.Clear();// reload card values
            int len = weightedManifest.Count;
            for (int i = 0; i < len; ++i)
                manifest.Add(weightedManifest[i].Value); //add card to manifest
        }

        #endregion Unity Messages

        public override TValue Draw()
        {
            List<TContainer> deck = unusedCards;
            if (deck.Count == 0)
                return default;

            int iCard = deck.GetWeightedIndex();
            TContainer cardAt = deck[iCard];
            MoveCardToDiscard(iCard);
            return cardAt.Value;
        }

        /// <summary>
        /// Remove item from deck at given index and place it on top of discard pile.
        /// </summary>
        public void MoveCardToDiscard(int index)
            => usedCards.Add(unusedCards.GetRemoveAt(index));

        public override void Reload()
        {
            usedCards.Clear();
            unusedCards.Clear();

            //add all cards to unused pile
            weightedManifest.ForEachBackwards(unusedCards.Add);
        }

        /// <summary>
        /// Shuffling has no effect on a weighted random deck but is not an error to do so.
        /// </summary>
        public override void Shuffle() { } //nada

        /// <summary>
        /// Shuffling has no effect on a weighted random deck but is not an error to do so.
        /// </summary>
        public override void ShuffleRemaining() { } //nada
    }
}
