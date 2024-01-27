using UnityEngine.UI;
using UnityEditor;

namespace RichPackage.Editor
{
	/// <summary>
	/// 
	/// </summary>
	public static class Button_ContextMenu
	{
		[MenuItem("CONTEXT/Button/Invoke")]
		private static void InvokeEvent(MenuCommand command)
		{
			command.context.CastTo<Button>().onClick.Invoke();
		}
	}
}
