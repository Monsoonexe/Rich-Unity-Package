using RichPackage;
using RichPackage.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace ProjectEmpiresEdge.UI
{
    [Serializable]
    public class NavigationPanelController : APanelController
    {
        [Title("Settings")]
        [SerializeField]
        protected bool navigateOnShow = false;

        [Title("Resources")]
        [SerializeField]
        private Transform buttonHolder;

        // runtime data
        [SerializeField,
            CustomContextMenu(nameof(GatherNavButtons), nameof(GatherNavButtons))]
        protected RichUIButton[] navButtons;

        // runtime data
        private RichUIButton previousButton;

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            if (buttonHolder == null)
                buttonHolder = this.transform;
        }
        
        #endregion Unity Messages

        protected override void OnHide()
        {
            ClearButtons(); // free up some memory by destroying button
        }

        protected override void OnPropertiesSet()
        {
            ClearButtons(); // remove existing buttons
            int buttonCount = navButtons.Length;

            // create buttons
            for(int i = 0; i < buttonCount; ++i)
            {
                var button = navButtons[i];
                button.gameObject.SetActiveChecked(true);
                button.OnPressedEvent += OnNavigationButtonClicked; // hook to event
            }

            // default to showing first button
            if (navigateOnShow)
            {
                OnNavigationButtonClicked(navButtons[0]);
            }
        }

        protected virtual void OnNavigationButtonClicked(RichUIButton clickedButton)
        {
            // track button state
            if (previousButton != null)
            {
                previousButton.Interactable = true;
            }
            previousButton = clickedButton;
            clickedButton.Interactable = false;
            clickedButton.Button.Select();

            // open window
            GalaxyMapUI.UIFrame.CloseCurrentWindow();
            GalaxyMapUI.UIFrame.OpenWindow(clickedButton.StringProperty);
        }

        /// <summary>
        /// Destroy Button Objects
        /// </summary>
        protected virtual void ClearButtons()
        {
            var buttonCount = navButtons.Length;

            for(var i = 0; i < buttonCount; ++i)
            {
                var button = navButtons[i];

                button.OnPressedEvent -= OnNavigationButtonClicked;
                Destroy(button.gameObject); //TODO - pool
            }
        }

        public virtual void UI_CloseWindow()
        {
            Hide(); // don't need to go through the frame to hide a panel
            GalaxyMapUI.UIFrame.CloseCurrentWindow();
        }

        private void GatherNavButtons()
        {
            navButtons = buttonHolder.GetComponentsInChildren<RichUIButton>();
        }
    }
}

