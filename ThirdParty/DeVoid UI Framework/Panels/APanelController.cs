using UnityEngine;

/// <summary>
/// Base class for Panels with no special Properties
/// </summary>
public abstract class APanelController : APanelController<PanelProperties>
{
    //exists
}

/// <summary>
/// Base class for Panels
/// </summary>
/// <typeparam name="TProps"></typeparam>
public abstract class APanelController<TProps> : AUIScreenController<TProps>,
    IPanelController where TProps : IPanelProperties
{
    public EPanelPriority Priority
    {
        get => Properties?.Priority ?? EPanelPriority.None;
    }

    /// <summary>
    /// Not sure why this is the case.
    /// </summary>
    /// <param name="props"></param>
    protected sealed override void SetProperties(TProps props)
    {
        base.SetProperties(props);
    }
}
