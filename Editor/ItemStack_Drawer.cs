using UnityEditor;
using UnityEngine;

//[CustomPropertyDrawer(typeof(ItemStack))]
public class ItemStack_Drawer : AKeyPair_Drawer
{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {   //more space for key, less space for value
//        base.OnGUI(position, property, label); return;
//        valueWidth = 40;
//        keyWidth = 165;
//        horizontalSpace = 15;

//        var startPosition = position;

//        //begin drawing -- assigning to 'label' makes [Tooltip] and right-click ContextMenu work
//        label = EditorGUI.BeginProperty(position, label, property);

//        EditorGUI.indentLevel = 0;

//        //set new position after the label
//        position = EditorGUI.PrefixLabel(position,
//            GUIUtility.GetControlID(FocusType.Passive), label);

//        //build rects
//        var factionLabel = new Rect(position.x, position.y,
//            keyWidth, position.height);
//        position.x += horizontalSpace + keyWidth;

//        var amountLabel = new Rect(position.x, position.y,
//            valueWidth, position.height);
//        //contentPosition.x += HORIZONTAL_SPACE;

//        //draw fields
//        EditorGUI.PropertyField(factionLabel, property.FindPropertyRelative("_item"), 
//            GUIContent.none);
//        EditorGUI.PropertyField(amountLabel, property.FindPropertyRelative("value"), 
//            GUIContent.none);
        

//        //end drawing
//        EditorGUI.EndProperty();
//    }
}
