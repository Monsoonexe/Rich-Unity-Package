using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// No Neutonian Physics.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ArcadeSpaceShip : RichMonoBehaviour
{
    [Header("---Speed Settings---")]
    [SerializeField] private float maxSpeed = 50;
    [SerializeField] private float speed = 0;
    [SerializeField] private float acceleration = 5;

    [Header("---Pitch Settings---")]
    [SerializeField] private float pitchSpeed = 50;

    [Tooltip("TODO....")]
    [ReadOnly]
    [SerializeField] private float pitchDamping = 0;

    [Header("---Yaw Settings---")]
    [SerializeField] private float yawSpeed = 15;

    [Tooltip("TODO....")]
    [ReadOnly]
    [SerializeField] private float yawDamping = 0;

    [Header("---Roll Settings---")]
    [SerializeField] private float rollSpeed = 25;

    [Tooltip("TODO....")]
    [ReadOnly]
    [SerializeField] private float rollDamping = 0;

    [Header("---Brakes Settings---")]
    [SerializeField] private float inertialDampenersEffect = 0.7f;

    //member Components
    private Rigidbody myRigidbody;

    //Inputs
    /// <summary>
    /// x-pitch, y-yaw, z-roll
    /// </summary>
    public Vector3 PitchYawRollInput
    {
        get => pitchYawRollInput;
        set => pitchYawRollInput = value;
    } 
    public float PitchInput
    {
        get => pitchYawRollInput.x;
        set => pitchYawRollInput.x = value;
    }
    public float RollInput
    {
        get => pitchYawRollInput.z;
        set => pitchYawRollInput.z = value;
    }
    public float YawInput
    {
        get => pitchYawRollInput.y;
        set => pitchYawRollInput.y = value;
    }

    public float ThrottleInput { get; set; }
    public bool IsBraking { get; set; }

    //runtime data
    private float fixedDeltaTime;
    private Vector3 pitchYawRollInput;

    protected override void Awake()
    {
        base.Awake();
        myRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        fixedDeltaTime = Time.fixedDeltaTime;//cache for multiple uses.

        HandleInertialDampener(); //brakes?

        var rotateVector = new Vector3
            (
                PitchInput * fixedDeltaTime * pitchSpeed,
                YawInput * fixedDeltaTime * yawSpeed,
                RollInput * fixedDeltaTime * rollSpeed
            );

        var rotateQuat = Quaternion.Euler(rotateVector);
        myRigidbody.MoveRotation(myRigidbody.rotation * rotateQuat);

        HandleThrottle(ThrottleInput);

        //do actual move
        //cachedTransform.Translate(cachedTransform.forward * Time.deltaTime * speed, Space.World);
        myRigidbody.MovePosition(myTransform.position + myTransform.forward
            * (fixedDeltaTime * speed));
    }

    private void HandleInertialDampener()
    {
        if (IsBraking)
        {
            myRigidbody.velocity = myRigidbody.velocity * 
                (inertialDampenersEffect * fixedDeltaTime);
            myRigidbody.angularVelocity = myRigidbody.angularVelocity 
                * (inertialDampenersEffect * fixedDeltaTime);

            PitchYawRollInput = Vector3.zero;
            ThrottleInput = 0;
        }
    }

    private void HandleThrottle(float throttleInput)
    {
        if (!IsBraking && Mathf.Abs(throttleInput) > 0)
        {
            //add forward or backward
            var targetSpeed = speed + throttleInput * acceleration;
            targetSpeed = Mathf.Clamp(targetSpeed, -maxSpeed, maxSpeed);

            speed = Mathf.Lerp(speed, targetSpeed, fixedDeltaTime);
        }

        else if (IsBraking)
        {
            speed *= inertialDampenersEffect;
        }
    }

    public void UpdateInput(bool apply,
        float throttle, Vector3 pitchYawRoll)
    {
        pitchYawRollInput = pitchYawRoll;
        ThrottleInput = throttle;
        IsBraking = apply;
    }

    public void UpdateInput(bool apply,
        float throttle, float pitch,
        float yaw, float roll)
    {
        pitchYawRollInput.x = pitch;
        pitchYawRollInput.y = yaw;
        pitchYawRollInput.z = roll;
        ThrottleInput = throttle;
        IsBraking = apply;
    }
}
