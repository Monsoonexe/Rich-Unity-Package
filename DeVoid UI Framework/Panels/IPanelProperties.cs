/// <summary>
/// Base interface for all Panel properties
/// </summary>
public interface IPanelProperties : IScreenProperties
{
    PanelPriorityENUM Priority { get; set; }
}
