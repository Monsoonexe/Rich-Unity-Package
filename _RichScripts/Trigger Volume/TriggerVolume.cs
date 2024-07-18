using RichPackage.Events;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.TriggerVolumes
{
    /// <summary>
    /// Do something when something enters this volume.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerVolume : ATriggerVolume
    {
        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsTrigger))]
        public ColliderUnityEvent onColliderEnterEvent;

        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsTrigger))]
        public ColliderUnityEvent onColliderExitEvent;

        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsPhysical))]
        public CollisionUnityEvent onCollisionEnterEvent;

        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsPhysical))]
        public CollisionUnityEvent onCollisionExitEvent;

        private readonly EventHandlerList<TriggerVolume, Collider> onTriggerEnterHandlers = new();
        private readonly EventHandlerList<TriggerVolume, Collider> onTriggerExitHandlers = new();

        private readonly EventHandlerList<TriggerVolume, Collision> onCollisionEnterHandlers = new();
        private readonly EventHandlerList<TriggerVolume, Collision> onCollisionExitHandlers = new();

#pragma warning disable IDE1006 // Naming Styles

        public event Action<TriggerVolume, Collider> onTriggerEnter
        {
            add => onTriggerEnterHandlers.Add(value);
            remove => onTriggerEnterHandlers.Remove(value);
        }

        public event Action<TriggerVolume, Collider> onTriggerExit
        {
            add => onTriggerExitHandlers.Add(value);
            remove => onTriggerExitHandlers.Remove(value);
        }

        public event Action<TriggerVolume, Collision> onCollisionEnter
        {
            add => onCollisionEnterHandlers.Add(value);
            remove => onCollisionEnterHandlers.Remove(value);
        }

        public event Action<TriggerVolume, Collision> onCollisionExit
        {
            add => onCollisionExitHandlers.Add(value);
            remove => onCollisionExitHandlers.Remove(value);
        }

#pragma warning restore IDE1006 // Naming Styles

        public Collider Collider => GetComponent<Collider>();
        public bool IsTrigger => Collider.isTrigger;
        public bool IsPhysical => !Collider.isTrigger;

        #region Unity Messages

        protected void OnTriggerEnter(Collider other)
        {
            if (ApplyCollisionFilter(other.gameObject))
            {
                OnEnter.Invoke();
                onColliderEnterEvent.Invoke(other);
                onTriggerEnterHandlers.Invoke(this, other);
            }
        }

        protected void OnTriggerExit(Collider other)
        {
            if (ApplyCollisionFilter(other.gameObject))
            {
                OnExit.Invoke();
                onColliderExitEvent.Invoke(other);
                onTriggerExitHandlers.Invoke(this, other);
            }
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (ApplyCollisionFilter(collision.gameObject))
            {
                OnEnter.Invoke();
                onCollisionEnterEvent.Invoke(collision);
                onCollisionEnterHandlers.Invoke(this, collision);
            }
        }

        protected void OnCollisionExit(Collision collision)
        {
            if (ApplyCollisionFilter(collision.gameObject))
            {
                OnExit.Invoke();
                onCollisionExitEvent.Invoke(collision);
                onCollisionExitHandlers.Invoke(this, collision);
            }
        }

        #endregion Unity Messages
    }
}
