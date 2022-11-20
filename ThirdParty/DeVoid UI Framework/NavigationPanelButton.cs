using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RichPackage;
using Sirenix.OdinInspector;

namespace ProjectEmpiresEdge.UI
{
    [RequireComponent(typeof(Button)), Obsolete]
    public class NavigationPanelButton : RichMonoBehaviour
    {
        [Title("References")]
        [SerializeField, Required]
        private TextMeshProUGUI buttonLabel;

        [SerializeField, Required]
        private Image icon;

        [SerializeField, Required]
        private Button myButton; // button Component

        /// <summary>
        /// Event for when the UI button was clicked.
        /// </summary>
        public event Action<NavigationPanelButton> OnButtonClicked;

        /// <summary>
        /// Where to go and what to look like.
        /// </summary>
        private NavigationPanelEntry navigationData;

        public bool IsInteractable
        {
            get => myButton.interactable;
            set => myButton.interactable = value;
        }

        public string TargetScreen { get => navigationData.TargetScreen; }

		protected override void Reset()
		{
            base.Reset();
            SetDevDescription("A tab to navigate a menu.");
            myButton = gameObject.GetComponent<Button>();
		}

		protected override void Awake()
        {
            base.Awake();
            if (myButton == null)
                myButton = GetComponent<Button>();
        }

        /// <summary>
        /// Setter. Basically constructor
        /// </summary>
        /// <param name="navData"></param>
        public void SetData(NavigationPanelEntry navData)
        {
            navigationData = navData; // set data

            //update visuals
            buttonLabel.text = navigationData.ButtonText;

            if (navigationData.Sprite)
            {
                icon.enabled = true;
                icon.sprite = navigationData.Sprite;
            }
            else
            {
                icon.enabled = false;
            }
        }

        /// <summary>
        /// Dis/enable self if this buttton is the selected button.
        /// </summary>
        /// <param name="selectedButton"></param>
        public void SetCurrentNavigationTarget(NavigationPanelButton selectedButton)
        {
            myButton.interactable = selectedButton != this;
        }

        /// <summary>
        /// Dis/enable self if this buttton is the selected button.
        /// </summary>
        /// <param name="selectedButton"></param>
        public void SetCurrentNavigationTarget(string screenID)
        {
            myButton.interactable = screenID != navigationData.TargetScreen;
        }

        /// <summary>
        /// Hook this up to a button!
        /// </summary>
        public void UI_OnClick()
        {
            OnButtonClicked?.Invoke(this);
        }
    }
}
