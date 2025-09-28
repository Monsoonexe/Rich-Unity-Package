using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.Interaction
{
    /// <remarks>Useful for first-person games where the interaction ray is from a fixed origin.</remarks>
    public class RaycastInteractableFinder : RichMonoBehaviour
    {
        [Title("Dependencies")]
        public InteractionManager interactionManager;
        public Transform raycastOrigin = null;

        [Title("Settings")]
        public bool requireMatchingTag = false;

        [Tag, ShowIf(nameof(requireMatchingTag))]
        public string interactableTag = "Interactable";

        public QueryTriggerInteraction queryTriggerInteraction
            = QueryTriggerInteraction.Ignore;

        public LayerMask raycastLayerMask = -1;

        [MinValue(0)]
        public float raycastLength = 10.0f;

        [Tooltip("[Modifying has no effect in PlayMode]\r\n"
            + "Seconds between each raycast query for an IInteractable.\r\n"
            + "Lower is more responsive but costly.")]
        [MinValue(0)]
        public float raycastInterval = 0.2f;

        // runtime
        private Timer myRaycastTimer;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            raycastOrigin = transform;
            interactionManager = GetComponentInParent<InteractionManager>();
        }

        protected override void Awake()
        {
            base.Awake();
            myRaycastTimer = gameObject.AddComponent<Timer>(); //this timer isn't for mortal eyes 0.o
            myRaycastTimer.Initialize(raycastInterval, ProcessRaycast, loop: true);
        }

        private void OnEnable()
        {
            myRaycastTimer.Restart();
            interactionManager.OnInteractableChanged += OnInteractableChangedHandler;
        }

        private void OnDisable()
        {
            myRaycastTimer.Stop();
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
            Ray ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
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
            Debug.DrawRay(raycastOrigin.position,
                raycastOrigin.forward, Color.red, 2);
        }
    }
}
