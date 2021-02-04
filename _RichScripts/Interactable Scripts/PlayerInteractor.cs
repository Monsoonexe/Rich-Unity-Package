using UnityEngine;
using Explore;
using Explore.Events;

/// <summary>
/// Handles triggering Interactions between a Player and the world.
/// </summary>
/// <remarks>
/// GameObject that is interactable MUST have implement the IPlayerInteractable interface,
/// MUST have a TriggerCollider attached, and MUST have the tag "Interactable".
/// 
/// IsKinematic Rigidbodies sometimes call OnTriggerEnter/Exit at unexpected times, 
/// or repeatedly
/// 
/// If you need more information, either follow the same pattern with a new IInteractable,
/// or get it from the 'context': player.Inventory[0].Item or something.
/// </remarks>
/// <seealso cref="IPlayerInteractable"/>
/// <seealso cref="IInteractable{T}"/>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerInteractor : RichMonoBehaviour
{
    [Header("--Settings--")]
    [SerializeField]
    private Player playerCharacter;
    private IPlayerInteractable targetInteractable;
    private Rigidbody myRigidbody;
    private Collider myCollider;

    protected override void Awake()
    {
        base.Awake();
        myRigidbody = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();

        myRigidbody.isKinematic = true;
        myRigidbody.useGravity = false;
        myRigidbody.angularDrag = 0;
        myRigidbody.drag = 0;
        myRigidbody.mass = Mathf.Epsilon;//can't actually be zero.

        Signals.Get<OnPauseGameSignal>().AddListener(PauseGameHandler);
        Signals.Get<TogglePlayerInteractability>().AddListener(ToggleHandler);
    }

    private void OnDestroy()
    {

        Signals.Get<OnPauseGameSignal>().RemoveListener(PauseGameHandler);
        Signals.Get<TogglePlayerInteractability>().RemoveListener(ToggleHandler);
    }

    private void Update()
    {
        //check for button press and an Interactable nearby
        if (Input.GetButtonDown(ConstStrings.INPUT_INTERACT) && targetInteractable != null)
        {
            targetInteractable.Interact(playerCharacter);//do interaction
            Signals.Get<InteractableInteractSignal>() // alert any other passive listeners
                .Dispatch(targetInteractable, playerCharacter);//interaction occured
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(ConstStrings.TAG_INTERACTABLE))
        {
            var newInteractable = other.GetComponent<IPlayerInteractable>();
            if(newInteractable != targetInteractable) // prevents repeats / stuttering
            {
                targetInteractable = newInteractable;
                targetInteractable.OnEnterRange(playerCharacter);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(ConstStrings.TAG_INTERACTABLE))
        {
            var newInteractable = other.GetComponent<IPlayerInteractable>();
            if (newInteractable == targetInteractable) // prevents repeats / stuttering
            {
                targetInteractable.OnExitRange(playerCharacter);
                targetInteractable = null;
            }
        }
    }

    private void ToggleHandler(bool active)
    {
        this.enabled = active; // en/disable Update()
        myCollider.enabled = active;// en/disable collisions
    }

    private void PauseGameHandler(bool paused) => this.enabled = !paused;
}
