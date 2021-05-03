using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

/// <summary>
/// Say something!
/// </summary>
public class ToggleBoxUI : RichUIElement
{
    [Header("---Toggle State---")]
    public bool ToggleValue;

    [Header("---Toggle Vernacular---")]
    public string onString = "On";
    public string offString = "Off";

    [Header("---Toggle Colors---")]
    public Color onColor;
    public Color offColor;

    [Header("---Toggle Sprites---")]
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Image toggleImage;

    [SerializeField]
    private Button toggleButton;

    [SerializeField]
    private TextMeshProUGUI toggleTextTMP;

    [Header("---Game Events---")]
    [SerializeField]
    private BoolUnityEvent toggleEvent;

    private void Awake()
    {   //subscribe to events
        toggleButton.onClick.AddListener(Toggle);
    }

    private void Start()
    {
        SetState(ToggleValue); // init starting state
    }

    private void OnDestroy()
    {   //unsubscribe
        toggleButton.onClick.RemoveListener(Toggle);
    }

    public void Toggle()
    {
        SetState(!ToggleValue);
    }

    public void SetState(bool togg)
    {
        ToggleValue = togg;
        if(togg)
        {
            toggleImage.sprite = onSprite;
            toggleImage.color = onColor;
            toggleTextTMP.text = onString;
        }
        else
        {
            toggleImage.sprite = offSprite;
            toggleImage.color = offColor;
            toggleTextTMP.text = offString;
        }
        toggleEvent.Invoke(togg);
    }
    
}
