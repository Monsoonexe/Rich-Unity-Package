
/// <summary>
/// Interface that all Panels must implement.
/// </summary>
public interface IPanelController : IUIScreenController
{
    PanelPriorityENUM Priority { get; }
}
