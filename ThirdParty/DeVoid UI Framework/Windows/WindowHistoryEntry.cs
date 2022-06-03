using System.Runtime.CompilerServices;

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Show()
    {
        screen.Show(properties);
    }
}
