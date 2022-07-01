using UnityEngine;

namespace RichPackage.Decks
{
	/// <summary>
	/// Draws a card in a pre-determined sequence.
	/// </summary>
	/// <seealso cref="WeightedScriptableDeck"/>
	/// <seealso cref="StackedScriptableDeck"/>
	/// <seealso cref="ScriptableCardGenerator"/>
	/// <seealso cref="ScriptableObjectDeck"/>
	[CreateAssetMenu(fileName = "ScriptableSequentialDeck",
		menuName = "ScriptableObjects/Decks/ScriptableSequentialDeck")]
	public sealed class ScriptableSequentialDeck : SequentialDeck<ScriptableObject>
	{
		protected override void Reset()
		{
			SetDevDescription("Draws the next card in a pre-determined sequence.");
		}
	}
}
