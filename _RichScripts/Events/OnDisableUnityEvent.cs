
/// <summary>
/// Raises UnityEvent on OnDisable(). Rig it in Inspector.
/// </summary>
public sealed class OnDisableUnityEvent : AUnityLifetimeEvent
{
    private void OnDisable()
    {
        lifetimeEvent.Invoke();
    }
}
