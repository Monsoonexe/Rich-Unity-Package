
/// <summary>
/// Base implementation for Window Screen Controllers that need no special Properties
/// </summary>
public abstract class AWindowController : AWindowController<WindowProperties>
{
    //basic
}

/// <summary>
/// Base implementation for Window ScreenControllers. Its param is a specific type of IWindowProperties.
/// </summary>
/// <typeparam name="TProps"></typeparam>
public abstract class AWindowController<TProps> 
    : AUIScreenController<TProps>, IWindowController
    where TProps : IWindowProperties
{
    public bool HideOnForegroundLost { get => Properties.HideOnForegroundLost; } // repeat

    public bool IsPopup { get => Properties.IsPopup; } // repeat

    public WindowPriorityENUM WindowPriority { get => Properties.WindowQueuePriority; }

    protected sealed override void SetProperties(TProps newProperties)
    {
        if (newProperties != null)
        {
            //if these new properties don't suppress the defaults, use the defaults instead.
            //TODO - double-check suppressing Properties logic.
            if (!newProperties.SuppressPrefabProperties)
            {
                newProperties.HideOnForegroundLost = Properties.HideOnForegroundLost;
                newProperties.WindowQueuePriority = Properties.WindowQueuePriority;
                newProperties.IsPopup = Properties.IsPopup;
            }

            Properties = newProperties;
        }
    }

    protected override void OnHierarchyFix()
    {
        transform.SetAsLastSibling(); // place last in list so it's drawn on top of everything else
    }

    /// <summary>
    /// Called when this Window becomes the CurrentWindow. Safe to open other Windows now.
    /// </summary>
    public virtual void OnWindowOpen()
    {
        //nada
    }

    /// <summary>
    /// Requests this Window to be closed, handy for rigging directly from Editor.
    /// UI_Prefix for ease-of-use, even though it breaks naming convention.
    /// </summary>
    /// <seealso cref="IWindowController.OnHide"/>
    public virtual void UI_Close()
    {
        CloseRequest(this, true);
    }

}
