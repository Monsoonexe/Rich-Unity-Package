using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Exposes the Unity Lifetime Events to the Editor.
/// </summary>
/// <seealso cref="AwakeUnityEvent"/>
/// <seealso cref="StartUnityEvent"/>
/// <seealso cref="OnEnableUnityEvent"/>
/// <seealso cref="OnDisableUnityEvent"/>
/// <seealso cref="UpdateUnityEvent"/>
/// <seealso cref="LateUpdateUnityEvent"/>
/// <seealso cref="FixedUpdateUnityEvent"/>
/// <seealso cref="FixedUpdateUnityEvent"/>
/// <seealso cref="OnDestroyUnityEvent"/>
/// <seealso cref="OnApplicationQuitUnityEvent"/>
/// <seealso cref="OnApplicationPauseUnityEvent"/>
/// <seealso cref="TriggerCollider"/>
public abstract class AUnityLifetimeEvent : RichMonoBehaviour
{
    [SerializeField]
    protected UnityEvent lifetimeEvent = new UnityEvent();
    public UnityEvent LifetimeEvent { get => lifetimeEvent; }
}
