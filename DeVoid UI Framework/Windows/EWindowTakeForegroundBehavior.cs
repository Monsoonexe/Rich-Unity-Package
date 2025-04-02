namespace RichPackage.UI.Framework
{
	public enum EWindowTakeForegroundBehavior
	{
		/// <summary>
		/// No special behavior.
		/// </summary>
		None = 0,

		/// <summary>
		/// Close the window that lost the foreground to this window.
		/// </summary>
		ClosePrevious = 1,

		/// <summary>
		/// Close all open windows.
		/// </summary>
		CloseAll = 2
	}
}
