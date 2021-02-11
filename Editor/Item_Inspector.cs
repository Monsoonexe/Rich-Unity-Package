using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
[CanEditMultipleObjects]
public class Item_Inspector : Editor
{
	private Item targetObject;

    private void OnEnable()
    {
        targetObject = (Item)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUILayout.Space(10);
        GUILayout.Label("-Item Inspector-", EditorStyles.boldLabel);
        var spriteWidth = 50;
        var spriteHeight = 50;
        //draw the icon
        if (targetObject.Icon
            && targets.Length == 1) //only 1 at a time
        {
            GUILayout.Box(targetObject.Icon.texture,
                GUILayout.Width(spriteWidth), GUILayout.Height(spriteHeight));

        }
    	
    }
}
