using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//TODO - Peeking feature. Look at the next card to be drawn but don't actually draw it.

namespace RichPackage.Decks
{
    /// <summary>
    /// Base class for decks to help with serialization.
    /// </summary>
    /// <seealso cref="Deck{T}"/>
    /// <seealso cref="StackedDeck{TContainer, TValue}"/>
    /// <seealso cref="SequentialDeck{T}"/>
    /// <seealso cref="CardGenerator{TContainer, TValue}"/>
    public abstract class ADeck : RichScriptableObject
    {
		/// <summary>
		/// Draw()s remaining until Deck needs to Reload().
		/// </summary>
        [ShowInInspector, ReadOnly]
        public abstract int CardsRemaining { get; }

        protected virtual void OnEnable()
        {
            Reload();
        }

        /// <summary>
        /// Get a card out of the deck and cast it.
        /// </summary>
        public abstract U DrawAs<U>() where U : class;

        /// <summary>
        /// Recombine un/used cards.
        /// </summary>
        [Button]
        public abstract void Reload();

        /// <summary>
        /// Randomize the order of the cards in the deck.
        /// </summary>
        [Button]
        public abstract void Shuffle();

        /// <summary>
        /// Randomize the order of the cards in the deck that have not been dealt.
        /// </summary>
        [Button]
        public abstract void ShuffleRemaining();
    }

    /// <summary>
    /// Base class for a Deck of somethings.
    /// </summary>
    /// <typeparam name="T">The thing that you want to get out of this Deck.</typeparam>
    /// <seealso cref="Deck{T}"/>
    public abstract class ADeck<T> : ADeck
    {
        [SerializeField]
        [Tooltip("A list of the cards that belong to this deck.")]
        protected List<T> manifest = new List<T>(16);

        /// <summary>
        /// A list of the cards that belong to this deck.
        /// </summary>
        public List<T> Manifest { get => manifest; }

        public int TotalDeckSize { get => manifest.Count; }

        /// <summary>
        /// Get a card out of the deck.
        /// </summary>
        public abstract T Draw();

        /// <summary>
        /// Get a card out of the deck and cast it.
        /// </summary>
        public U Draw<U>() where U : T => (U)Draw();

        /// <summary>
        /// Get a card out of the deck and cast it.
        /// </summary>
        public override U DrawAs<U>() => Draw() as U;

        [Button]
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public void TestDraw()
        {
            var card = Draw(); //breakpoint here.
            UnityEngine.Debug.Log(card);
        }
    }
}
