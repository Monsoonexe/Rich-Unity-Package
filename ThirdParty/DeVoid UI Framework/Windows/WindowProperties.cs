using System;
using UnityEngine;

/// <summary>
/// The payload that gets passed to a <see cref="AWindowController"/> with 
/// all the data required to populate its fields.
/// </summary>
/// <seealso cref="PanelProperties"/>
[Serializable]
public partial class WindowProperties : IWindowProperties
{
    [Tooltip("Should this Window be hidden when another Window takes its foreground?")]
    [SerializeField]
    private bool hideOnForegroundLost = true;

    /// <summary>
    /// Should this Window be hidden when another Window takes its foreground?
    /// </summary>
    public bool HideOnForegroundLost
    { get => hideOnForegroundLost; set => hideOnForegroundLost = value; }

    [Tooltip("How should this window behave in case another window is already opened?")]
    [SerializeField]
    protected WindowPriorityENUM windowQueuePriority = 0;

    /// <summary>
    /// How should this window behave in case another window is already opened?
    /// </summary>
    public WindowPriorityENUM WindowQueuePriority
    { get => windowQueuePriority; set => windowQueuePriority = value; }

    [Tooltip("Popups are displayed in front of all other Windows.")]
    [SerializeField]
    protected bool isPopup = false;

    /// <summary>
    /// Popups are displayed in front of all other Windows.
    /// </summary>
    public bool IsPopup
    { get => isPopup; set => isPopup = value; }

    /// <summary>
    /// When Properties are passed through Open(), should it override the prefab Properties?
    /// </summary>
    public bool SuppressPrefabProperties { get; set; }

    #region Constructors

    public WindowProperties(
        WindowPriorityENUM windowQueuePriority = WindowPriorityENUM.ForceForeground,
        bool hideOnForegroundLost = true, 
        bool isPopup = false,
        bool suppressPrefabProperties = false)
    {
        this.hideOnForegroundLost = hideOnForegroundLost;
        this.windowQueuePriority = windowQueuePriority;
        this.isPopup = isPopup;
        this.SuppressPrefabProperties = suppressPrefabProperties;
    }

    #endregion

}
