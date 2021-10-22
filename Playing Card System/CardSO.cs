using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/New Card", fileName = "Card_V_S")]
public class CardSO : ScriptableObject
{
    /// <summary>
    /// Rank Numeric value (1-13) 1:A, 11: J, 12: Q, 13: K.
    /// </summary>
    public int value;

    /// <summary>
    /// Suit (heart, spade, club)
    /// </summary>
    public ESuit suit;

    /// <summary>
    /// Front-facing card art.
    /// </summary>
    public Sprite faceImage;

    public override string ToString()
    {
        var outputString = new System.Text.StringBuilder();
        outputString.Append(ConvertValueToString(value));
        outputString.Append(" of ");
        outputString.Append(suit.ToString());

        return outputString.ToString();
    }

    /// <summary>
    /// Returns the card that has a larger value (1 - 13; A - K)
    /// </summary>
    /// <param name="cardA"></param>
    /// <param name="cardB"></param>
    /// <returns>Returns null if cards have same value.</returns>
    public static CardSO GetLargerValue(CardSO cardA, CardSO cardB)
    {
        if(cardA.value > cardB.value)//if A is larger
        {
            return cardA;
        }
        else if(cardB.value > cardA.value)//if B is larger
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
    {
        var cardsAreSame = true;//prove me wrong

        //compare suit -- clubs to hearts
        if (cardA.suit != cardB.suit)
        {
            cardsAreSame = false;
            Debug.Log("Cards are not equal: different Suits.");//output to console for developer's sake
        }

        //compare value 10 to K
        if (cardA.value != cardB.value)
        {
            cardsAreSame = false;
            Debug.Log("Cards are not equal: different Values.");

        }

        return cardsAreSame;
    }

    public static string ConvertValueToString(int value)
    {
        string valueAsString;

        //convert face card values
        switch (value)
        {
            case 1:
                valueAsString = "Ace";
                break;

            case 11:
                valueAsString = "Jack";
                break;

            case 12:
                valueAsString = "Queen";
                break;

            case 13:
                valueAsString = "King";
                break;

            default:
                valueAsString = value.ToString();
                break;
        }

        return valueAsString;
    }
}
