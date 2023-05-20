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
        //[Header("---Settings---")]
        //public bool autoInteract;

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
        
        protected override void Awake()
        {
            base.Awake();
            RegisterWithManager();
        }

        protected virtual void OnDestroy()
            => UnregisterWithManager();

        public virtual void OnEnterHover()
            => enterHoverEvent.Invoke();

        public virtual void OnExitHover()
            => exitHoverEvent.Invoke();

        public virtual void OnEnterRange(IInteractor actor)
            => enterRangeEvent.Invoke();

        public virtual void OnExitRange(IInteractor actor)
            => exitRangeEvent.Invoke();

        public virtual void Activate(IInteractor actor)
        {
            interactEvent.Invoke();
            // actor.InteractWith(this);
        }

        public virtual void Release(IInteractor actor)
            => endInteractEvent.Invoke();

        public bool IsAvailable { get => IsEnabled; set => IsEnabled = value; }

        public bool IsEnabled { get => enabled; set => enabled = value; }

        protected void RegisterWithManager()
            => InteractionManager.RegisterInteractable(this);

        protected void UnregisterWithManager()
            => InteractionManager.UnregisterInteractable(this);
    }
}
