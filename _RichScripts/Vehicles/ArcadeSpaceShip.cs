using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// No Neutonian Physics.
/// </summary>
/// <seealso cref="SpaceShipController"/>
[RequireComponent(typeof(Rigidbody))]
public class ArcadeSpaceShip : RichMonoBehaviour
{
    /* To dampen, decrease the 'gravity' 
     * field of the Axis in the Input Manager.
     * I find 1 - 2 works well
    */

    [Header("---Speed Settings---")]
    public float maxSpeed = 50;

    [Header("---Throttle Settings---")]
    public float acceleration = 5;

    [Header("---Pitch Settings---")]
    public float pitchSpeed = 55;

    [Header("---Yaw Settings---")]
    public float yawSpeed = 40;

    [Header("---Roll Settings---")]
    public float rollSpeed = 120;

    [Header("---Brakes Settings---")]
    public float inertialDampenersEffect = 0.7f;

    [Header("---Lateral Thruster Settings---")]
    public Vector2 lateralThrustSpeed = new Vector2(3, 3);

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

    /// <summary>
    /// Movement around x-axis.
    /// </summary>
    public float PitchInput
    {
        get => pitchYawRollInput.x;
        set => pitchYawRollInput.x = value;
    }

    /// <summary>
    /// Movement around y-axis.
    /// </summary>
    public float YawInput
    {
        get => pitchYawRollInput.y;
        set => pitchYawRollInput.y = value;
    }

    /// <summary>
    /// Movement around z-axis.
    /// </summary>
    public float RollInput
    {
        get => pitchYawRollInput.z;
        set => pitchYawRollInput.z = value;
    }

    public float ThrottleInput { get; set; }
    public bool IsBraking { get; set; }

    /// <summary>
    /// For up/down and left/right lateral movement.
    /// </summary>
    public Vector2 LateralThrustInput
    {
        get => lateralThrustInput;
        set => lateralThrustInput = value;
    }
    public float HorizontalThrustInput
    {
        get => lateralThrustInput.x;
        set => lateralThrustInput.x = value;
    }
    public float VerticalThrustInput
    {
        get => lateralThrustInput.y;
        set => lateralThrustInput.y = value;
    }

    //runtime data
    private float fixedDeltaTime;
    private Vector3 pitchYawRollInput;
    private float speed = 0;
    public float CurrentSpeed { get => speed; }
    private float speedVelocity;
    private Vector2 lateralThrustInput;

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
        //myRigidbody.MoveRotation(myRigidbody.rotation * rotateQuat);
        //^^ moved next to MovePosition for better locality.

        HandleThrottle(ThrottleInput);

        //up/down, left/right
        //vertical thrust input mod
        var verticalThrustMod = lateralThrustInput.y 
            * lateralThrustSpeed.y;

        var verticalThrustersVector 
            = myTransform.up * verticalThrustMod;

        //horizontal thrust input mod
        var horizontalThrustMod = lateralThrustInput.x
            * lateralThrustSpeed.x;

        var horizontalThrustersVector
            = myTransform.right * horizontalThrustMod;

        //main thruster
        var forwardThrustVector = myTransform.forward
            * speed;

        //all combined movement
        var stepVector = verticalThrustersVector 
            + horizontalThrustersVector + forwardThrustVector;

        //scaled
        stepVector *= fixedDeltaTime;

        //final offsets applied to current values.
        var finalMovement = myTransform.position + stepVector;
        var finalRotation = myRigidbody.rotation * rotateQuat;

        //do actual transformations
        //cachedTransform.Translate(cachedTransform.forward * Time.deltaTime * speed, Space.World);
        myRigidbody.MovePosition(finalMovement);
        myRigidbody.MoveRotation(finalRotation);
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
            targetSpeed = RichMath.Clamp(targetSpeed, -maxSpeed, maxSpeed);

            speed = Mathf.Lerp(speed, targetSpeed, fixedDeltaTime);
        }
        else if (IsBraking)
        {
            speed *= inertialDampenersEffect;
        }
    }

}
