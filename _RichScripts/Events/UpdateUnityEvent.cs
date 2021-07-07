
/// <summary>
/// Raises UnityEvent on Update(). Rig it in Inspector.
/// </summary>
public sealed class UpdateUnityEvent : AUnityLifetimeEvent
{
    private void Update()
    {
        lifetimeEvent.Invoke();
    }
}
