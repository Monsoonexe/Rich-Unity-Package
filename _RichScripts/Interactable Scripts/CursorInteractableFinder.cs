using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.Interaction
{
    /// <summary>
    /// Finds <see cref="IInteractable"/>s by cursor (screen-based raycast)."/>
    /// </summary>
    /// <seealso cref="RaycastInteractableFinder"/>"/>
    /// <seealso cref="ProximityInteractableFinder"/>
    public class CursorInteractableFinder : RichMonoBehaviour
    {
        [Title("Dependencies")]
        public InteractionManager interactionManager;
        public Camera raycastOrigin = null;

        [Title("Settings")]
        public bool requireMatchingTag = false;

        [Tag, ShowIf(nameof(requireMatchingTag))]
        public string interactableTag = "Interactable";

        public QueryTriggerInteraction queryTriggerInteraction
            = QueryTriggerInteraction.Collide;

        public LayerMask raycastLayerMask = -1;

        [MinValue(0)]
        public float raycastLength = 100.0f;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            interactionManager = GetComponentInParent<InteractionManager>();
            raycastOrigin = Camera.main;
        }

        private void OnEnable()
        {
            interactionManager.OnInteractableChanged += OnInteractableChangedHandler;
        }

        private void OnDisable()
        {
            interactionManager.OnInteractableChanged -= OnInteractableChangedHandler;
        }

        #endregion Unity Messages

        private void OnInteractableChangedHandler(
            IInteractor interactor, IInteractable interactable)
        {
            if (interactable == null)
            {
                // lost our target, do a raycast immediately to find a new one
                ProcessRaycast();
            }
        }

        public void ProcessRaycast()
        {
            // check to see if player is looking at interactable object's model
            IInteractable interactable = null;
            Ray ray = raycastOrigin.ScreenPointToRay(Input.mousePosition);
            bool hit = Physics.Raycast(ray, out RaycastHit hitInfo,
                raycastLength, raycastLayerMask, queryTriggerInteraction);

            if (hit)
            {
                Collider collider = hitInfo.collider;
                if (!requireMatchingTag || collider.CompareTag(interactableTag))
                {
                    interactable = collider.GetComponent<IInteractable>();
                }
            }

            interactionManager.Target = interactable;
        }

        [Button]
        public void DrawRay()
        {
            Ray ray = raycastOrigin.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction, Color.red, raycastLength);
        }
    }
}