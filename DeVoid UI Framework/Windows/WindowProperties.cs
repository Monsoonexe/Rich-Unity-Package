using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.UI.Framework
{
	/// <summary>
	/// The payload that gets passed to a <see cref="AWindow"/> with 
	/// all the data required to populate its fields.
	/// </summary>
	/// <seealso cref="PanelProperties"/>
	[Serializable]
	public class WindowProperties : IWindowProperties
	{
		[SerializeField,
			Tooltip("Should this Window be hidden when another Window takes its foreground?")]
		private EWindowLostForegroundBehavior lostForegroundBehavior = EWindowLostForegroundBehavior.Hide;

		[SerializeField,
			Tooltip("How other windows react to this window opening.")]
		private EWindowTakeForegroundBehavior takeForegroundBehavior = EWindowTakeForegroundBehavior.None;

		[SerializeField,
			Tooltip("How should this window behave in case another window is already opened?")]
		protected EWindowPriority windowQueuePriority = 0;

		[SerializeField,
			Tooltip("Popups are displayed in front of all other Windows.")]
		protected bool isPopup = false;

		[SerializeField, ShowIf(nameof(isPopup)),
			Tooltip("If this is a popup, should it darken the background?")]
		protected bool shouldDarkenBackground = true;

		#region IWindowProperties

		public EWindowPriority QueuePriority
		{
			get => windowQueuePriority;
			set => windowQueuePriority = value;
		}

		public bool SuppressPrefabProperties { get; set; }

		public EWindowLostForegroundBehavior LostForegroundBehavior
		{
			get => lostForegroundBehavior;
			set => lostForegroundBehavior = value;
		}

		public EWindowTakeForegroundBehavior TakeForegroundBehavior
		{
			get => takeForegroundBehavior;
			set => takeForegroundBehavior = value;
		}

		public bool IsPopup
		{
			get => isPopup;
			set => isPopup = value;
		}

		public bool ShouldDarkenBackground
		{
			get => shouldDarkenBackground;
			set => shouldDarkenBackground = value;
		}

		#endregion IWindowProperties
	}
}
