using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(AKeyPair))]
public abstract class AKeyPair_Drawer : PropertyDrawer
{
    protected int valueWidth = 75;
    protected int keyWidth = 120;
    protected int horizontalSpace = 20; // end of the Inspector

    //TODO: not yet used. Intention is that the label will display these instead of generic.
    protected string keyName = "Key";
    protected string valueName = "Value";

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //begin drawing -- assigning to 'label' makes [Tooltip] and right-click ContextMenu work
        label = EditorGUI.BeginProperty(position, label, property);

        EditorGUI.indentLevel = 0;

        //set new position after the label
        position = EditorGUI.PrefixLabel(position,
            GUIUtility.GetControlID(FocusType.Passive), label);

        //build rects
        var factionLabel = new Rect(position.x, position.y,
            keyWidth, position.height);
        position.x += horizontalSpace + keyWidth;

        var amountLabel = new Rect(position.x, position.y,
            valueWidth, position.height);
        //contentPosition.x += HORIZONTAL_SPACE;

        //draw fields
        EditorGUI.PropertyField(factionLabel, property.FindPropertyRelative("key"), GUIContent.none);
        EditorGUI.PropertyField(amountLabel, property.FindPropertyRelative("value"), GUIContent.none);

        //end drawing
        EditorGUI.EndProperty();
    }
}
