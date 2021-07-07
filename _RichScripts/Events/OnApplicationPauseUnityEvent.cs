using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Raises UnityEvent on OnApplicationPause(). Rig it in Inspector.
/// </summary>
/// <remarks>If I really need to, I'll make an event for a bool.</remarks>
public sealed class OnApplicationPauseUnityEvent : AUnityLifetimeEvent
{
    [SerializeField]
    private UnityEvent onApplicationUnpauseEvent = new UnityEvent();
    public UnityEvent OnApplicationUnpauseEvent { get => onApplicationUnpauseEvent; }

    private void OnApplicationPause(bool paused)
    {
        if (paused)
            lifetimeEvent.Invoke();
        else
            onApplicationUnpauseEvent.Invoke();
    }
}
