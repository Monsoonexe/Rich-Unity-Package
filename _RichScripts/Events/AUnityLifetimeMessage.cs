using UnityEngine;
using UnityEngine.Events;

namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Exposes the Unity Lifetime Events to the Editor.
    /// </summary>
    /// <seealso cref="AwakeUnityMessage"/>
    /// <seealso cref="StartUnityMessage"/>
    /// <seealso cref="OnEnableUnityMessage"/>
    /// <seealso cref="OnDisableUnityMessage"/>
    /// <seealso cref="UpdateUnityMessage"/>
    /// <seealso cref="LateUpdateUnityMessage"/>
    /// <seealso cref="FixedUpdateUnityMessage"/>
    /// <seealso cref="FixedUpdateUnityMessage"/>
    /// <seealso cref="OnDestroyUnityMessage"/>
    /// <seealso cref="OnApplicationQuitUnityMessage"/>
    /// <seealso cref="OnApplicationPauseUnityMessage"/>
    /// <seealso cref="TriggerCollisionEvent"/>
    public abstract class AUnityLifetimeMessage : RichMonoBehaviour
    {
        [SerializeField]
        protected UnityEvent lifetimeEvent = new UnityEvent();
        public UnityEvent LifetimeEvent { get => lifetimeEvent; }
    }
}
