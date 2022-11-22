using System;
using UnityEngine;

/// <summary>
/// The payload that gets passed to a <see cref="APanelController"/> with 
/// all the data required to populate its fields.
/// </summary>
/// <seealso cref="WindowProperties"/>
[Serializable]
public partial class PanelProperties : IPanelProperties
{
    [Tooltip("Panels go to different para-layers depending on their priority. You can set up para-layers in the Panel Layer.")]
    [SerializeField]
    private EPanelPriority priority;

    public EPanelPriority Priority { get => priority; set => priority = value; }

    #region Constructors

    public PanelProperties()
    {
        priority = EPanelPriority.Prioritary;
    }

    public PanelProperties(EPanelPriority priority)
    {
        this.priority = priority;
    }

    #endregion

}
