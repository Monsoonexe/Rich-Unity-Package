using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace RichPackage.UI
{
    /// <summary>
    /// Automatically handles subscribing and responding to a button. 
    /// No need to rig Button in Scene.
    /// </summary>
    /// <seealso cref="RichUIButton{Tpayload}"/>
    [SelectionBase]
    public class RichUIButton : RichUIElement
    {
        [Header("---Button---")]
        [SerializeField, Required]
        protected Button myButton;
        public Button Button { get => myButton; }
        public bool ButtonEnabled
        {
            get => myButton.enabled;
            set => myButton.enabled = value;
        }

        public ColorBlock ColorBlock 
        { 
            get => myButton.colors;
            set => myButton.colors = value;
        }

        public Image Image { get => myButton.targetGraphic as Image; }
        public bool ImageEnabled
        {
            get => myButton.targetGraphic.enabled;
            set => myButton.targetGraphic.enabled = value;
        }

        public Sprite Sprite
        {
            get => Image.sprite;
            set => Image.sprite = value;
        }

        public Color Color 
        { 
            get => myButton.targetGraphic.color; 
            set => myButton.targetGraphic.color = value; 
        }

        [SerializeField]
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

        public void AddListener(Action action) => myButton.onClick.AddListener(action.Invoke);

        public void RemoveListener(Action action) => myButton.onClick.RemoveListener(action.Invoke);

        public void RemoveAllListeners() => myButton.onClick.RemoveAllListeners();

        /// <summary>
        /// Function called by Button.
        /// </summary>
        [Button("Click"), DisableInEditorMode]
        protected virtual void OnButtonClick() => OnPressedEvent?.Invoke(this);

        /// <summary>
        /// Hide/Show visual elements.
        /// </summary>
        /// <param name="active"></param>
        public override void ToggleVisuals(bool active)
        {
            myButton.enabled = active;
            ImageEnabled = active;
            if (myText)
                myText.enabled = active;
        }

        public void Show(string text)
        {
            Text = text;
            Show();
        }

        public void Show(string text, Sprite sprite)
        {
            Text = text;
            Sprite = sprite;
            Show();
        }

        public void Show(Action onPress)
        {
            RemoveAllListeners();
            AddListener(onPress.Invoke);
            Show();
        }

        public void Show(Action onPress, Sprite sprite)
        {
            Sprite = sprite;
            Show(onPress);
        }

        public void Show(Action onPress, string text)
        {
            Text = text;
            Show(onPress);
        }

        public void Show(Action onPress, string text, Sprite sprite)
        {
            Sprite = sprite;
            Show(onPress, text);
        }

        public void Show(Sprite sprite)
        {
            Sprite = sprite;
            Show();
        }

        #region Button Helpers

        public void SetNormalColor(in Color newColor)
		{
            ColorBlock overrideColors = myButton.colors;
            overrideColors.normalColor = newColor;
            myButton.colors = overrideColors;
        }

        public void SetHighlightedColor(in Color newColor)
		{
            ColorBlock overrideColors = myButton.colors;
            overrideColors.highlightedColor = newColor;
            myButton.colors = overrideColors;
        }

        #endregion

        public static implicit operator Button (RichUIButton a) => a.Button;
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
        where TPayload : new()//has a default constructor
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

            //assume this hierarchy when initializing, but not always.
            myButton = GetComponent<Button>();
            myText = GetComponentInChildren<TextMeshProUGUI>();
        }

        public virtual void Show(TPayload newPayloadData, string label, Sprite image)
        {
            PayloadData = newPayloadData;
            Show(label, image);//forward
        }

        protected override void OnButtonClick()
        {
            base.OnButtonClick();
            OnPressedPayloadEvent?.Invoke(PayloadData);
        }

        public static implicit operator TPayload(RichUIButton<TPayload> a)
            => a.PayloadData;
    }
}
