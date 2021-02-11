using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomIntGenerator))]
public class RandomIntGenerator_Inspector : ARandomGenerator_Inspector
{
	private RandomIntGenerator targetObject;
	
    public override void OnInspectorGUI()
    {
    	base.OnInspectorGUI();
    	
    	targetObject = (RandomIntGenerator)target;

        var weightPool = targetObject.AvailablePool;
        var totalWeight = targetObject.TotalWeight;

        GUILayout.Space(10);
        GUILayout.Label("Total Weight: " + totalWeight);
        GUILayout.Space(5);
        DrawPercents(weightPool, totalWeight);
    }
}
