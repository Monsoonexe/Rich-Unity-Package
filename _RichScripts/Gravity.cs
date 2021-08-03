using UnityEngine;

public class Gravity : RichMonoBehaviour
{
    public static readonly Vector3 STANDARD_GRAVITY 
        = new Vector3(0, -9.81f, 0);

    [Header("---Settings---")]
    [SerializeField]
    private Vector3 gravityVector = STANDARD_GRAVITY;

    [Header("---Ground Detection Settings---")]
    [SerializeField]
    [Min(0.2f)]
    private float groundDetectDistance = 0.5f;

    [SerializeField]
    private QueryTriggerInteraction detectTriggersAsGround 
        = QueryTriggerInteraction.Ignore;

    [SerializeField]
    private LayerMask raycastLayerMask = 512;//9 = ground

    public bool IsGrounded { get; private set; }

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform raycastOriginPoint;

    private void Reset()
    {
        SetDevDescription("I act as gravity for objects with no non-kinematic rigidbody.");
        raycastOriginPoint = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        HandleGroundDetection();
        if(!IsGrounded)
        {
            var step = gravityVector * ApexGameController.FixedDeltaTime;
            myTransform.position += step;
        }
    }

    private void HandleGroundDetection()
    {
        //grounded if hit something on ground layer
        IsGrounded = Physics.Raycast(
            raycastOriginPoint.position, gravityVector,
            out RaycastHit hitInfo,
            groundDetectDistance, raycastLayerMask,
            detectTriggersAsGround);
    }
}
