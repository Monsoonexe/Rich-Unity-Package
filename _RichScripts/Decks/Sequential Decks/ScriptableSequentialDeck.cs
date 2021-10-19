using UnityEngine;

/// <summary>
/// Draws a card in a pre-determined sequence.
/// </summary>
/// <seealso cref="WeightedScriptableDeck"/>
/// <seealso cref="StackedScriptableDeck"/>
/// <seealso cref="ScriptableCardGenerator"/>
/// <seealso cref="ScriptableObjectDeck"/>
[CreateAssetMenu(fileName = "ScriptableSequentialDeck",
	menuName = "ScriptableObjects/Decks/ScriptableSequentialDeck")]
public class ScriptableSequentialDeck : SequentialDeck<ScriptableObject>
{
    private void Reset()
	{
		SetDevDescription("Draws a card in a pre-determined sequence.");
	}
}
