using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.UI.Framework
{
	/// <seealso cref="AWindow"/>
	/// <seealso cref="PanelUILayer"/>
	public class WindowUILayer : AUILayer<IWindow>
	{
		[SerializeField, Required]
		private WindowParaLayer priorityParaLayer;

		public IWindow CurrentWindow { get; private set; } // readonly

		private readonly List<IUIScreen> screensTransitioning = new List<IUIScreen>();
		private readonly Queue<WindowHistoryEntry> windowQueue = new Queue<WindowHistoryEntry>();
		private readonly Stack<WindowHistoryEntry> windowHistory = new Stack<WindowHistoryEntry>();

		[ShowInInspector, ReadOnly]
		public string CurrentWindowID => CurrentWindow?.ScreenID ?? "none";

		/// <summary>
		/// Is a Window currently Transitioning?
		/// </summary>
		public bool IsScreenTransitionInProgress
		{
			get => screensTransitioning.Count != 0;
		}

		#region Process Screens

		protected sealed override void ProcessScreenRegister(string screenID, IWindow window)
		{
			base.ProcessScreenRegister(screenID, window);
			window.Frame = frame;

			// rig events
			window.OnTransitionInFinishedCallback += OnInAnimationFinished;
			window.OnTransitionOutFinishedCallback += OnOutAnimationFinished;
		}

		protected sealed override void ProcessScreenUnregister(string screenID, IWindow window)
		{
			base.ProcessScreenUnregister(screenID, window);
			window.Frame = null;

			// unrig events
			window.OnTransitionInFinishedCallback -= OnInAnimationFinished;
			window.OnTransitionOutFinishedCallback -= OnOutAnimationFinished;
		}

		#endregion Process Screens

		#region Show/Hide Screens

		private void HandleOverlap(IWindow newWindow, IWindow currentWindow)
		{
			bool newWindowIsPopup = newWindow.IsPopup;
			IWindowProperties currentProperties = currentWindow.Properties;
			IWindowProperties newProperties = newWindow.Properties;

			// pre-emptive behavior
			if (!newWindowIsPopup)
			{
				if (newProperties.TakeForegroundBehavior == EWindowTakeForegroundBehavior.CloseAll)
				{
					HideAll();
					return;
				}

				if (currentProperties.LostForegroundBehavior == EWindowLostForegroundBehavior.Close
					|| newProperties.TakeForegroundBehavior == EWindowTakeForegroundBehavior.ClosePrevious)
				{
					// close the window
					HideScreen(currentWindow);
					return;
				}
			}

			currentWindow.OnForegroundLost();
			if (currentProperties.LostForegroundBehavior == EWindowLostForegroundBehavior.Hide
				&& !newWindowIsPopup) // and not a pop-up, which are always on top
			{
				currentWindow.Hide(animate: true);
				return;
			}
		}

		private void DoShow(WindowHistoryEntry entry) => DoShow(entry.screen, entry.properties);

		private void DoShow(IWindow window, IWindowProperties properties)
		{
			IWindow currentWindow = CurrentWindow;

			// validate
			if (window == currentWindow)
			{
				Debug.LogWarning($"[{nameof(WindowUILayer)}] Requested windowID ({currentWindow.ScreenID}) " +
					"is already open! Ignoring request.");
				return;
			}

			// hide current window?
			if (currentWindow != null)
			{
				HandleOverlap(window, currentWindow);
			}

			windowHistory.Push(new WindowHistoryEntry(window, properties));
			AddTransition(window);

			if (window.ShouldDarkenBackground)
				priorityParaLayer.DarkenBackground();

			window.Show(properties); // show it!
			CurrentWindow = window; //set as active window
		}

		public sealed override void HideScreen(IWindow window, bool animate = true)
		{
			// verify screen being closed is current window
			if (window != CurrentWindow)
			{
				Debug.LogWarning($"[{nameof(WindowUILayer)}] Hide requested for id '{window.ScreenID}' "
					+ $"but it's not the currently open one ({CurrentWindowID})! " +
					"Ignoring request.");
				return;
			}

			windowHistory.Pop();
			AddTransition(window);
			window.Hide(animate);
			CurrentWindow = null;

			// show next in queue
			if (windowQueue.Count > 0)
			{
				//Debug.LogFormat("if( windowQueue.Count ({0}) > 0", windowQueue.Count);
				Next();
			}
			// show next from history
			else if (windowHistory.Count > 0)
			{
				//Debug.LogFormat("if( windowHistory.Count ({0}) > 0", windowHistory.Count);
				Previous();
			}
			// else all closed/hid
		}

		/// <summary>
		/// Close all Windows.
		/// </summary>
		public sealed override void HideAll(bool animate = true)
		{
			if (CurrentWindow == null)
				return;

			do
			{
				HideScreen(CurrentWindow, false);
			}
			while (CurrentWindow != null);

			priorityParaLayer.RefreshDarken();
			windowHistory.Clear();
		}

		/// <summary>
		/// Show a Screen with default data payload.
		/// </summary>
		public sealed override void ShowScreen(IWindow window) => ShowScreen(window, null);

		/// <summary>
		/// Show Screen and load with given data payload.
		/// </summary>
		public sealed override void ShowScreen(IWindow window, IScreenProperties properties)
		{
			var windowProperties = (IWindowProperties)properties;

			if (ShouldEnqueue(window, windowProperties))
			{
				EnqueueWindow(window, windowProperties);
			}
			else
			{
				DoShow(window, windowProperties);
			}
		}

		#endregion Show/Hide Screens

		#region Screen History

		private void EnqueueWindow(IWindow window, IWindowProperties properties)
		{
			windowQueue.Enqueue(new WindowHistoryEntry(window, properties));
		}

		private bool ShouldEnqueue(IWindow window, IWindowProperties newProperties)
		{
			// Don't enqueue if the Window is empty
			if (CurrentWindow == null && windowQueue.Count == 0)
			{
				return false;
			}

			// if the property intends to override default properties
			if (newProperties != null && newProperties.SuppressPrefabProperties)
			{
				return newProperties.QueuePriority != EWindowPriority.ForceForeground;
			}

			// enqueue if it doesn't HAVE to be at the foreground
			if (window.Properties.QueuePriority != EWindowPriority.ForceForeground)
			{
				// Debug.Log("Enqueueing screenID: " + window.ScreenID);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Pop history.
		/// </summary>
		public void Previous()
		{
			// validate stack not empty
			if (windowHistory.Count > 0)
				DoShow(windowHistory.Pop());
			// maybe count is 0 sometimes.
		}

		/// <summary>
		/// Dequeue Window.
		/// </summary>
		public void Next()
		{
			if (windowQueue.Count > 0)
				DoShow(windowQueue.Dequeue());
		}

		#endregion Screen History

		#region Event Responses

		private void OnInAnimationFinished(IUIScreen screen)
		{
			RemoveTransition(screen);
		}

		private void OnOutAnimationFinished(IUIScreen screen)
		{
			RemoveTransition(screen);

			var window = screen as IWindow;

			// darken screen if popup
			if (window.IsPopup)
				priorityParaLayer.RefreshDarken();
		}

		#endregion Event Responses

		#region Add/Remove Transitions

		private void AddTransition(IUIScreen screen)
		{
			screensTransitioning.Add(screen);
			frame.BlockScreens();
		}

		private void RemoveTransition(IUIScreen screen)
		{
			screensTransitioning.Remove(screen);
			if (!IsScreenTransitionInProgress)
				frame.UnblockScreens();
		}

		#endregion Add/Remove Transitions

		public bool IsWindowOpen(string screenID)
		{
			return registeredScreens.TryGetValue(screenID, out IWindow screen)
				&& screen.IsOpen;
		}

		public sealed override void ReparentScreen(IUIScreen screen,
			Transform screenTransform)
		{
			if (screen is IWindow window)
			{
				if (window.IsPopup)
				{
					priorityParaLayer.AddScreen(screenTransform);
					return; // don't reparent popups
				}
			}
			else
			{
				Debug.LogError($"Screen {screenTransform.name} is not a Window!", screenTransform);
			}

			base.ReparentScreen(screen, screenTransform);
		}
	}
}
