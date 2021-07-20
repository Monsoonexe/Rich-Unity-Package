using UnityEngine;

public class SmoothFollow : RichMonoBehaviour
{
    [SerializeField]
    private Transform followTarget;
    public Transform FollowTarget
    {
        get => followTarget;
        set => followTarget = value;
    }

    [SerializeField]
    private Transform lookTarget;
    public Transform LookTarget
    {
        get => lookTarget;
        set => lookTarget = value;
    }

    public Vector3 offset = new Vector3(0, -2, 8);

    public float Distance
    {
        get => offset.z;
        set => offset.z = value;
    }

    public float Height
    {
        get => offset.y;
        set => offset.y = value;
    }

    [SerializeField]
    private float rotationDamping;

    [SerializeField]
    private Vector3 damping = new Vector3(3,3,3);

    private void LateUpdate()
    {
        // Early out if we don't have a target
        if (!followTarget) return;
        var followPoint = followTarget.position; //cache

        // Calculate the current rotation angles
        var wantedRotationAngle = followTarget.eulerAngles.y;
        var wantedHeight = followPoint.y + offset.y;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentPosition = transform.position;

        var deltaTime = Time.deltaTime;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(
            currentRotationAngle, wantedRotationAngle, 
            rotationDamping * deltaTime);

        // Damp the height
        currentPosition.y = Mathf.Lerp(currentPosition.y, 
            wantedHeight, damping.y * deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, 
            currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        var targetPos = followPoint - currentRotation 
            * (Vector3.forward * offset.z);

        // Damp the lateral
        targetPos.x = Mathf.Lerp(currentPosition.x,
            targetPos.x, damping.x * deltaTime);

        // Damp the follow distance
        targetPos.z = Mathf.Lerp(currentPosition.z,
            targetPos.z, damping.z * deltaTime);

        // Set the height of the camera
        transform.position = targetPos.WithY(currentPosition.y);

        // point at the target
        if(lookTarget)
            transform.LookAt(lookTarget);
    }

    public void SnapUpdate()
    {
        // Early out if we don't have a target
        if (!followTarget) return;

        // Calculate the current rotation angles
        var wantedRotationAngle = followTarget.eulerAngles.y;
        var wantedHeight = followTarget.position.y + offset.y;

        var currentRotationAngle = transform.eulerAngles.y;
        var currentHeight = transform.position.y;

        currentRotationAngle = wantedRotationAngle;

        // Damp the height
        currentHeight = wantedHeight;

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0,
            currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        var targetPos = followTarget.position - currentRotation
            * (Vector3.forward * offset.z);

        // Set the height of the camera
        transform.position = targetPos.WithY(currentHeight);

        // point at the target
        if (lookTarget)
            transform.LookAt(lookTarget);
    }
}
