using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CollectionGenerator))]
public class CollectionGenerator_Inspector : ARandomGenerator_Inspector
{
	private CollectionGenerator targetObject;
	
    public override void OnInspectorGUI()
    {
    	base.OnInspectorGUI();
    	
    	targetObject = (CollectionGenerator)target;

        var weightPool = targetObject.AvailablePool;
        var totalWeight = targetObject.TotalWeight;

        GUILayout.Space(10);
        GUILayout.Label("Total Weight: " + totalWeight);
        GUILayout.Space(5);
        DrawPercents(weightPool, totalWeight);
    }
}
