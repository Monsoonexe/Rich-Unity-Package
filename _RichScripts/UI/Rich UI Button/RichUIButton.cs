using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

/// <summary>
/// Automatically handles subscribing and responding to a button. 
/// No need to rig Button in Scene.
/// </summary>
/// <seealso cref="RichUIButtonPayload{Tpayload}"/>
/// <seealso cref="LoadedButton{TPayload}"/>
[SelectionBase]
public class RichUIButton : RichUIElement
{
    [Header("---Button---")]
    [SerializeField]
    [Required]
    protected Button myButton;
    public Button Button { get => myButton; }
    public bool ButtonEnabled 
    { 
        get => myButton.enabled; 
        set => myButton.enabled = value; 
    }

    [SerializeField]
    [Required]
    protected Image myImage;
    public Image Image { get => myImage; }
    public bool ImageEnabled 
    {
		get => myImage.enabled; 
        set => myImage.enabled = value;
	}

    public Sprite Sprite
    {
        get => myImage.sprite;
        set => myImage.sprite = value;
    }

    [SerializeField]
    [Required]
    protected TextMeshProUGUI myText;
    public TextMeshProUGUI Label { get => myText; }
    public string Text 
    {
		get => myText.text; 
        set => myText.text = value;
	}

    public bool TextEnabled 
    {
		get => myText.enabled; 
        set => myText.enabled = value;
	}

    public bool Interactable
    {
        get => myButton.interactable;
        set => myButton.interactable = value;
    }

    //events
    public event Action<RichUIButton> OnPressedEvent;

    protected virtual void Reset()
    {
        SetDevDescription("Automatically handles subscribing " +
            "and responding to a button.");

        myButton = GetComponent<Button>();
        myImage = GetComponent<Image>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Please call base.Awake() for maximum ease.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        if (!myButton)
        {
            myButton = GetComponent<Button>(); //keep looking
            if (!myButton)
            {
                myButton = GetComponentInChildren<Button>(); // don't give up!
            }
        }

        if (!myImage)
        {
            myImage = GetComponent<Image>(); //keep looking
            if (!myImage)
            {
                myImage = GetComponentInChildren<Image>(); //keep looking
            }
        }

        if (!myText)
        {
            myText = GetComponent<TextMeshProUGUI>(); //keep looking
            if (!myText)
            {
                myText = GetComponentInChildren<TextMeshProUGUI>(); // don't give up!
            }
        }
    }

    /// <summary>
    /// call base.OnEnable() to auto subscribe to button
    /// </summary>
    protected override void OnEnable()
    {
        myButton.onClick.AddListener(OnButtonClick);
        UpdateUI();
    }

    /// <summary>
    /// call base.OnDisable() to auto unsubscribe from button
    /// </summary>
    protected virtual void OnDisable()
        => myButton.onClick.RemoveListener(OnButtonClick);

    /// <summary>
    /// Function called by Button.
    /// </summary>
    protected virtual void OnButtonClick() => OnPressedEvent?.Invoke(this);

    /// <summary>
    /// Hide/Show visual elements.
    /// </summary>
    /// <param name="active"></param>
    public override void ToggleVisuals(bool active)
    {
        myImage.enabled = active;
        myText.enabled = active;
    }

    /// <summary>
    /// Basic way to set configure look of button. For more flexibility,
    /// use Configure(RichUIButtonPayload to add more data).
    /// </summary>
    /// <param name="image"></param>
    /// <param name="label"></param>
    public virtual void SetContent(Sprite image, string label)
    {
        myImage.sprite = image;
        myText.text = label;
    }
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
public abstract class RichUIButton<TPayload> : RichUIButton
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

    public event Action<TPayload> OnPressedPayloadEvent;

    protected override void Reset()
    {
        SetDevDescription("Automatically handles subscribing " +
            "and responding to a button." +
            "\r\nAnnounces its payload when pressed.");
    }

    public virtual void SetContent(TPayload newPayloadData, Sprite image, string label)
    {
        SetContent(image, label);//forward
        PayloadData = newPayloadData;
    }

	protected override void OnButtonClick()
	{
		base.OnButtonClick();
        OnPressedPayloadEvent?.Invoke(PayloadData);
	}

	public static implicit operator TPayload (RichUIButton<TPayload> a)
        => a.PayloadData;
}
