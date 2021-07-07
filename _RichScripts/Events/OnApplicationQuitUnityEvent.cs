
/// <summary>
/// Raises UnityEvent on Start(). Rig it in Inspector. 
/// </summary>
/// <remarks>Probably useless. Included for completeness.</remarks>
public sealed class OnApplicationQuitUnityEvent : AUnityLifetimeEvent
{
    private void OnApplicationQuit()
    {
        lifetimeEvent.Invoke();
    }
}
