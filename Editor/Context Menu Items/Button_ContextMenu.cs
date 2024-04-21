using UnityEngine.UI;
using UnityEditor;
using RichPackage.FunctionalProgramming;

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
