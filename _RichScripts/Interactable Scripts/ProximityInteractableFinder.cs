using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace RichPackage.Interaction
{
    /// <summary>
    /// Finds <see cref="Interactable"/>s by proximity (physics system).
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ProximityInteractableFinder : RichMonoBehaviour
    {
        [Title("Dependencies")]
        [Required]
        public Collider myCollider;

        [Required]
        public InteractionManager interactionManager;

        [Title("Settings")]
        public bool debug = false;
        public bool requireMatchingTag = false;

        [Tag, ShowIf(nameof(requireMatchingTag))]
        public string interactableTag = "Interactable";

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            if (!myCollider)
                myCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            interactionManager.OnInteractableChanged += OnInteractableChanged;
        }

        private void OnDisable()
        {
            interactionManager.OnInteractableChanged -= OnInteractableChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            // NOTE: this code runs even when disabled
            // bail if tag doesn't match
            if (requireMatchingTag && !other.gameObject.CompareTag(interactableTag))
                return;

            // this might be an interactable. Should we target it?
            if (other.TryGetComponent(out IInteractable newInteractable)) // if encountered an IInteractable
            {
                // this is a new thing
                interactionManager.Add(newInteractable);
                // could add extra IProximalInteractable logic here if desired
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // NOTE: this code runs even when disabled
            // bail if tag doesn't match
            if (requireMatchingTag && !other.gameObject.CompareTag(interactableTag))
                return;

            // this might be an interactable
            if (other.TryGetComponent(out IInteractable newInteractable)) // if encountered an IInteractable
            {
                interactionManager.Remove(newInteractable);
                // could add extra IProximalInteractable logic here if desired
            }
        }

        #endregion Unity Messages

        private void OnInteractableChanged(IInteractor interactor,
            IInteractable interactable)
        {
            // if we lost our target, find a new one
            if (interactionManager.Target == null)
                FocusClosestInteractable();
        }

        /// <summary>
        /// Focuses the closest known interactable.
        /// </summary>
        public void FocusClosestInteractable()
        {
            // select something new if there are still valid interactables close by
            int enabledInteractablesCount; // lazy-fetch-cache
            if (interactionManager.KnownInteractables.Count == 0
                || (enabledInteractablesCount = interactionManager.EnumerateEnabledInteractables()
                .Count()) == 0)
            {
                return;
            }

            // we lost our target, but are within range of 1[+] interactables.
            // which should we take?
            IInteractable nextInteractable;
            // if there is only 1, take that one
            if (enabledInteractablesCount == 1)
            {
                nextInteractable = interactionManager.EnumerateEnabledInteractables().First().Interactable;
            }
            else
            {
                // there are many interactables close by -- favor the closest one
                nextInteractable = interactionManager.GetClosestEnabledInteractable();
            }

            interactionManager.Target = nextInteractable;
        }
    }
}
