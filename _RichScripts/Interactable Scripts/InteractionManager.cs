using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace RichPackage.Interaction
{
    /// <summary>
    /// Handles triggering Interactions between a Player and the world.
    /// </summary>
    /// <remarks>
    /// IsKinematic Rigidbodies sometimes call OnTriggerEnter/Exit at unexpected times, 
    /// or repeatedly
    /// 
    /// If you need more information, either follow the same pattern with a new IIInteractable,
    /// or get it from the 'context': player.Inventory[0].Item or something.
    /// </remarks>
    /// <seealso cref="Interactable"/>
    [RequireComponent(typeof(Collider))]
    public sealed class InteractionManager : RichMonoBehaviour
    {
        private static readonly List<IInteractable> interactableList = new List<IInteractable>(12);

        [Title("---Settings---")]
        public bool requireMatchingTag = true;

        [Tag, ShowIf(nameof(requireMatchingTag))]
        public string interactableTag = "Interactable";

        public bool allowProximityInteractions = true;

        [ShowIf(nameof(allowProximityInteractions))]
        [Tooltip("This component needs a Rigidbody on the same GameObject" +
            "in order to detect proximity.")]
        [Required, SerializeField]
        private Rigidbody myRigidbody;

        [Header("---Input---")]
        public bool useKeyCode = false;

        [ShowIf(nameof(useKeyCode))]
        public KeyCode interactKeyCode = KeyCode.Space;

        public bool useButton = true;

        [ShowIf(nameof(useButton))]
        [NaughtyAttributes.InputAxis]
        public string interactButton = "Fire1";

        [BoxGroup("---Raycast Settings---")]
        public bool allowRaycastInteractions = true;

        [BoxGroup("---Raycast Settings---")]
        [ShowIf(nameof(allowRaycastInteractions))]
        public QueryTriggerInteraction queryTriggerInteraction
            = QueryTriggerInteraction.Ignore;

        [BoxGroup("---Raycast Settings---")]
        [ShowIf(nameof(allowRaycastInteractions))]
        public LayerMask raycastLayerMask = -1;

        [MinValue(0)]
        [BoxGroup("---Raycast Settings---")]
        [ShowIf(nameof(allowRaycastInteractions))]
        public float raycastLength = 10.0f;

        [Tooltip("[Modifying has no effect in PlayMode]\r\n"
            + "Seconds between each raycast query for an IInteractable.\r\n"
            + "Lower is more responsive but costly.")]
        [MinValue(0)]
        [BoxGroup("---Raycast Settings---")]
        [ShowIf(nameof(allowRaycastInteractions))]
        public float raycastInterval = 0.25f;

        [BoxGroup("---Raycast Settings---")]
        [ShowIf(nameof(allowRaycastInteractions))]
        [Required]
        public Transform raycastOrigin = null;

        [FoldoutGroup("---Events---")]
        [SerializeField]
        private UnityEvent interactEvent = new UnityEvent();
        public UnityEvent OnInteractEvent { get => interactEvent; }

        [FoldoutGroup("---Events---")]
        [SerializeField]
        private UnityEvent enterRangeEvent = new UnityEvent();
        public UnityEvent OnEnterEvent { get => enterRangeEvent; }

        [FoldoutGroup("---Events---")]
        [SerializeField]
        private UnityEvent exitRangeEvent = new UnityEvent();
        public UnityEvent OnExitEvent { get => exitRangeEvent; }

        [FoldoutGroup("---Events---")]
        [SerializeField]
        private UnityEvent enterHoverEvent = new UnityEvent();
        public UnityEvent OnEnterHoverEvent { get => enterHoverEvent; }

        [FoldoutGroup("---Events---")]
        [SerializeField]
        private UnityEvent exitHoverEvent = new UnityEvent();
        public UnityEvent OnExitHoverEvent { get => exitHoverEvent; }

        // member components
        private Collider myCollider;
        private Timer myRaycastTimer;

        // runtime data
        public IInteractor actor;
        private IInteractable proximityIInteractable;
        private IInteractable raycastInteractable; //has higher priority

        /// <summary>
        /// Can externally request an Interact to occur.
        /// </summary>
        private bool interactRequested = false;

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();

            //gather member component references
            myRigidbody = GetComponent<Rigidbody>();
            myCollider = GetComponent<Collider>();
            myRaycastTimer = gameObject.AddComponent<Timer>(); //this timer isn't for mortal eyes 0.o

            //configure timer to raycast on repeat
            myRaycastTimer.Initialize(raycastInterval, ProcessRaycast, loop: true);

            //configure rigidbody (only needs to do this if the RB is separate from player RB)
            // myRigidbody.isKinematic = true;
            // myRigidbody.useGravity = false;
            // myRigidbody.angularDrag = 0;
            // myRigidbody.drag = 0;
            // myRigidbody.mass = Mathf.Epsilon;//can't actually be zero.

            //validate settings
            Debug.Assert(useButton || useKeyCode,
                "[InteractionManager] useButton and useKeyCode both false! "
                + "How to interact???", this);

            // Signals.Get<OnPauseGameSignal>().AddListener(PauseGameHandler);
            // Signals.Get<TogglePlayerInteractability>().AddListener(ToggleHandler);
        }

        private void Start()
        {
            // if (!playerCharacter)
            // 	LocatePlayer();
            // if (!raycastOrigin)
            // 	LocateMainCameraTransform();
        }

        private void OnDestroy()
        {
            // Signals.Get<OnPauseGameSignal>().RemoveListener(PauseGameHandler);
            // Signals.Get<TogglePlayerInteractability>().RemoveListener(ToggleHandler);
        }

        private void OnEnable()
        {
            myRaycastTimer.Restart();
        }

        private void OnDisable()
        {
            myRaycastTimer.Stop();
        }

        private void Update()
        {
            // if Input indicates we are to cause an Interact to occur
            if (interactRequested || GetInteractRequested())
            {
                IInteractable targetIInteractable = null;

                // prioritize raycast interactable since Player is pointing to it.
                if (allowRaycastInteractions && raycastInteractable != null)
                    targetIInteractable = raycastInteractable;
                else if (allowProximityInteractions && proximityIInteractable != null)
                    targetIInteractable = proximityIInteractable;

                if (actor == null)
                {
                    Debug.LogError("No actor has been set.");
                    return;
                }

                if (targetIInteractable == null)
                {
                    Debug.LogError("No interactable has been set.");
                    return;
                }

                // interact is requested and possible 
                interactEvent.Invoke();
                actor.Interact(targetIInteractable);
                interactRequested = false; //clear flag
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (allowProximityInteractions == false) return;

            if (!requireMatchingTag || other.gameObject.CompareTag(interactableTag))
            {
                var newIInteractable = other.GetComponent<IInteractable>();
                if (newIInteractable != null  //if encountered an IInteractable
                    && newIInteractable != proximityIInteractable) // prevents repeats / stuttering
                {
                    proximityIInteractable = newIInteractable;
                    enterRangeEvent.Invoke();
                    proximityIInteractable.OnEnterRange();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (allowProximityInteractions == false) return;

            if (!requireMatchingTag || other.gameObject.CompareTag(interactableTag))
            {
                var newIInteractable = other.GetComponent<IInteractable>();
                if (newIInteractable != null  //if encountered an IInteractable
                    && newIInteractable == proximityIInteractable) // prevents repeats / stuttering
                {
                    exitRangeEvent.Invoke();
                    proximityIInteractable.OnExitRange();
                    proximityIInteractable = null;
                }
            }
        }

        #endregion Unity Messages

        /// <summary>
        /// Determine if an Interact, through Input or other means, was requested.
        /// </summary>
        private bool GetInteractRequested()
        {
            var pressedInteract = false;

            if (useKeyCode)
            {
                pressedInteract = Input.GetKeyDown(interactKeyCode);
            }
            if (!pressedInteract && useButton)
            {
                pressedInteract = Input.GetButtonDown(interactButton);
            }

            return pressedInteract;
        }

        private void TakeInteractable(IInteractable interactable)
        {
            raycastInteractable = interactable; // track
            enterHoverEvent.Invoke();
            raycastInteractable.OnEnterHover();
        }

        private void ReleaseInteractable()
        {
            exitHoverEvent.Invoke();
            raycastInteractable.OnExitHover();
            raycastInteractable = null; // release
        }

        private IInteractable GetInteractableFromMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo,
                raycastLength, raycastLayerMask, queryTriggerInteraction)
                && (!requireMatchingTag || hitInfo.collider.CompareTag(interactableTag)))
            {
                return hitInfo.collider.GetComponent<IInteractable>();
            }
            return null;
        }

        private IInteractable GetInteractable()
        {
            // check to see if player is looking at interactable object's model
            var ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo,
                raycastLength, raycastLayerMask, queryTriggerInteraction)
                && (!requireMatchingTag || hitInfo.collider.CompareTag(interactableTag)))
            {
                return hitInfo.collider.GetComponent<IInteractable>();
            }
            return null;
        }

        /// <summary>
        /// Casts raycast to look for IInteractable within range.
        /// </summary>
        private void ProcessRaycast()
        {
            if (!allowRaycastInteractions) return; //check if raycasting is disabled.

            IInteractable newInteractable = GetInteractable();
            if (raycastInteractable != null)
                ReleaseInteractable();

            if (newInteractable != null)
                TakeInteractable(newInteractable);
        }

        ///<summary>
        /// Can externally request interaction start.
        ///</summary>
        public void RequestInteract() => interactRequested = true;

        ///<summary>
        /// Cancel a request for an interaction (only works if it was set requested this frame).
        ///</summary>
        public void CancelRequestInteract() => interactRequested = false;

        [Button("TestRay()")]
        public void DrawRay()
        {
            Debug.DrawRay(raycastOrigin.position,
                raycastOrigin.forward, Color.red, 2);
        }

        public static void RegisterInteractable(IInteractable i)
            => interactableList.AddIfNew(i);

        public static void UnregisterInteractable(IInteractable i)
            => interactableList.Remove(i);
    }
}
