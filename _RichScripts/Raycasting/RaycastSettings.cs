using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[Serializable]
public struct RaycastSettings
{
    [Min(0)]
    public float distance;
    public LayerMask layerMask;
    public QueryTriggerInteraction queryTriggers;

    #region Constructors

    public RaycastSettings(float distance, LayerMask layerMask,
        QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Ignore)
    {
        this.distance = distance;
        this.layerMask = layerMask;
        this.queryTriggers = queryTrigger;
    }

    #endregion

    public static RaycastSettings Default
        => new RaycastSettings(0.5f, -1,
            QueryTriggerInteraction.Ignore);

}
