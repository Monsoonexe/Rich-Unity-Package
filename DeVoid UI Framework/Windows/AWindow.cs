namespace RichPackage.UI.Framework
{
	/// <summary>
	/// Base class for Windows -- a Screen which is the main point of interest at a given time, 
	/// usually using all or most of the display. Eg: popups, modals. Composed of 1+ widgets.
	/// </summary>
	/// <seealso cref="APanel"/>
	public abstract class AWindow<TProps> : AUIScreen<TProps>, IWindow where TProps : IWindowProperties
	{
		#region IWindow

		public bool IsPopup { get => Properties.IsPopup; } // repeat
		public bool ShouldDarkenBackground { get => IsPopup && Properties.ShouldDarkenBackground; }
		IWindowProperties IWindow.Properties { get => Properties; }
		UIFrame IWindow.Frame { get; set; }
		protected IWindow Window => this; // fast cast

		#endregion IWindow

		protected sealed override void SetProperties(TProps payload)
		{
			if (payload != null)
			{
				// if these new properties don't suppress the defaults, use the defaults instead.
				if (!payload.SuppressPrefabProperties)
				{
					payload.LostForegroundBehavior = Properties.LostForegroundBehavior;
					payload.TakeForegroundBehavior = Properties.TakeForegroundBehavior;
					payload.QueuePriority = Properties.QueuePriority;
					payload.IsPopup = Properties.IsPopup;
					payload.ShouldDarkenBackground = Properties.ShouldDarkenBackground;
				}

				base.SetProperties(payload);
			}
		}

		protected override void OnHierarchyChanged()
		{
			BringToForeground();
		}

		/// <summary>
		/// Puts this window on top of all other windows on its layer.
		/// </summary>
		protected void BringToForeground()
		{
			transform.SetAsLastSibling(); // place last in list so it's drawn on top of everything else
		}

		void IWindow.OnForegroundLost() => OnForegroundLost();

		/// <remarks>No need to call base.</remarks>
		protected virtual void OnForegroundLost() { }

		/// <summary>
		/// Requests this Window to be closed.
		/// </summary>
		/// <remarks>Animates if able.</remarks>
		/// <seealso cref="IWindow.OnHide"/>
		public void Close() => Window.Frame.HideScreen(ScreenID, animate: true);

		/// <summary>
		/// Requests this Window to be closed without animations.
		/// </summary>
		/// <remarks>Suppresses animation.</remarks>
		public void CloseImmediately() => Window.Frame.HideScreen(ScreenID, animate: false);
	}

	/// <summary>
	/// Base implementation for Window Screen Controllers that need no special Properties
	/// </summary>
	public abstract class AWindow : AWindow<WindowProperties>
	{
		// basic
	}
}
