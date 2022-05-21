using System.Collections.Generic;
using UnityEngine;
using RichPackage.WeightedProbabilities;

namespace RichPackage.Decks
{
    /// <summary>
    /// Not every card has the same probability of being drawn.
    /// </summary>
    public class StackedDeck<TContainer, TValue> : ADeck<TValue>
        where TContainer : AWeightedProbability<TValue>
    {
        [SerializeField]
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

        private void OnValidate()
        {
            manifest.Clear();// reload card values
            var len = weightedManifest.Count;
            for (var i = 0; i < len; ++i)
                manifest.Add(weightedManifest[i].Value); //add card to manifest
        }

        public override TValue Draw()
        {
            var deck = unusedCards;
            if (deck.Count == 0) return default;

            var iCard = GetWeightedIndex(deck);
            var cardAt = deck[iCard];
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

        protected static int GetTotalWeight(IList<TContainer>
            probabilityTemplates)
        {
            var totalWeight = 0;
            var length = probabilityTemplates.Count;

            for (var i = 0; i < length; ++i)
                totalWeight += probabilityTemplates[i].Weight;

            return totalWeight;
        }

        protected static int GetWeightedIndex(IList<TContainer> items)
        {
            var totalWeight = GetTotalWeight(items);
            var randomValue = Random.Range(0, totalWeight) + 1;
            var index = 0;
            TContainer result;

            while (randomValue > 0)
            {
                result = items[index++];
                randomValue -= result.Weight;
            }

            return index - 1;
        }
    }
}
