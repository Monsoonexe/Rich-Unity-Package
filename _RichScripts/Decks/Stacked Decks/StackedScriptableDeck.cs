using UnityEngine;

namespace RichPackage.Decks
{
	/// <summary>
	/// Draws a weighted random card without replacement.
	/// </summary>
	/// <seealso cref="WeightedScriptableDeck"/>
	/// <seealso cref="ScriptableSequentialDeck"/>
	/// <seealso cref="ScriptableCardGenerator"/>
	/// <seealso cref="ScriptableObjectDeck"/>
	[CreateAssetMenu(fileName = "StackedDeckOfScriptableObjects",
		menuName = "ScriptableObjects/Decks/Stacked Deck of ScriptableObjects")]
	public class StackedScriptableDeck 
		: StackedDeck<WeightedScriptable, ScriptableObject>
	{
		private void Reset()
		{
			SetDevDescription("Draws a weighted random card without replacement.");
		}
	}
}
