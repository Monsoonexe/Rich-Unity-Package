using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

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
/// <seealso cref="IPlayerIInteractable"/>
/// <seealso cref="IIInteractable{T}"/>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public sealed class InteractionManager : RichMonoBehaviour
{
    private static List<IInteractable> interactableList = new List<IInteractable>(12);

    public PlayerScript playerCharacter;

    [Header("---Settings---")]
    public bool requireMatchingTag = true;

    [ShowIf("requireMatchingTag")]
    [Tag]
    public string interactableTag = "Interactable";

    public bool allowProximityInteractions = true;

    public bool useKeyCode = false;

    [ShowIf("useKeyCode")]
    public KeyCode interactKeyCode = KeyCode.Space;

    public bool useButton = true;

    [ShowIf("useButton")]
    [InputAxis]
    public string interactButton = "Fire1";

    [Header("---Raycast Settings---")]
    public bool allowRaycastInteractions = true;

    public QueryTriggerInteraction queryTriggerInteraction 
        = QueryTriggerInteraction.Ignore;

    public LayerMask raycastLayerMask;

    [MinValue(0)]
    public float raycastLength = 10.0f;

    [Tooltip("[Modifying has no effect in PlayMode]\r\n" 
        + "Seconds between each raycast query for an IInteractable.\r\n"
        + "Lower is more responsive but costly.")]
    [MinValue(0)]
    public float raycastInterval = 0.25f;

    public Transform raycastOrigin = null;

    [Header("---Events---")]
    [SerializeField] 
    private UnityEvent interactEvent = new UnityEvent();
    public UnityEvent OnInteractEvent { get => interactEvent; }

    [SerializeField]
    private UnityEvent enterEvent = new UnityEvent();
    public UnityEvent OnEnterEvent { get => enterEvent; }

    [SerializeField]
    private UnityEvent exitEvent = new UnityEvent();
    public UnityEvent OnExitEvent { get => exitEvent; }

    [SerializeField]
    private UnityEvent enterHoverEvent = new UnityEvent();
    public UnityEvent OnEnterHoverEvent { get => enterHoverEvent; }

    [SerializeField]
    private UnityEvent exitHoverEvent = new UnityEvent();
    public UnityEvent OnExitHoverEvent { get => exitHoverEvent; }

    //member components
    private Rigidbody myRigidbody;
    private Collider myCollider;
    private Timer myRaycastTimer;

    //runtime data
    private IInteractable proximityIInteractable;
    private IInteractable raycastIInteractable; //has higher priority

    /// <summary>
    /// Can externally request an Interact to occur.
    /// </summary>
    private bool interactRequested = false;

    protected override void Awake()
    {
        base.Awake();

        //gather member component references
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        myRaycastTimer = gameObject.AddComponent<Timer>(); //this timer isn't for mortal eyes 0.o

        //configure timer to raycast on repeat
        myRaycastTimer.Initialize(raycastInterval, HandleRaycast, loop: true);

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
        if(!playerCharacter)
            LocatePlayer();
        if(!raycastOrigin)
            LocateMainCameraTransform();
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
        //if Input indicates we are to cause an Interact to occur
        if(interactRequested || GetInteractRequested())
        {
            IInteractable targetIInteractable = null;

            //prioritize raycast interactable since Player is pointing to it.
            if(allowRaycastInteractions && raycastIInteractable != null)
                targetIInteractable = raycastIInteractable;
            else if (allowProximityInteractions && proximityIInteractable != null)
                targetIInteractable = proximityIInteractable;

            //interact is requested and possible
            if(targetIInteractable != null)
            {   //do interaction
                interactEvent.Invoke();
                targetIInteractable.Interact(playerCharacter);
            }
            interactRequested = false; //clear flag
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(allowProximityInteractions == false) return;

        if (!requireMatchingTag || other.gameObject.CompareTag(interactableTag))
        {
            var newIInteractable = other.GetComponent<IInteractable>();
            if(newIInteractable != null  //if encountered an IInteractable
                && newIInteractable != proximityIInteractable) // prevents repeats / stuttering
            {
                proximityIInteractable = newIInteractable;
                enterEvent.Invoke();
                proximityIInteractable.OnEnterRange(playerCharacter);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(allowProximityInteractions == false) return;

        if (!requireMatchingTag || other.gameObject.CompareTag(interactableTag))
        {
            var newIInteractable = other.GetComponent<IInteractable>();
            if (newIInteractable != null  //if encountered an IInteractable
                && newIInteractable == proximityIInteractable) // prevents repeats / stuttering
            {
                exitEvent.Invoke();
                proximityIInteractable.OnExitRange(playerCharacter);
                proximityIInteractable = null;
            }
        }
    }

    /// <summary>
    /// Determine if an Interact, through Input or other means, was requested.
    /// </summary>
    private bool GetInteractRequested()
    {
        var pressedInteract = false;

        if(useKeyCode)
        {
            pressedInteract = Input.GetKeyDown(interactKeyCode);
        }
        if(!pressedInteract && useButton)
        {
            pressedInteract = Input.GetButtonDown(interactButton);
        }

        return pressedInteract;
    }

    /// <summary>
    /// Casts raycast to look for IInteractable within range.
    /// </summary>
    private void HandleRaycast()
    {
        if(!allowRaycastInteractions) return; //check if raycasting is disabled.
            
        //check to see if player is looking at interactable object's model
        RaycastHit hitInfo;
        var ray = new Ray(raycastOrigin.position, raycastOrigin.forward);
        var rayHitIInteractable = false;

        //shoot a raycast from the cameras position forward, store the info, limit the ray to this length, use normal raycast layers, ignore triggerColliders
        if (Physics.Raycast(ray, out hitInfo, 
            raycastLength, raycastLayerMask, queryTriggerInteraction))
        {
            //is the player looking at the item model?
            if (!requireMatchingTag || hitInfo.collider.CompareTag(interactableTag))
            {
                var newIInteractable = hitInfo.collider.GetComponent<IInteractable>();
                rayHitIInteractable = newIInteractable != null;

                if(rayHitIInteractable)
                {
                    if(raycastIInteractable == null)//first encounter
                    {
                        raycastIInteractable = newIInteractable; //track

                        //begin hover
                        enterHoverEvent.Invoke();
                        raycastIInteractable.OnEnterHover(playerCharacter);
                    }
                    else if(raycastIInteractable != newIInteractable)//saw something different
                    {
                        //exit hover
                        exitHoverEvent.Invoke();
                        raycastIInteractable.OnExitHover(playerCharacter);

                        raycastIInteractable = newIInteractable; //track

                        //enter hover
                        enterHoverEvent.Invoke();
                        raycastIInteractable.OnEnterHover(playerCharacter);
                    }//else saw the same interactable -- no reaction
                }//else hit something, but it didn't have an IInteractable on it.
            } //else hit something, but it didn't have an IInteractable on it.
        }//else hit nothing at all

        //hit nothing but was tracking something, so need to release it
        if(!rayHitIInteractable && raycastIInteractable != null)
        {   //release interactable
            exitHoverEvent.Invoke();
            raycastIInteractable.OnExitHover(playerCharacter);
            raycastIInteractable = null;
        }
    }

    public void LocatePlayer()
    {
        playerCharacter = FindObjectOfType<PlayerScript>();
    }

    public void LocateMainCameraTransform()
    {
        raycastOrigin = GameObject.FindGameObjectWithTag(
            ConstStrings.TAG_MAIN_CAMERA)
            .GetComponent<Transform>();
    }

    ///<summary>
    /// Can externally request interaction start.
    ///</summary>
    public void RequestInteract() => interactRequested = true;

    ///<summary>
    /// Cancel a request for an interaction (only works if it was set requested this frame).
    ///</summary>
    public void CancelRequestInteract() => interactRequested = false;

    [Button("TestRay()", EButtonEnableMode.Always)]
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
