using System;
using UnityEngine;

[Serializable]
public class PanelProperties : IPanelProperties
{
    [Tooltip("Panels go to different para-layers depending on their priority. You can set up para-layers in the Panel Layer.")]
    [SerializeField]
    private PanelPriorityENUM priority;

    public PanelPriorityENUM Priority { get => priority; set => priority = value; }

    #region Constructors

    public PanelProperties()
    {
        priority = PanelPriorityENUM.Prioritary;
    }

    public PanelProperties(PanelPriorityENUM priority)
    {
        this.priority = priority;
    }

    #endregion

}
