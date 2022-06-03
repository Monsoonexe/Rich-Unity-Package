
using System;
/// <summary>
/// Interface that all Windows must implement
/// </summary>
/// <seealso cref="WindowUILayer"/>
public interface IWindowController : IUIScreenController
{
    bool HideOnForegroundLost { get; }

    bool IsPopup { get; }

    WindowPriorityENUM WindowPriority { get; }

    event Action<IWindowController, bool> CloseRequest;

    void OnWindowOpen();//ugh
}
