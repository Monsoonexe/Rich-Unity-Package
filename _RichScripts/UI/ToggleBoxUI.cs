﻿using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;

namespace RichPackage.UI
{
    /// <summary>
    /// Displays things that have to do with toggling and switches.
    /// </summary>
    [SelectionBase]
    public class ToggleBoxUI : VariableText
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

        [Header("---Game Events---")]
        [SerializeField]
        private BoolUnityEvent toggleEvent;
        
        protected override void Awake()
        {
            base.Awake();
            //subscribe to events
            toggleButton.onClick.AddListener(Toggle);
        }

        private void OnDestroy()
        {   //unsubscribe
            toggleButton.onClick.RemoveListener(Toggle);
        }

        public void Toggle()
        {
            SetState(!ToggleValue);
        }

        public override void UpdateUI()
        {
            SetState(ToggleValue); // init starting state
        }

        public void SetState(bool togg)
        {
            ToggleValue = togg;
            if(togg)
            {
                toggleImage.sprite = onSprite;
                toggleImage.color = onColor;
                tmp.text = onString;
            }
            else
            {
                toggleImage.sprite = offSprite;
                toggleImage.color = offColor;
                tmp.text = offString;
            }
            toggleEvent.Invoke(togg);
        }
        
    }
}
