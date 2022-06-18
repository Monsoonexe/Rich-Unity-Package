using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

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
        genericMenu.AddItem(GUIHelper.TempContent($"{nameof(Component.GetComponent)}<{typeof(T).Name}>()"), 
            on: false, () => GetComponentAndSetReference(property));
    }

    private void GetComponentAndSetReference(InspectorProperty property)
    {
        Component comp = ((Component)property.Parent.BaseValueEntry.WeakSmartValue).GetComponent<T>();
        property.ValueEntry.WeakValues[0] = comp;
    }

}
