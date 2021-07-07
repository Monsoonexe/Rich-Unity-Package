
/// <summary>
/// Raises UnityEvent on LateUpdate(). Rig it in Inspector.
/// </summary>
public sealed class LateUpdateUnityEvent : AUnityLifetimeEvent
{
    private void LateUpdate()
    {
        lifetimeEvent.Invoke();
    }
}
