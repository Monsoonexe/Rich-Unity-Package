using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectEmpiresEdge.UI
{
    [Serializable]
    public class NavigationPanelController : APanelController
    {
        [Header("---Navigation Data---")]
        [SerializeField]
        protected string owningWindow;

        [SerializeField]
        protected bool navigateOnShow = false;

        [SerializeField]
        protected NavigationPanelButton navButtonPrefab;

        [SerializeField]
        protected NavigationPanelEntry[] navTargets;

        protected readonly List<NavigationPanelButton> currentButtons = new List<NavigationPanelButton>(8);
        
        //protected override void AddListeners()
        //{
        //    - hook into a global event or signal
        //}

        //protected override void RemoveListeners()
        //{
        //    - hook into a global event or signal
        //}

        protected override void OnHide()
        {
            ClearButtons(); // free up some memory by destroying button
        }

        protected override void OnPropertiesSet()
        {
            ClearButtons(); // remove existing buttons
            var buttonCount = navTargets.Length;

            //create buttons
            for(var i = 0; i < buttonCount; ++i)
            {
                var target = navTargets[i];
                var newButton = Instantiate(navButtonPrefab); // give birth

                newButton.transform.SetParent(this.transform, false); // reparent to this object
                newButton.SetData(target); // set nav target and visuals

                if(!newButton.isActiveAndEnabled)//make sure it's enabled
                    newButton.gameObject.SetActive(true);

                newButton.OnButtonClicked += OnNavigationButtonClicked; // hook to event
                currentButtons.Add(newButton);
            }

            //default to showing first button
            if (navigateOnShow)
            {
                //GalaxyMapUI.UIFrame.OpenWindow(currentButtons[0].TargetScreen); // navigate
                currentButtons[0].SetCurrentNavigationTarget(currentButtons[0].TargetScreen); // set button inactive
            }
        }

        protected virtual void OnNavigationButtonClicked(NavigationPanelButton clickedButton)
        {
            //GalaxyMapUI.UIFrame.CloseCurrentWindow();
            //GalaxyMapUI.UIFrame.OpenWindow(clickedButton.TargetScreen);

            //dis/enable button interactability
            var buttonCount = currentButtons.Count;
            for (var i = 0; i < buttonCount; ++i)
            {
                currentButtons[i].SetCurrentNavigationTarget(clickedButton);
            }
        }

        /// <summary>
        /// Used if navigation happens outside of this Controller
        /// </summary>
        /// <param name="screenID"></param>
        protected virtual void OnExternalNavigation(string screenID)
        {
            //TODO - hook into a global event or signal
            //dis/enable button interactability
            var buttonCount = currentButtons.Count;
            for(var i = 0; i < buttonCount; ++i)
            {
                currentButtons[i].SetCurrentNavigationTarget(screenID);
            }
        }

        /// <summary>
        /// Destroy Button Objects
        /// </summary>
        protected virtual void ClearButtons()
        {
            var buttonCount = currentButtons.Count;

            for(var i = 0; i < buttonCount; ++i)
            {
                var button = currentButtons[i];

                button.OnButtonClicked -= OnNavigationButtonClicked;
                Destroy(button.gameObject);
                //TODO - pool
            }

            currentButtons.Clear();
        }

        public virtual void UI_CloseWindow()
        {
            Hide(); // don't need to go through the frame to hide a panel
            //GalaxyMapUI.UIFrame.CloseCurrentWindow();
            //GalaxyMapUI.UIFrame.CloseWindow(owningWindow);
        }
    }
}

