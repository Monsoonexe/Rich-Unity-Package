using RichPackage;
using System;
using System.Collections.Generic;

/// <summary>
/// Routes navigation requests to the correct screen with the proper properties.
/// </summary>
public abstract class ANavigationProvider : RichMonoBehaviour
{
    protected readonly Dictionary<string, Action> windowMapping
        = new Dictionary<string, Action>();

    public abstract void NavigateTo(string screenID);

    protected void MapWindow(string screenID, Action opener)
    {
        // TODO - error handling
        windowMapping.Add(screenID, opener);
    }

    protected void OpenWindow(string screenID)
    {
        // TODO - error handling
        windowMapping[screenID]();
    }
}
