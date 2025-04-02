using System.Collections.Generic;

namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Base class for Panels -- a given chunk of UI that can coexist at the same time as 
	/// other pieces of UI. Eg: status bars, elements on your HUD. Encapsulate 1+ widgets.
	/// </summary>
	/// <seealso cref="AWindow"/>
	public abstract class APanel<TProps> : AUIScreen<TProps>, IPanel
		where TProps : IPanelProperties
	{
		// need a stack because multiple windows might make the same request
		private Stack<bool> stateHistory;

		#region IPanel

		public EPanelPriority Priority => Properties?.Priority ?? EPanelPriority.Default;
		PanelUILayer IPanel.Layer { get; set; }
		protected IPanel Panel => this; // fast cast

		#endregion IPanel

		public void Push()
		{
			// lazy init
			if (stateHistory == null)
				stateHistory = new Stack<bool>();

			bool isOpen = IsOpen;
			stateHistory.Push(isOpen);
			if (isOpen)
				Hide();
		}

		public void Pop()
		{
			bool wasOpen = stateHistory.Pop();
			if (wasOpen)
				Show();
		}

		/// <remarks>Animates if able.</remarks>
		public void Show() => Panel.Layer.ShowScreen(this);
		/// <remarks>Animates if able.</remarks>
		public void Hide() => Panel.Layer.HideScreen(this, animate: true);
		/// <remarks>Suppresses animation.</remarks>
		public void HideImmediately() => Panel.Layer.HideScreen(this, animate: false);

		protected sealed override void SetProperties(TProps payload)
		{
			// seal the default impl
			base.SetProperties(payload);
		}
	}

	/// <summary>
	/// Base class for Panels with no special Properties.
	/// </summary>
	public abstract class APanel : APanel<PanelProperties>
	{
		// exists
	}
}
