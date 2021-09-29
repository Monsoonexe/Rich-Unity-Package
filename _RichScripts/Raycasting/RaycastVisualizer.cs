using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class RaycastVisualizer : RichMonoBehaviour
{
    [Required]
    public Transform origin;

    [Required]
    public Transform target;

    [MinValue(0)]
    public float rayInterval = 1.0f;

    [MinValue(0)]
    public float targetDistance = 50;

    [ReadOnly]
    [SerializeField]
    private float actualDistance;

    private LineRenderer myLineRenderer;
    public LineRenderer LineRenderer
    {
        get
        {
            if (myLineRenderer == null)
                return myLineRenderer = GetComponent<LineRenderer>();
            else
                return myLineRenderer;
        }
    }
    private float nextRayTime;

    protected override void Awake()
    {
        base.Awake();
        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.positionCount = 2;
    }

    private void Update()
    {
        var currentTime = Time.time;
        if(nextRayTime < currentTime)
        {
            nextRayTime = currentTime += rayInterval;
            UpdateLineRenderer();
        }
    }

    [Button]
    public void UpdateLineRenderer()
    {
        var originPosition = origin.position;
        var targetPosition = target.position;
        var directionVector = targetPosition - originPosition;
        var ray = new Ray(originPosition, directionVector);
        actualDistance = Vector3.Distance(originPosition, targetPosition);

        LineRenderer.SetPosition(0, originPosition);
        //LineRenderer.SetPosition(1, targetPosition);
        LineRenderer.SetPosition(1, ray.GetPoint(targetDistance));
    }

    private void Reset()
    {
        Awake();
    }
}
