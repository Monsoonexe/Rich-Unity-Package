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

    [SerializeField, ShowIf(nameof(navigateOnShow))]
    private int startingNavButton = 0;

    [Title("Resources")]
    [SerializeField, Required]
    private Transform buttonHolder;

    [SerializeField, Required]
    private ANavigationProvider router;

    [SerializeField,
        CustomContextMenu(nameof(GatherNavButtons), nameof(GatherNavButtons))]
    protected RichUIButton[] navButtons;

    // runtime data
    /// <summary>
    /// A button that was previously selected.
    /// </summary>
    private RichUIButton selectedButton;

    #region Unity Messages

    protected override void Reset()
    {
        base.Reset();
        if (buttonHolder == null)
            buttonHolder = this.transform;
    }

    protected override void Awake()
    {
        base.Awake();
        if (buttonHolder == null)
            buttonHolder = this.transform;
    }

    #endregion Unity Messages

    #region AUIScreenController

    protected override void OnHide()
    {
        ResetButtons();
    }

    protected override void OnPropertiesSet()
    {
        InitButtons();

        // default to showing first button
        if (navigateOnShow)
        {
            OnNavigationButtonClicked(navButtons[startingNavButton]);
        }
    }

    #endregion AUIScreenController

    protected virtual void OnNavigationButtonClicked(RichUIButton clickedButton)
    {
        // handle button state
        Select(clickedButton);

        // open window
        router.NavigateTo(clickedButton.StringProperty);
    }

    #region Button Management

    private void GatherNavButtons()
    {
        navButtons = buttonHolder.GetComponentsInChildren<RichUIButton>();
    }

    /// <summary>
    /// Puts <paramref name="button"/> into selected state.
    /// </summary>
    private void Select(RichUIButton button)
    {
        // release existing button
        if (selectedButton != null)
        {
            selectedButton.Interactable = true;
        }

        selectedButton = button;
        selectedButton.Interactable = false;
        selectedButton.Button.Select();
    }

    private void InitButtons()
    {
        // create buttons
        int buttonCount = navButtons.Length;
        for (int i = 0; i < buttonCount; ++i)
        {
            RichUIButton button = navButtons[i];
            button.OnPressedEvent += OnNavigationButtonClicked;
            button.gameObject.SetActiveChecked(true);
        }
    }

    protected virtual void ResetButtons()
    {
        int buttonCount = navButtons.Length;
        for (int i = 0; i < buttonCount; ++i)
        {
            RichUIButton button = navButtons[i];
            button.OnPressedEvent -= OnNavigationButtonClicked;
        }
    }

    #endregion Button Management
}
