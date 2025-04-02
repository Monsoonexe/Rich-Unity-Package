using System;
using UnityEngine;

namespace RichPackage.UI.Framework
{
	[Serializable]
	public class PanelPriorityLayerListEntry
	{
		[SerializeField]
		private EPanelPriority priority;

		public EPanelPriority Priority { get => priority; }

		[Tooltip("The Transform that should hold all Panels tagged with this priority.")]
		[SerializeField]
		private Transform targetParent;

		/// <summary>
		/// The Transform that should hold all Panels tagged with this priority.
		/// </summary>
		public Transform TargetParent { get => targetParent; }

		#region Constructor

		public PanelPriorityLayerListEntry(EPanelPriority priority, Transform parent)
		{
			this.priority = priority;
			targetParent = parent;
		}

		#endregion

	}
}
