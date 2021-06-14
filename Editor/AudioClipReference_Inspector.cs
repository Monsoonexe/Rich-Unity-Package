using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioClipReference))]
public class AudioClipReference_Inspector : Editor
{
	private AudioClipReference targetObject;
	
    public override void OnInspectorGUI()
    {
    	base.OnInspectorGUI();
    	
    	targetObject = (AudioClipReference)target;
    	
    	GUILayout.Label("AudioClipReference_Inspector", EditorStyles.boldLabel);

        // if(targetObject.Value != null && GUILayout.Button("Preview Clip"))
        // {
        //     RichEditorUtility.EditorPlayClip(targetObject);
        // }
    }
}
