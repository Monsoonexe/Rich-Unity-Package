using UnityEngine;

/// <summary>
/// Generates a random ScriptableObject from a list of weights.
/// </summary>
[CreateAssetMenu(fileName = "RandomScriptableGen",
    menuName = "ScriptableObjects/Random Generators/ScriptableObject")]
public class RandomScriptableGenerator 
    : ARandomGeneratorBase<WeightedScriptable, ScriptableObject>
{
	//exists
}
