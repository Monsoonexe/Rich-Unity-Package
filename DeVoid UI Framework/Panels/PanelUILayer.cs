using UnityEngine;

namespace RichPackage.UI.Framework
{
	/// <seealso cref="WindowUILayer"/>
	/// <seealso cref="APanel"/>
	public class PanelUILayer : AUILayer<IPanel>
	{
		[SerializeField]
		private PanelPriorityLayerList priorityLayers = new PanelPriorityLayerList();

		private void ReparentToParaLayer(EPanelPriority priority, Transform screenTransform)
		{
			screenTransform.SetParent(priorityLayers.LookupLayer(priority), false);
		}

		public sealed override void ReparentScreen(IUIScreen screen, Transform screenTransform)
		{
			if (screen is IPanel panel)
			{
				ReparentToParaLayer(panel.Priority, screenTransform);
			}
			else
			{
				base.ReparentScreen(screen, screenTransform);
			}
		}

		public sealed override void HideScreen(IPanel panel, bool animate = true)
		{
			panel.Hide(animate);
		}

		protected sealed override void ProcessScreenRegister(string screenID, IPanel panel)
		{
			base.ProcessScreenRegister(screenID, panel);
			panel.Layer = this;
		}

		protected sealed override void ProcessScreenUnregister(string screenID, IPanel panel)
		{
			base.ProcessScreenUnregister(screenID, panel);
			panel.Layer = null;
		}

		public sealed override void ShowScreen(IPanel panel) => ShowScreen(panel, null);

		public sealed override void ShowScreen(IPanel panel, IScreenProperties properties)
		{
			panel.Show(properties);
		}

		public bool IsPanelVisible(string panelID)
		{
			return registeredScreens.TryGetValue(panelID, out IPanel panel)
				&& panel.IsOpen;
		}
	}
}
