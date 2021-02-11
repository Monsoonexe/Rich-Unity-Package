using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomItemStackGenerator))]
public class RandomItemStackGenerator_Inspector : ARandomGenerator_Inspector
{
    private RandomItemStackGenerator targetObject;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        targetObject = (RandomItemStackGenerator)target;

        var weightPool = targetObject.AvailablePool;
        var totalWeight = targetObject.TotalWeight;

        GUILayout.Space(10);
        GUILayout.Label("Total Weight: " + totalWeight);
        GUILayout.Space(5);
        DrawPercents(weightPool, totalWeight);
    }
}
