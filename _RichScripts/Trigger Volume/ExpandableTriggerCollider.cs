using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Trigger Collider that expands its radius when the player is within promixity.
/// Note: Must use a sphere collider.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class ExpandableTriggerCollider : TriggerCollider
{
    [Header("Expansion Settings")]
    [Range(1f, 2f)] public float expansionFactor;

    private SphereCollider sphereCollider;
    private float radiusOrigin;

    #region Initialization
    void OnEnable()
    {
        sphereCollider = (SphereCollider)triggerCollider;
        radiusOrigin = sphereCollider.radius;
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
        Debug.Log("Expanded");
        sphereCollider.radius = radiusOrigin * expansionFactor;
    }

    /// <summary>
    /// Decreases the radius of the collider
    /// </summary>
    public void Contract()
    {
        Debug.Log("Contracted");
        sphereCollider.radius = radiusOrigin;
    }
    #endregion
}
