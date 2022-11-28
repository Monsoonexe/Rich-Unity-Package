using RichPackage;
using RichPackage.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class NavigationPanelController : APanelController
{
    [Title("Settings")]
    [SerializeField]
    protected bool navigateOnShow = false;

    [Title("Resources")]
    [SerializeField]
    private Transform buttonHolder;

    [SerializeField, Required]
    private ANavigationProvider router;

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
        ResetButtons(); // free up some memory by destroying button
    }

    protected override void OnPropertiesSet()
    {
        InitButtons();

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

        // set button state
        clickedButton.Interactable = false;
        clickedButton.Button.Select();

        // open window
        router.NavigateTo(clickedButton.StringProperty);
    }

    #region Button Management

    private void GatherNavButtons()
    {
        navButtons = buttonHolder.GetComponentsInChildren<RichUIButton>();
    }

    private void InitButtons()
    {
        // create buttons
        int buttonCount = navButtons.Length;
        for (int i = 0; i < buttonCount; ++i)
        {
            var button = navButtons[i];
            button.OnPressedEvent += OnNavigationButtonClicked;
            button.gameObject.SetActiveChecked(true);
        }
    }

    /// <summary>
    /// Destroy Button Objects
    /// </summary>
    protected virtual void ResetButtons()
    {
        var buttonCount = navButtons.Length;
        for (var i = 0; i < buttonCount; ++i)
        {
            var button = navButtons[i];
            button.OnPressedEvent -= OnNavigationButtonClicked;
            Destroy(button.gameObject);
        }
    }
        
    #endregion Button Management
}
