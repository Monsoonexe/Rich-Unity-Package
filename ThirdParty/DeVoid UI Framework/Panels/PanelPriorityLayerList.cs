using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PanelPriorityLayerList
{
    [SerializeField]
    private List<PanelPriorityLayerListEntry> paraLayers
        = new List<PanelPriorityLayerListEntry>();

    private readonly Dictionary<EPanelPriority, Transform> lookup
        = new Dictionary<EPanelPriority, Transform>();

    #region Constructors

    public PanelPriorityLayerList()
    {
        // nada
    }

    public PanelPriorityLayerList(List<PanelPriorityLayerListEntry> entries)
    {
        paraLayers = entries;
    }

    #endregion Constructors

    public Dictionary<EPanelPriority, Transform> ParaLayerLookup
    {
        get
        {
            // validate
            if (lookup.Count == 0)
            {
                CacheLookup();
            }

            return lookup;
        }
    }

    /// <summary>
    /// Cache List to a Dictionary for quicker retrieval.
    /// </summary>
    private void CacheLookup()
    {
        for (int i = 0; i < paraLayers.Count; ++i)
        {
            PanelPriorityLayerListEntry layer = paraLayers[i];
            lookup.Add(layer.Priority, layer.TargetParent);
        }
    }
}
