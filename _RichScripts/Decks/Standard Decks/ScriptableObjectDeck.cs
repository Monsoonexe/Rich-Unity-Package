using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Decks
{
	/// <summary>
	/// Draws a random card without replacement. Standard deck.
	/// </summary>
	/// <seealso cref="WeightedScriptableDeck"/>
	/// <seealso cref="StackedScriptableDeck"/>
	/// <seealso cref="ScriptableCardGenerator"/>
	/// <seealso cref="ScriptableObjectDeck"/>
	/// <seealso cref="ScriptableSequentialDeck"/>
	[CreateAssetMenu(fileName = "DeckOfScriptableObjects",
		menuName = "ScriptableObjects/Decks/Deck of ScriptableObjects"),
		InfoBox("Draws a random card without replacement. Standard deck of playing cards.")]
	public sealed class ScriptableObjectDeck : Deck<ScriptableObject>
	{
		protected override void Reset()
		{
			SetDevDescription("Draws a random card without replacement. Standard deck of playing cards.");
		}
	}
}
