using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Decks
{
	/// <summary>
	/// Always draws cards in the exact same order.
	/// </summary>
	[InfoBox("Sequential Deck: Always draws cards in the exact same order.")]
    public class SequentialDeck<T> : Deck<T>
    {
        protected override void Reset()
		{
            SetDevDescription("Sequential Deck: Always draws cards in the exact same order.");
		}

        /// <summary>
        /// Just calls <see cref="Reload"/>.
        /// </summary>
        public override void Shuffle()
        {
            Reload();
        }

        /// <summary>
        /// This has no effect on a <see cref="SequentialDeck{T}"/> but is not an error to call.
        /// </summary>
        public override void ShuffleRemaining()
        {
            //shuffling has no random effect
        }
    }
}
