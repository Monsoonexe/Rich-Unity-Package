using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

namespace RichPackage.UI
{
    /// <summary>
    /// Automatically handles subscribing and responding to a button. 
    /// Can also store information in its Properties and retrieve it later.
    /// </summary>
    [SelectionBase]
    public class RichUIButton : RichUIElement
    {
        #region Constants

        private const string Default_String_Property = "--";
        private const string Properties_Group = "Properties";

		#endregion Constants

		[Title("Refs")]
        [SerializeField, Required]
        protected Button myButton;
        public Button Button { get => myButton; }

        [SerializeField]
        protected TextMeshProUGUI myText;
        public TextMeshProUGUI Label { get => myText; }

		[Tooltip("Optional value that can be stored and retrieved later.")]
        [LabelWidth(100), BoxGroup(Properties_Group)]
        public int IntegerProperty = -1;

        [Tooltip("Optional value that can be stored and retrieved later.")]
        [LabelWidth(100), BoxGroup(Properties_Group)]
        public string StringProperty = Default_String_Property;

        /// <summary>
        /// An optional value that can be placed on the button and retrieved later. Useful
        /// to help with mapping data to controls.
        /// </summary>
        public object ObjectProperty;

#if UNITY_EDITOR

        [ShowInInspector, ReadOnly, BoxGroup(Properties_Group),
            LabelText(nameof(ObjectProperty)), LabelWidth(100),
            PropertyTooltip("Optional value that can be stored and retrieved later.")]
        public string Editor_ObjectPayloadString
        {
            get => ObjectProperty?.ToString() ?? Default_String_Property;
        }

#endif

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

        /// <summary>
        /// Calls events with <see langword="this"/> as the argument.
        /// </summary>
        public event Action<RichUIButton> OnPressedEvent;

		#region Unity Messages

		protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Automatically handles subscribing " +
                "and responding to a button. Can store and retrieve data.");
            if (!TryGetComponent(out myButton))
                myButton = GetComponentInChildren<Button>();
            myText = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected override void Awake()
        {
            base.Awake();
            if (!myButton)
            {
                if (!TryGetComponent(out myButton))
                    myButton = GetComponentInChildren<Button>();
            }
        }

        /// <summary>
        /// call base.OnEnable() to auto subscribe to button
        /// </summary>
        protected override void OnEnable()
        {
            myButton.onClick.AddListener(Click);
            UpdateUI();
        }

        /// <summary>
        /// call base.OnDisable() to auto unsubscribe from button
        /// </summary>
        protected virtual void OnDisable()
            => myButton.onClick.RemoveListener(Click);

		#endregion Unity Messages

		public void AddListener(UnityAction action) => myButton.onClick.AddListener(action);

        public void RemoveListener(UnityAction action) => myButton.onClick.RemoveListener(action);

        /// <summary>
        /// Removes all listeners except my own.
        /// </summary>
        public void RemoveAllListeners()
        {
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(Click);
        }

        /// <summary>
        /// Function called by Button.
        /// </summary>
        [Button, DisableInEditorMode]
        public void Click() => OnPressedEvent?.Invoke(this); // FIXME - doesn't call myButton.OnClick events.

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

        public void Show(UnityAction onPress)
        {
            RemoveAllListeners();
            AddListener(onPress);
            Show();
        }

        public void Show(UnityAction onPress, Sprite sprite)
        {
            Sprite = sprite;
            Show(onPress);
        }

        public void Show(UnityAction onPress, string text)
        {
            Text = text;
            Show(onPress);
        }

        public void Show(UnityAction onPress, string text, Sprite sprite)
        {
            Sprite = sprite;
            Show(onPress, text);
        }

        public void Show(Sprite sprite)
        {
            Sprite = sprite;
            Show();
        }

        /// <returns>The <see cref="ObjectProperty"/> value cast to <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException"/>
        /// <seealso cref="SetPayload(object)"/>
        public T GetPayload<T>() => (T)ObjectProperty;

        /// <summary>
        /// Sets the <see cref="ObjectProperty"/>.
        /// </summary>
        /// <seealso cref="GetPayload{T}"/>
        public void SetPayload(object value) => ObjectProperty = value;

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

        public static RichUIButton operator +(RichUIButton button, UnityAction action)
        {
            button.AddListener(action);
            return button;
        }

        public static RichUIButton operator -(RichUIButton button, UnityAction action)
        {
            button.RemoveListener(action);
            return button;
        }
    }
}
