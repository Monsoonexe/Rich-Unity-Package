namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Interface that all Panels must implement.
	/// </summary>
	public interface IPanel : IUIScreen
	{
		EPanelPriority Priority { get; }
		PanelUILayer Layer { get; set; }
	}
}
