
/// <summary>
/// Raises UnityEvent on Start(). Rig it in Inspector.
/// </summary>
public sealed class StartUnityEvent : AUnityLifetimeEvent
{
    private void Start()
    {
        lifetimeEvent.Invoke();
    }
}
