using UnityEngine;

/// <summary>
/// Draws a random card without replacement. Standard deck.
/// </summary>
/// <seealso cref="WeightedScriptableDeck"/>
/// <seealso cref="StackedScriptableDeck"/>
/// <seealso cref="ScriptableCardGenerator"/>
/// <seealso cref="ScriptableObjectDeck"/>
/// <seealso cref="ScriptableSequentialDeck"/>
[CreateAssetMenu(fileName = "DeckOfScriptableObjects",
    menuName = "ScriptableObjects/Decks/Deck of ScriptableObjects")]
public class ScriptableObjectDeck : Deck<ScriptableObject>
{
	private void Reset()
	{
		SetDevDescription("Draws a random card without replacement. Standard deck.");
	}
}
