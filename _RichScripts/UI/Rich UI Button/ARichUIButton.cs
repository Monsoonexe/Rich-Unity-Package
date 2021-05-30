using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ARichUIButton : RichUIElement
{
    private static int static_ID = 0;
    private static int GetNextID { get => static_ID++; }
    /// <summary>
    /// Every RichUIButton has a unique ID. Use this to differentiate when event is called.
    /// </summary>
    public int ID { get; private set; }

    [Header("---Button---")]
    [SerializeField]
    protected Button myButton;
    public bool ButtonEnabled => myButton.enabled;

    [SerializeField]
    protected Image myImage;
    public Image Image { get => myImage; }
    public bool ImageEnabled => myImage.enabled;
    public Sprite Sprite => myImage.sprite;

    [SerializeField]
    protected TextMeshProUGUI myText;
    public TextMeshProUGUI Label { get => myText; }
    public string Text => myText.text;
    public bool TextEnabled => myText.enabled;

    /// <summary>
    /// Please call base.Awake() for maximum ease.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        ID = GetNextID;// get that ID!
        if (!myButton)
        {
            myButton = GetComponent<Button>(); //keep looking
            if (!myButton)
            {
                myButton =GetComponentInChildren<Button>(); // don't give up!
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

    /// <summary>
    /// Function called by Button. Throws Exception if no subscribers.
    /// </summary>
    protected abstract void OnButtonClick();
}
