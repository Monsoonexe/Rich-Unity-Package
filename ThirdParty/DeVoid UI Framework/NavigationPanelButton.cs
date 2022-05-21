using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using RichPackage;

namespace ProjectEmpiresEdge.UI
{
    [RequireComponent(typeof(Button))]
    public class NavigationPanelButton : RichMonoBehaviour
    {
        [Header("---Navigation Panel Button---")]
        [SerializeField]
        private TextMeshProUGUI buttonLabel;

        [SerializeField]
        private Image icon;

        /// <summary>
        /// Event for when the UI button was clicked.
        /// </summary>
        public event Action<NavigationPanelButton> OnButtonClicked;

        /// <summary>
        /// Where to go and what to look like.
        /// </summary>
        private NavigationPanelEntry navigationData;

        private Button myButton; // button Component

        public bool IsInteractable
        {
            get => myButton.interactable;
            set => myButton.interactable = value;
        }

        public string TargetScreen { get => navigationData.TargetScreen; }

        protected override void Awake()
        {
            base.Awake();
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
