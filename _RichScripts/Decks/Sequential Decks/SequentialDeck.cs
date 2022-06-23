using UnityEngine;

namespace RichPackage.Decks
{
	/// <summary>
	/// Always draws cards in the exact same order.
	/// </summary>
	[Sirenix.OdinInspector.InfoBox("Always draws cards in the exact same order.")]
    public class SequentialDeck<T> : Deck<T>
    {
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
