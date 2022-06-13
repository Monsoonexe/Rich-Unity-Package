using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using RichPackage.WeightedProbabilities;

namespace RichPackage.Decks
{
    /// <summary>
    /// Generators never expend cards -- probability of outcome never changes.
    /// </summary>
    /// <typeparam name="TContainer">Weight and value.</typeparam>
    /// <typeparam name="TValue">The underlying <see cref="System.Type"/>
    /// of the thing you expect to get back.</typeparam>
    public class CardGenerator<TContainer, TValue> : ADeck<TValue>
        where TContainer : AWeightedProbability<TValue>
    {
        [Tooltip("Modify THIS manifest and the above manifest will be automatically updated.")]
        [SerializeField]
        protected List<TContainer> weightedManifest = new List<TContainer>(16);

        [ShowInInspector, ReadOnly, PropertyTooltip("Does not decrease with use.")]
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
            if (deck.Count == 0) 
                return default;

            return deck.GetWeightedRandomElement<TValue, TContainer>();
        }

        /// <summary>
        /// Has no effect but is not an error.
        /// </summary>
        public override void Reload() { } //nada

        /// <summary>
        /// Has no effect but is not an error.
        /// </summary>
        public override void Shuffle() { }//nada

        /// <summary>
        /// Has no effect but is not an error.
        /// </summary>
        public override void ShuffleRemaining() { }//nada
    }
}
