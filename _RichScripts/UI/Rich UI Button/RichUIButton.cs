using System;
using UnityEngine;

/// <summary>
/// Automatically handles subscribing and responding to a button. 
/// No need to rig Button in Scene.
/// </summary>
/// <seealso cref="RichUIButtonPayload{Tpayload}"/>
/// <seealso cref="LoadedButton{TPayload}"/>
public sealed class RichUIButton : ARichUIButton
{
    /// <summary>
    /// call base.OnEnable() to auto subscribe to button
    /// </summary>
    private void OnEnable()
        => myButton.onClick.AddListener(OnButtonClick);

    /// <summary>
    /// call base.OnDisable() to auto unsubscribe from button
    /// </summary>
    private void OnDisable()
        => myButton.onClick.RemoveListener(OnButtonClick);

    /// <summary>
    /// Function called by Button. Throws Exception if no subscribers.
    /// </summary>
    protected override void OnButtonClick() => OnPressedEvent(this);

    //events
    public event Action<RichUIButton> OnPressedEvent;
}

/// <summary>
/// Automatically handles subscribing and responding to a button. 
/// No need to rig Button in the Scene.
/// </summary>
/// <remarks>A "Payload" is any other data the button should hold. This is useful for mapping
/// a button to information, like "when I push this button, give me one tall potion". 
/// You can assign the payload to the button, then get the payload off the button 
/// when it is pushed.
/// </remarks>
/// <seealso cref="IntButton"/>
public abstract class RichUIButton<TPayload> : ARichUIButton
    where TPayload : new()//is a payload and has a default constructor
{
    /// <summary>
    /// class that holds the actual payload and any other data. Like if you want to play
    /// particle effect on click or something, that data would be held inside.
    /// </summary>
    [SerializeField]
    [Tooltip("Can be set in Inspector, but probably should be set " +
        "using Configure() at runtime")]
    public TPayload PayloadData = default;

    //EVERY DERIVING CLASS SHOULD HAVE THESE TWO PROPERTIES!
    //events
    //public event Action<RichUIButton> OnPressedEvent;
    //protected override void OnButtonClick() => OnPressedEvent(this);

    /// <summary>
    /// call base.OnEnable() to auto subscribe to button
    /// </summary>
    protected virtual void OnEnable()
        => myButton.onClick.AddListener(OnButtonClick);

    /// <summary>
    /// call base.OnDisable() to auto unsubscribe from button
    /// </summary>
    protected virtual void OnDisable()
        => myButton.onClick.RemoveListener(OnButtonClick);

    public virtual void SetContent(TPayload newPayloadData, Sprite image, string label)
    {
        SetContent(image, label);//forward
        PayloadData = newPayloadData;
        //space for other stuff.
    }

    public static implicit operator TPayload (RichUIButton<TPayload> a)
        => a.PayloadData;
}
