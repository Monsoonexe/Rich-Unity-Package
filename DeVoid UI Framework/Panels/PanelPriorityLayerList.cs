using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.UI.Framework
{
	[Serializable]
	public class PanelPriorityLayerList
	{
		[SerializeField]
		private List<PanelPriorityLayerListEntry> paraLayers = new List<PanelPriorityLayerListEntry>();

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

		public Transform LookupLayer(EPanelPriority priority)
		{
			for (int i = 0; i < paraLayers.Count; i++)
			{
				PanelPriorityLayerListEntry entry = paraLayers[i];
				if (entry.Priority == priority)
					return entry.TargetParent;
			}

			throw new KeyNotFoundException(priority.ToString());
		}
	}
}
