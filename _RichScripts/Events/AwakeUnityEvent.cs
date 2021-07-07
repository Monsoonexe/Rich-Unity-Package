
/// <summary>
/// Raises UnityEvent on Awake(). Rig it in Inspector.
/// </summary>
public sealed class AwakeUnityEvent : AUnityLifetimeEvent
{
    protected override void Awake()
    {
        base.Awake();
        lifetimeEvent.Invoke();
    }
}
