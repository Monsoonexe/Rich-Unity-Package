using UnityEngine;

public struct WindowHistoryEntry
{
    public readonly IWindowController screen;
    public readonly IWindowProperties properties;

    public WindowHistoryEntry(IWindowController screen, IWindowProperties properties)
    {
        this.screen = screen;
        this.properties = properties;
    }

    /// <summary>
    /// Shortcut.
    /// </summary>
    public void Show()
    {
        screen.Show(properties);
    }
}
