using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

namespace RichPackage.Editor
{
    public class GetComponentContextMenuItemDrawer<T>
        : OdinValueDrawer<T>, IDefinesGenericMenuItems
        where T : Component
    {
        protected override void Initialize()
        {
            // We don't use this drawer to "draw" something so skip it when drawing.
            SkipWhenDrawing = true;
        }

        public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
        {
            // add GetComponent<T>()
            genericMenu.AddItem(new GUIContent(
                $"{nameof(Component.GetComponent)}<{typeof(T).Name}>()"),
                on: false, () => GetComponentAndSetReference(property));
            
            // add GetComponentInChildren<T>()
            genericMenu.AddItem(new GUIContent(
                $"{nameof(Component.GetComponentInChildren)}<{typeof(T).Name}>()"),
                on: false, () => GetComponentAndSetReference(property));

            // add FindObjectOfType<T>()
            genericMenu.AddItem(new GUIContent(
                $"{nameof(Object.FindObjectOfType)}<{typeof(T).Name}>()"),
                on: false, () => FindObjectOfTypeAndSetReference(property));
        }

        private void GetComponentAndSetReference(InspectorProperty property)
        {
            Component comp = property.Parent.BaseValueEntry.WeakSmartValue
                .CastTo<Component>().GetComponent<T>();
            property.ValueEntry.WeakValues[0] = comp;
        }

        private void GetComponentInChildrenAndSetReference(InspectorProperty property)
        {
            Component comp = property.Parent.BaseValueEntry.WeakSmartValue
                .CastTo<Component>().GetComponentInChildren<T>();
            property.ValueEntry.WeakValues[0] = comp;
        }

        private void FindObjectOfTypeAndSetReference(InspectorProperty property)
        {
            Component comp = Object.FindObjectOfType<T>();

            if (comp == null)
            {
                Debug.LogError("Could not find object of type " + typeof(T).Name);
            }

            property.ValueEntry.WeakValues[0] = comp;
        }
    }
}
