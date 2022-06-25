using UnityEngine;
using RichPackage.WeightedProbabilities;

namespace RichPackage.Decks
{
	/// <summary>
	/// Draws a weighted random card from a pool with replacement.
	/// </summary>
	/// <seealso cref="WeightedScriptableDeck"/>
	/// <seealso cref="ScriptableSequentialDeck"/>
	/// <seealso cref="StackedScriptableDeck"/>
	/// <seealso cref="ScriptableObjectDeck"/>
	[CreateAssetMenu(fileName = nameof(ScriptableCardGenerator),
		menuName = "ScriptableObjects/Decks/" + nameof(ScriptableCardGenerator))]
	public class ScriptableCardGenerator 
		: CardGenerator<WeightedScriptable, ScriptableObject>
	{
		//exists
	}
}
