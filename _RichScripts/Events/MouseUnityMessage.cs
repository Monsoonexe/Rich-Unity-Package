using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvents in response to Mouse Events. 
    /// Rig in the Inspector.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class MouseUnityMessage : RichMonoBehaviour
    {
        [Title("---Scene Refs---")]
        [Tooltip("[Optional] Assumes self if null.")]
        [SerializeField]
        private Collider myCollider;
        
        [FoldoutGroup("---Events---")]
        public UnityEvent onMouseDownEvent = new UnityEvent();

        [FoldoutGroup("---Events---")]
        public UnityEvent onMouseUpAsButtonEvent = new UnityEvent();

        [FoldoutGroup("---Events---")]
        public UnityEvent onMouseEnterEvent = new UnityEvent();

        [FoldoutGroup("---Events---")]
        public UnityEvent onMouseExitEvent = new UnityEvent();

        [ShowInInspector]
        public bool IsInteractable
        {
            get => myCollider.enabled;
            set => myCollider.enabled = value;
        }

        protected override void Awake()
        {
            base.Awake();
            if(myCollider == null)
                myCollider = GetComponent<Collider>();
        }

        private void Reset()
        {
            myCollider = GetComponent<Collider>();
        }

        public void OnMouseDown()
            => onMouseDownEvent.Invoke();

        public void OnMouseUpAsButton()
            => onMouseUpAsButtonEvent.Invoke();

        public void OnMouseEnter()
            => onMouseEnterEvent.Invoke();

        public void OnMouseExit()
            => onMouseExitEvent.Invoke();

        [Button]
        public void ToggleInteractable() => IsInteractable = !IsInteractable;
    }
}
