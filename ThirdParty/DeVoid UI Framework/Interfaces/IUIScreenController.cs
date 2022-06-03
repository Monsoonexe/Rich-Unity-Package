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

    event Action<IUIScreenController> OnTransitionInFinishedCallback;

    event Action<IUIScreenController> OnTransitionOutFinishedCallback;

    event Action<IUIScreenController> OnScreenDestroyed;
}
