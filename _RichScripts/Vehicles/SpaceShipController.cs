using UnityEngine;
using NaughtyAttributes;

public class SpaceShipController : RichMonoBehaviour
{
    #region Input Axes

    [Foldout("---Input Axes---")]
    [SerializeField]
    [InputAxis]
    private string yawInputName = "Horizontal";

    [Foldout("---Input Axes---")]
    [SerializeField]
    [InputAxis]
    private string pitchInputName = "Vertical";

    [Foldout("---Input Axes---")]
    [SerializeField]
    [InputAxis]
    private string throttleInputName = "Throttle";

    [Foldout("---Input Axes---")]
    [SerializeField]
    [InputAxis]
    private string rollInputName = "Roll";

    [Foldout("---Input Axes---")]
    [SerializeField]
    [InputAxis]
    private string brakesName = "InertialDampener";

    #endregion

    [Header("---Resources---")]
    [Tooltip("Take the pilot's seat!")]
    [Required]
    public ArcadeSpaceShip spaceShip;

    private void Update()
    {
        //get movement input
        var pitchInput = Input.GetAxis(pitchInputName);
        var yawInput = Input.GetAxis(yawInputName);
        var rollInput = Input.GetAxis(rollInputName);
        var throttleInput = Input.GetAxis(throttleInputName);
        var isBraking = Input.GetButton(brakesName);

        //combine
        var inputVector = new Vector3(pitchInput, yawInput, rollInput);

        //Drive
        spaceShip.IsBraking = isBraking;
        spaceShip.ThrottleInput = throttleInput;
        spaceShip.PitchYawRollInput = inputVector;
    }
}
