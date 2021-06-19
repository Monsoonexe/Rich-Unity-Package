using UnityEngine;

/// <summary>
/// Returns a weightedScriptable instead of a Scriptable, so it's used as a backer usually.
/// Does NOT use weights. Regular deck.
/// </summary>
/// <seealso cref="StackedScriptableDeck"/>
[CreateAssetMenu(fileName = "WeightedScriptableDeck",
	menuName = "ScriptableObjects/Decks/Weighted Scriptable Deck")]
public class WeightedScriptableDeck : Deck<WeightedScriptable>
{
    
}
