using UnityEngine;
using UnityEditor;

/// <summary>
///
/// </summary>
//[CustomPropertyDrawer(typeof(TransformLine))]
public class TransformLine_Drawer : PropertyDrawer
{
	//NOT IMPLEMENTED
	
	public override void OnGUI(Rect position, 
		SerializedProperty property, GUIContent label)
	{
        //begin drawing -- assigning to 'label' makes [Tooltip] and right-click ContextMenu work
        label = EditorGUI.BeginProperty(position, label, property);
        
        EditorGUI.indentLevel = 0;
        
        //set new position after the label
        position = EditorGUI.PrefixLabel(position,
            GUIUtility.GetControlID(FocusType.Passive), label);
            
        //example draw field
        //EditorGUI.PropertyField(factionLabel, property.FindPropertyRelative("key"), GUIContent.none);
        //GUITu

        //end drawing
        EditorGUI.EndProperty();
        
	}
}
