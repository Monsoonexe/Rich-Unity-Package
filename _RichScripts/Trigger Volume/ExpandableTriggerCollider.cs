//credit: William Lau

using UnityEngine;

/// <summary>
/// Trigger Collider that expands its radius when something is within promixity.
/// Note: Must use a sphere collider.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ExpandableTriggerCollider : TriggerVolume
{
    [Header("Expansion Settings")]
    [Min(0))] public float expandedRadius = 10f;

    private SphereCollider sphereCollider;
    private float radiusOrigin;

	protected override void Awake()
    {
        base.Awake();
        sphereCollider = (SphereCollider)triggerCollider;
        radiusOrigin = sphereCollider.radius;
    }

	#region Initialization
	void OnEnable()
    {
        OnEnter.AddListener(Expand);
        OnExit.AddListener(Contract);
    }

    void OnDisable()
    {
        OnEnter.RemoveListener(Expand);
        OnExit.RemoveListener(Contract);
    }
    #endregion

    #region Expanding
    /// <summary>
    /// Increases the radius of the collider
    /// </summary>
    public void Expand()
    {
        sphereCollider.radius = expandedRadius;
    }

    /// <summary>
    /// Decreases the radius of the collider
    /// </summary>
    public void Contract()
    {
        sphereCollider.radius = radiusOrigin;
    }
    #endregion
}
