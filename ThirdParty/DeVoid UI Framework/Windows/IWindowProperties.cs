
/// <summary>
/// Base interface for Window properties
/// </summary>
public interface IWindowProperties : IScreenProperties
{
    WindowPriorityENUM WindowQueuePriority { get; set; }

    bool HideOnForegroundLost { get; set; }

    bool IsPopup { get; set; }

    bool SuppressPrefabProperties { get; set; }
}
