using System;
using UnityEngine;

[Serializable]
public class PanelPriorityLayerListEntry
{
    [Tooltip("")]
    [SerializeField]
    private PanelPriorityENUM priority;

    public PanelPriorityENUM Priority { get => priority; set => priority = value; } // wrap serialized field

    [Tooltip("The Transform that should hold all Panels tagged with this priority.")]
    [SerializeField]
    private Transform targetParent;

    /// <summary>
    /// The Transform that should hold all Panels tagged with this priority.
    /// </summary>
    public Transform TargetParent { get => targetParent; set => targetParent = value; } // wrap serialized field

    #region Constructor

    public PanelPriorityLayerListEntry(PanelPriorityENUM priority, Transform parent)
    {
        this.priority = priority;
        this.targetParent = parent;
    }

    #endregion

}
