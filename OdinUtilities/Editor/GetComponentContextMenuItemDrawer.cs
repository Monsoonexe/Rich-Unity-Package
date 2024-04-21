using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using RichPackage.FunctionalProgramming;

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
            string componentName = typeof(T).Name;

            // add GetComponent<T>()
            genericMenu.AddItem(new GUIContent(
                $"{nameof(Component.GetComponent)}<{componentName}>()"),
                on: false, () => GetComponentAndSetReference(property));

            // add GetComponentInParent<T>()
            genericMenu.AddItem(new GUIContent(
                $"{nameof(Component.GetComponentInParent)}<{componentName}>()"),
                on: false, () => GetComponentInParentAndSetReference(property));

			// add GetComponentInChildren<T>()
			genericMenu.AddItem(new GUIContent(
                $"{nameof(Component.GetComponentInChildren)}<{componentName}>()"),
                on: false, () => GetComponentInChildrenAndSetReference(property));

            // add FindObjectOfType<T>()
            genericMenu.AddItem(new GUIContent(
                $"{nameof(Object.FindObjectOfType)}<{componentName}>()"),
                on: false, () => FindObjectOfTypeAndSetReference(property));

            // add AddComponent<T>()
            if (!typeof(T).IsAbstract)
            {
                genericMenu.AddItem(new GUIContent(
                    $"{nameof(GameObject.AddComponent)}<{componentName}>()"),
                    on: false, () => AddComponentAndSetReference(property));
            }
        }

        private void GetComponentAndSetReference(InspectorProperty property)
        {
            Component comp = property.Parent.BaseValueEntry.WeakSmartValue
                .CastTo<Component>()
                .GetComponent<T>();
            property.ValueEntry.WeakValues[0] = comp;
        }

        private void GetComponentInParentAndSetReference(InspectorProperty property)
        {
			Component comp = property.Parent.BaseValueEntry.WeakSmartValue
				.CastTo<Component>()
                .GetComponentInParent<T>();
			property.ValueEntry.WeakValues[0] = comp;
		}

		private void GetComponentInChildrenAndSetReference(InspectorProperty property)
        {
            Component comp = property.Parent.BaseValueEntry.WeakSmartValue
                .CastTo<Component>()
                .GetComponentInChildren<T>(includeInactive: true);
            property.ValueEntry.WeakValues[0] = comp;
        }

        private void FindObjectOfTypeAndSetReference(InspectorProperty property)
        {
            Component comp = Object.FindObjectOfType<T>(true);

            if (comp == null)
            {
                Debug.LogError("Could not find object of type " + typeof(T).Name);
            }

            property.ValueEntry.WeakValues[0] = comp;
        }
        
        private void AddComponentAndSetReference(InspectorProperty property)
        {
            Component newComponent = property.Parent.BaseValueEntry.WeakSmartValue
                .CastTo<Component>().gameObject
                .AddComponent<T>();
            property.ValueEntry.WeakValues[0] = newComponent;
        }
    }
}
