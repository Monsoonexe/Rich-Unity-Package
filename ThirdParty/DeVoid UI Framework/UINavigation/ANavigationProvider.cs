using RichPackage;

/// <summary>
/// Routes navigation requests to the correct screen with the proper properties.
/// </summary>
public abstract class ANavigationProvider : RichMonoBehaviour
{
    public abstract void NavigateTo(string screenID);
}

