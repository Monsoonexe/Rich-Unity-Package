using System;

/// <summary>
/// Interface that all UI Screens must implement in/directly
/// </summary>
public interface IUIScreenController
{
    string ScreenID { get; set; }

    bool IsVisible { get; }

    void Show(IScreenProperties payload = null);

    void Hide(bool animate = true);

    Action<IUIScreenController> OnTransitionInFinishedCallback { get; set; }

    Action<IUIScreenController> OnTransitionOutFinishedCallback { get; set; }

    Action<IUIScreenController, bool> CloseRequest { get; set; }

    Action<IUIScreenController> OnScreenDestroyed { get; set; }
}
