
/// <summary>
/// Raises UnityEvent on FixedUpdate(). Rig it in Inspector.
/// </summary>
public sealed class FixedUpdateUnityEvent : AUnityLifetimeEvent
{
    private void FixedUpdate()
    {
        lifetimeEvent.Invoke();
    }
}
