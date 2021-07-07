
/// <summary>
/// Raises UnityEvent on OnEnable(). Rig it in Inspector.
/// </summary>
public sealed class OnEnableUnityEvent : AUnityLifetimeEvent
{
    private void OnEnable()
    {
        lifetimeEvent.Invoke();
    }
}

