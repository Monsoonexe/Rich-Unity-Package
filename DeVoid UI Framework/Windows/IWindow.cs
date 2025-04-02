namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Interface that all Windows must implement
	/// </summary>
	/// <seealso cref="WindowUILayer"/>
	public interface IWindow : IUIScreen
	{
		// note most of these Properties are just properties of IWindowProperties...
		// why not just expose the properties and remove the copy-paste?

		/// <summary>
		/// 
		/// </summary>
		bool IsPopup { get; }

		/// <summary>
		/// 
		/// </summary>
		bool ShouldDarkenBackground { get; }

		/// <summary>
		/// 
		/// </summary>
		IWindowProperties Properties { get; } // maybe expose this instead of nesting properties?

		/// <summary>
		/// The frame that owns this window.
		/// </summary>
		UIFrame Frame { get; set; }

		/// <summary>
		/// Called when a new window has taken the foreground (not called if hidden).
		/// </summary>
		void OnForegroundLost();
	}
}
