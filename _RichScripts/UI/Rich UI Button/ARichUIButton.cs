using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class ARichUIButton : RichUIElement
{
    private static int static_ID = 0;
    /// <summary>
    /// Every RichUIButton has a unique ID. Use this to differentiate when event is called.
    /// </summary>
    public int ID { get; private set; }

    [Header("---Button---")]
    [SerializeField]
    protected Button myButton;
    public Button Button => myButton;

    [SerializeField]
    protected Image myImage;
    public Image Image => myImage;
    public Sprite Sprite { get => myImage.sprite; set => myImage.sprite = value; }

    [SerializeField]
    protected TextMeshProUGUI myText;
    public TextMeshProUGUI Label => myText;
    public string Text { get => myText.text; set => myText.text = value; }

    /// <summary>
    /// Please call base.Awake() for maximum ease.
    /// </summary>
    protected virtual void Awake()
    {
        ID = static_ID++;// get that ID!
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
