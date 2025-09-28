using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace RichPackage.Interaction
{
    /// <summary>
    /// An IPlayerInteractable that comes with built-in behaviour and events already rigged.
    /// </summary>
    /// <remarks>Must be tagged "Interactable"</remarks>
    [SelectionBase]
    [RequireComponent(typeof(Collider))]
    public class Interactable : RichMonoBehaviour, IInteractable
    {
        [Foldout("---Events---")]
        [SerializeField]
        protected UnityEvent enterRangeEvent = new UnityEvent();

        [Foldout("---Events---")]
        [SerializeField]
        protected UnityEvent exitRangeEvent = new UnityEvent();

        [Foldout("---Events---")]
        [SerializeField]
        protected UnityEvent enterHoverEvent = new UnityEvent();

        [Foldout("---Events---")]
        [SerializeField]
        protected UnityEvent exitHoverEvent = new UnityEvent();

        [Foldout("---Events---")]
        [SerializeField]
        protected UnityEvent interactEvent = new UnityEvent();

        [Foldout("---Events---")]
        [SerializeField]
        protected UnityEvent endInteractEvent = new UnityEvent();
        
        protected virtual void OnEnable()
            => RegisterWithManager();

        protected virtual void OnDisable()
            => UnregisterWithManager();

        public virtual void OnTakeFocus(IInteractor actor)
            => enterHoverEvent.Invoke();

        public virtual void OnLoseFocus(IInteractor actor)
            => exitHoverEvent.Invoke();

        public virtual void OnEnterRange(IInteractor actor)
            => enterRangeEvent.Invoke();

        public virtual void OnExitRange(IInteractor actor)
            => exitRangeEvent.Invoke();

        public virtual void InteractWith(IInteractor actor)
        {
            actor.InteractWith(this);
            interactEvent.Invoke();
        }

        public virtual void Release(IInteractor actor)
            => endInteractEvent.Invoke();

        public bool IsAvailable { get => IsEnabled; set => IsEnabled = value; }

        public bool IsEnabled { get => enabled; set => enabled = value; }
        public virtual Transform InteractionPoint { get => transform; }

        protected void RegisterWithManager()
        {
            //=> InteractionManager.RegisterInteractable(this);
        }

        protected void UnregisterWithManager()
        {
            //=> InteractionManager.UnregisterInteractable(this);
        }
    }
}
