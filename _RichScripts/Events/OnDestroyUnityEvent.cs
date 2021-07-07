
/// <summary>
/// Raises UnityEvent on OnDestroy(). Rig it in Inspector.
/// </summary>
public sealed class OnDestroyUnityEvent : AUnityLifetimeEvent
{
    private void OnDestroy()
    {
        lifetimeEvent.Invoke();
    }
}
