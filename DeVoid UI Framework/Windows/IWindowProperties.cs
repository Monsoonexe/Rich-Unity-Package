namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Base interface for Window properties
	/// </summary>
	public interface IWindowProperties : IScreenProperties
	{
		/// <summary>
		/// How should this window behave in case another window is already opened?
		/// </summary>
		EWindowPriority QueuePriority { get; set; }

		/// <summary>
		/// Should this Window be hidden when another Window takes its foreground?
		/// </summary>
		EWindowLostForegroundBehavior LostForegroundBehavior { get; set; }

		EWindowTakeForegroundBehavior TakeForegroundBehavior { get; set; }

		/// <summary>
		/// When Properties are passed through Open(), should it override the prefab Properties?
		/// </summary>
		bool SuppressPrefabProperties { get; set; }

		/// <summary>
		/// Popups are displayed in front of all other Windows.
		/// </summary>
		bool IsPopup { get; set; }

		/// <summary>
		/// Popups typically darken the background behind the modal window.
		/// Clear this flag to disable this behavior.
		/// </summary>
		bool ShouldDarkenBackground { get; set; }
	}
}
