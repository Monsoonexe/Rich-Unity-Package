using RichPackage.Events;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.TriggerVolumes
{
    /// <summary>
    /// Do something when something enters this volume.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class TriggerVolume2D : ATriggerVolume
    {
        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsTrigger))]
        public Collider2DUnityEvent onColliderEnterEvent;

        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsTrigger))]
        public Collider2DUnityEvent onColliderExitEvent;

        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsPhysical))]
        public Collision2DUnityEvent onCollisionEnterEvent;

        [FoldoutGroup(EVENT_GROUP),
            ShowIf(nameof(IsPhysical))]
        public Collision2DUnityEvent onCollisionExitEvent;

        private readonly EventHandlerList<TriggerVolume2D, Collider2D> onTriggerEnterHandlers = new EventHandlerList<TriggerVolume2D, Collider2D>();
        private readonly EventHandlerList<TriggerVolume2D, Collider2D> onTriggerExitHandlers = new EventHandlerList<TriggerVolume2D, Collider2D>();

        private readonly EventHandlerList<TriggerVolume2D, Collision2D> onCollisionEnterHandlers = new EventHandlerList<TriggerVolume2D, Collision2D>();
        private readonly EventHandlerList<TriggerVolume2D, Collision2D> onCollisionExitHandlers = new EventHandlerList<TriggerVolume2D, Collision2D>();

#pragma warning disable IDE1006 // Naming Styles

        public event Action<TriggerVolume2D, Collider2D> onTriggerEnter
        {
            add => onTriggerEnterHandlers.Add(value);
            remove => onTriggerExitHandlers.Add(value);
        }

        public event Action<TriggerVolume2D, Collider2D> onTriggerExit
        {
            add => onTriggerExitHandlers.Add(value);
            remove => onTriggerEnterHandlers.Remove(value);
        }

        public event Action<TriggerVolume2D, Collision2D> onCollisionEnter
        {
            add => onCollisionEnterHandlers.Add(value);
            remove => onCollisionEnterHandlers.Remove(value);
        }

        public event Action<TriggerVolume2D, Collision2D> onCollisionExit
        {
            add => onCollisionExitHandlers.Add(value);
            remove => onCollisionExitHandlers.Remove(value);
        }

#pragma warning restore IDE1006 // Naming Styles

        public Collider2D Collider => GetComponent<Collider2D>();
        public bool IsTrigger => Collider.isTrigger;
        public bool IsPhysical => !Collider.isTrigger;

        #region Unity Messages

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (ApplyCollisionFilter(collision.gameObject))
            {
                OnEnter.Invoke();
                onColliderEnterEvent.Invoke(collision);
                onTriggerEnterHandlers.Invoke(this, collision);
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (ApplyCollisionFilter(collision.gameObject))
            {
                OnExit.Invoke();
                onColliderExitEvent.Invoke(collision);
                onTriggerExitHandlers.Invoke(this, collision);
            }
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (ApplyCollisionFilter(collision.gameObject))
            {
                OnEnter.Invoke();
                onCollisionEnterEvent.Invoke(collision);
                onCollisionEnterHandlers.Invoke(this, collision);
            }
        }

        protected void OnCollisionExit2D(Collision2D collision)
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
