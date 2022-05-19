using UnityEngine;
using UnityEngine.Events;
using ScriptableObjectArchitecture;

namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on OnApplicationPause(). Rig it in Inspector.
    /// </summary>
    /// <remarks>If I really need to, I'll make an event for a bool.</remarks>
    public sealed class OnApplicationPauseUnityMessage : AUnityLifetimeMessage
    {
        [SerializeField]
        private UnityEvent onApplicationUnpauseEvent = new UnityEvent();
        public UnityEvent OnApplicationUnpauseEvent { get => onApplicationUnpauseEvent; }
        
        [SerializeField, Tooltip("Raised after " + nameof(onApplicationUnpauseEvent))]
        private BoolUnityEvent onApplicationChangePauseStateEvent  = new BoolUnityEvent();
        public BoolUnityEvent OnApplicationChangePauseStateEvent { get => onApplicationChangePauseStateEvent; }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
                lifetimeEvent.Invoke();
            else
                onApplicationUnpauseEvent.Invoke();
            onApplicationChangePauseStateEvent.Invoke(paused);
        }
    }
}
