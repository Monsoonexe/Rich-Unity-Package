using UnityEngine;
using ScriptableObjectArchitecture;

[CreateAssetMenu(fileName = "CollectionGenerator",
    menuName = "ScriptableObjects/Random Generators/Collection Generator")]
public class CollectionGenerator
    : ARandomGeneratorBase<WeightedSOCollection, BaseCollection>
{
    //exists
}
