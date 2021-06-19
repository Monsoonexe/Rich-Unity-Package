using UnityEngine;

/// <summary>
/// Uses weights to determine outcome.
/// </summary>
/// <seealso cref="WeightedScriptableDeck"/>
[CreateAssetMenu(fileName = "StackedDeckOfScriptableObjects",
    menuName = "ScriptableObjects/Decks/Stacked Deck of ScriptableObjects")]
public class StackedScriptableDeck 
    : StackedDeck<WeightedScriptable, ScriptableObject>
{
	
}
