using UnityEngine;
using UnityEngine.Events;

namespace Explore
{
    /// <summary>
    /// A very, very basic Interactable to quickly rig Scene objects. To make your own:
    /// Trigger Collider, tag: "Interactable", implement IPlayerInteractable interface.
    /// For extra functionality, inherit from AInteractable, but not necessary.
    /// The UnityEvents are optional -- just used in this class.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public sealed class GenericInteractable : AInteractable, IPlayerInteractable
    {
        [Header("---Interactable---")]
        [SerializeField]
        private UnityEvent enterEvent = new UnityEvent();

        [SerializeField]
        private UnityEvent exitEvent = new UnityEvent();

        public override void OnEnterRange(Player context)
        {
            base.OnEnterRange(context);
            enterEvent.Invoke();
        }

        public override void OnExitRange(Player context)
        {
            base.OnExitRange(context);
            exitEvent.Invoke();
        }
    }

}
