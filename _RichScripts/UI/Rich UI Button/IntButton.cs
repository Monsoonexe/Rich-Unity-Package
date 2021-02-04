using System;

/// <summary>
/// A button that raises an int event when pressed.
/// </summary>
public class IntButton : RichUIButton<int>
{
    //events
    public event Action<IntButton> OnPressedEvent;

    protected override void OnButtonClick() => OnPressedEvent(this);
}
