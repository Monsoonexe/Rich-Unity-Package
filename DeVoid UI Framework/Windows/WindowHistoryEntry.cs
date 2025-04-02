namespace RichPackage.UI.Framework
{
	public readonly struct WindowHistoryEntry
	{
		public readonly IWindow screen;
		public readonly IWindowProperties properties;

		public WindowHistoryEntry(IWindow screen, IWindowProperties properties)
		{
			this.screen = screen;
			this.properties = properties;
		}
	}
}
