/* It doesn't work because UnityEvent isn't drawn by odin. :(
 * 
 */


using UnityEngine.Events;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;

namespace RichPackage.Editor
{
    /// <summary>
    /// Context menu entries for <see cref="UnityEvent"/>s.
    /// </summary>
 //   public class UnityEventContextMenu : OdinValueDrawer<UnityEvent>,
 //       IDefinesGenericMenuItems
	//{
 //       protected override void Initialize()
 //       {
 //           // We don't use this drawer to "draw" something so skip it when drawing.
 //           SkipWhenDrawing = true;
 //       }

 //       public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
 //       {
 //           genericMenu.AddItem(GUIHelper.TempContent(
 //               $"Invoke OnClick"),
 //               on: false,
 //               () => property.Parent.BaseValueEntry.WeakSmartValue
 //                   .CastTo<UnityEvent>().Invoke());
 //       }
 //   }
}
