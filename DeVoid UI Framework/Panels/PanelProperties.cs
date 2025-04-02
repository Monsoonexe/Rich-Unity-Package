using System;
using UnityEngine;

namespace RichPackage.UI.Framework
{
	/// <summary>
	/// The payload that gets passed to a <see cref="APanel"/> with 
	/// all the data required to populate its fields.
	/// </summary>
	/// <seealso cref="WindowProperties"/>
	[Serializable]
	public class PanelProperties : IPanelProperties
	{
		[Tooltip("Panels go to different para-layers depending on their priority. You can set up para-layers in the Panel Layer.")]
		[SerializeField]
		private EPanelPriority priority = EPanelPriority.Default;

		public EPanelPriority Priority { get => priority; set => priority = value; }
	}
}
