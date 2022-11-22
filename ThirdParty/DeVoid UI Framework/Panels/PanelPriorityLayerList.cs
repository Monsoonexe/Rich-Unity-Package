using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PanelPriorityLayerList
{
    [Tooltip("")]
    [SerializeField]
    private List<PanelPriorityLayerListEntry> paraLayers = 
        new List<PanelPriorityLayerListEntry>();

    private Dictionary<EPanelPriority, Transform> lookup = 
        new Dictionary<EPanelPriority, Transform>();

    public Dictionary<EPanelPriority, Transform> ParaLayerLookup
    {
        get
        {
            //validate
            if(lookup == null || lookup.Count == 0)
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
        lookup = new Dictionary<EPanelPriority, Transform>();

        for(var i = 0; i < paraLayers.Count; ++i)
        {
            lookup.Add(paraLayers[i].Priority, paraLayers[i].TargetParent);
        }
    }

    #region Constructors

    public PanelPriorityLayerList()
    {

    }

    public PanelPriorityLayerList(List<PanelPriorityLayerListEntry> entries)
    {
        paraLayers = entries;
    }

    #endregion Constructors
}
