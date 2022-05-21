using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

namespace RichPackage.PlayingCards
{
    [CreateAssetMenu(menuName = "ScriptableObjects/New Card", fileName = "Card_R_S")]
    public class CardSO : RichScriptableObject
    {
        /// <summary>
        /// Rank Numeric value (1-13) 1:A, 11: J, 12: Q, 13: K.
        /// </summary>
	    [FormerlySerializedAs("value")]
        [SerializeField] private CardRank rank;
        public int Rank => rank; //serialized readonly property pattern

        /// <summary>
        /// Suit (heart, spade, club)
        /// </summary>
        [SerializeField] private ESuit suit;
        public ESuit Suit => suit;

        /// <summary>
        /// Front-facing card art.
        /// </summary>
        [SerializeField, Required, PreviewField] private Sprite faceImage;
        public Sprite FaceImage => faceImage;

        protected virtual void Reset()
        {
            SetDevDescription("I'm data about an individual card in a deck.");
        }

        public override string ToString()
            => $"{Rank} of {Suit}.";

        /// <summary>
        /// Returns the card that has a larger value (1 - 13; A - K)
        /// </summary>
        /// <param name="cardA"></param>
        /// <param name="cardB"></param>
        /// <returns>Returns null if cards have same value.</returns>
        public static CardSO GetLargerRank(CardSO cardA, CardSO cardB)
        {
            if (cardA.rank > cardB.rank)//if A is larger
            {
                return cardA;
            }
            else if (cardB.rank > cardA.rank)//if B is larger
            {
                return cardB;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the card that has a larger suit (spade > heart > club > diamond)
        /// </summary>
        /// <param name="cardA"></param>
        /// <param name="cardB"></param>
        /// <returns>Returns null if cards have same value.</returns>
        public static CardSO GetLargerSuit(CardSO cardA, CardSO cardB)
        {
            if (cardA.suit > cardB.suit)//if A is larger
            {
                return cardA;
            }
            else if (cardB.suit > cardA.suit)//if B is larger
            {
                return cardB;
            }
            else
            {
                return null;//neither is larger; same size.
            }//end else
        }

        /// <summary>
        /// Compare two cards and determine if they have the same suit and value.
        /// </summary>
        /// <param name="cardA"></param>
        /// <param name="cardB"></param>
        /// <returns>True if both value and suit match.</returns>
        public static bool Equals(CardSO cardA, CardSO cardB)
            => (cardA.suit == cardB.suit) && (cardA.rank == cardB.rank);

        //auto conversions for simplicity
        public static implicit operator int(CardSO a) => a.Rank;
        public static implicit operator Sprite(CardSO a) => a.FaceImage;
    }
}
