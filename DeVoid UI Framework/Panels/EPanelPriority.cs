namespace RichPackage.UI.Framework
{
	public enum EPanelPriority
	{
		/// <summary>
		/// Standard panel priority.
		/// </summary>
		Default = 0,

		/// <summary>
		/// Higher priority than a regular panel and most windows.
		/// </summary>
		Priority = 1,

		/// <summary>
		/// The highest priority of panels. Useful for tutorial elements.
		/// </summary>
		SuperPriority = 2
	}
}
