using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor
{
    [CustomPropertyDrawer(typeof(UniqueID))]
    public class UniqueIDDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // hacky, but I don't know how to get the '<ID>k_BackingValue' of the id.
            // So just get the first, and only, property.
            // Hopefully this never breaks ever.
            SerializedProperty idProp = null;
            foreach (SerializedProperty p in property)
            {
                idProp = p;
                break;
            }

            // Draw ID field
            EditorGUI.PropertyField(position, idProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}
