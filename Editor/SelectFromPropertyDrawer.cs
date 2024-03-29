using NaughtyAttributes.Editor;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor
{
    [CustomPropertyDrawer(typeof(SelectFromAttribute), useForChildren: true)]
    public class SelectFromPropertyDrawer : PropertyDrawerBase
    {
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                // generate the taglist + custom tags
                List<string> tagList = new List<string>(GetOptions(property, (SelectFromAttribute)attribute));
                string propertyString = property.stringValue;
                int index = 0;
                // check if there is an entry that matches the entry and get the index
                // we skip index 0 as that is a special custom case
                for (int i = 1; i < tagList.Count; i++)
                {
                    if (tagList[i].Equals(propertyString, System.StringComparison.Ordinal))
                    {
                        index = i;
                        break;
                    }
                }

                // Draw the popup box with the current selected index
                int newIndex = EditorGUI.Popup(rect, label.text, index, tagList.ToArray());

                // Adjust the actual string value of the property based on the selection
                string newValue = newIndex >= 0 ? tagList[newIndex] : string.Empty;

                if (propertyString == string.Empty
                    || !propertyString.Equals(newValue, System.StringComparison.Ordinal))
                {
                    property.stringValue = newValue;
                }
            }
            else
            {
                string message = string.Format("{0} supports only string fields", typeof(TagAttribute).Name);
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return (property.propertyType == SerializedPropertyType.String)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected virtual IEnumerable<string> GetOptions(SerializedProperty property, SelectFromAttribute attribute)
        {
            return attribute.Options
                .Concat(GetFromMethod(property, attribute.MethodName))
                .Where(o => o != null);
        }

        private IEnumerable<string> GetFromMethod(SerializedProperty property, string method)
        {
            if (string.IsNullOrEmpty(method))
                yield break;

            SerializedObject target = property.serializedObject;
            var methodFlags = BindingFlags.NonPublic
                | BindingFlags.Public 
                | BindingFlags.Instance
                | BindingFlags.Static;

            foreach (MethodInfo m in target.GetType().GetMethods(methodFlags)
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => m.Name == method)
                .Where(m => m.ReturnType.ImplementsOrInherits(typeof(IEnumerable<string>))))
            {
                if (m.IsStatic)
                {
                    foreach (string result in (IEnumerable<string>)m.Invoke(null, null))
                    {
                        yield return result;
                    }
                }
                else
                {

                    foreach (string result in (IEnumerable<string>)m.Invoke(target, null))
                    {
                        yield return result;
                    }
                }

            }

        }
    }
}
