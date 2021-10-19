using UnityEngine;

/// <summary>
/// Draws a weighted random card from a pool with replacement.
/// </summary>
/// <seealso cref="WeightedScriptableDeck"/>
/// <seealso cref="ScriptableSequentialDeck"/>
/// <seealso cref="StackedScriptableDeck"/>
/// <seealso cref="ScriptableObjectDeck"/>
[CreateAssetMenu(fileName = "ScriptableCardGenerator",
	menuName = "ScriptableObjects/Decks/ScriptableCardGenerator")]
public class ScriptableCardGenerator 
    : CardGenerator<WeightedScriptable, ScriptableObject>
{
	private void Reset()
	{
		SetDevDescription("Draws a weighted random card from a pool with replacement.");
	}
}
