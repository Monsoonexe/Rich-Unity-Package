using System;
using UnityEngine;

/// <summary>
/// Weight and ScriptableObject
/// </summary>
[Serializable]
public class WeightedScriptable : AWeightedProbability<ScriptableObject>
{
    //exists

    public WeightedScriptable(int weight, ScriptableObject value)
        : base(weight, value)
    {
        //constructs
    }
}
