using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// 
/// </summary>
public static class Button_ContextMenu
{
	[MenuItem("CONTEXT/Button/Force Invoke")]
	private static void ForceInvokeEvent(MenuCommand command)
	{
		Button button = command.context as Button;
		button.onClick.Invoke();
	}
}
