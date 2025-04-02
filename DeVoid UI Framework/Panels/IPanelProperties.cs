namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Base interface for all Panel properties.
	/// </summary>
	public interface IPanelProperties : IScreenProperties
	{
		EPanelPriority Priority { get; set; }
	}
}
