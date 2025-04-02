// https://www.gamedeveloper.com/programming/a-ui-system-architecture-and-workflow-for-unity
// TODO - pull console stuff out

using QFSW.QC;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace RichPackage.UI.Framework
{
	/// <summary>
	/// This is the central hub for all things UI. All your calls should be directed at this.
	/// </summary>
	[SelectionBase,
		RequireComponent(typeof(Canvas)),
		RequireComponent(typeof(GraphicRaycaster)),
		DisallowMultipleComponent,
		CommandPrefix("UI.")]
	public class UIFrame : MonoBehaviour
	{
		private const string ButtonGroup = "Functions";

		[Tooltip("True if automatically initialized; False if manually initialized.")]
		public bool initializeOnAwake = true;

		[Title("Prefab Refs")]
		[SerializeField, Required]
		private PanelUILayer panelLayer;

		[SerializeField, Required]
		private WindowUILayer windowLayer;

		[SerializeField, Required]
		private Image raycastBlocker;

		[SerializeField, Required]
		private Canvas mainCanvas;

		[SerializeField, Required]
		private GraphicRaycaster myGraphicRaycaster;

		public Camera UICamera
		{
			get => mainCanvas.worldCamera;
			set => mainCanvas.worldCamera = value;
		}

		public Canvas Canvas => mainCanvas;

		/// <summary>
		/// Returns <see langword="true"/> if a Window is Transitioning.
		/// </summary>
		public bool IsTransitioning => windowLayer.IsScreenTransitionInProgress;

		public bool IsAnyWindowOpen => windowLayer.CurrentWindow != null;

		public IWindow CurrentWindow => windowLayer.CurrentWindow;

		#region Events

		public event Action OnFirstWindowOpened;
		public event Action OnAllWindowsClosed;

		#endregion Events

		#region Unity Messages

		protected void Reset()
		{
			mainCanvas = GetComponent<Canvas>();
			myGraphicRaycaster = GetComponent<GraphicRaycaster>();
			windowLayer = GetComponentInChildren<WindowUILayer>();
			panelLayer = GetComponentInChildren<PanelUILayer>();
		}

		protected void Awake()
		{
			// init others
			if (initializeOnAwake)
				Initialize();
		}

		protected void Start()
		{
			UnblockScreens();
		}

		protected void OnDestroy()
		{
			OnFirstWindowOpened = null;
			OnAllWindowsClosed = null;
		}

		#endregion Unity Messages

		public void Initialize()
		{
			panelLayer.Initialize(this);
			windowLayer.Initialize(this);

			// register screens that are already instantiated children
			IUIScreen[] screens = gameObject.GetComponentsInChildren<IUIScreen>(includeInactive: true);
			foreach (IUIScreen screen in screens)
			{
				RegisterScreen(screen.ScreenID, screen);
			}
		}

		/// <summary>
		/// Block UI interactability.
		/// </summary>
		internal void BlockScreens()
		{
			raycastBlocker.enabled = true; // block with a clear image
			myGraphicRaycaster.enabled = false; // disable raycast receiver for children of this canvas
		}

		/// <summary>
		/// Re-enable UI interactability.
		/// </summary>
		internal void UnblockScreens()
		{
			raycastBlocker.enabled = false;
			myGraphicRaycaster.enabled = true;
		}

		#region Window Interface

		public void CloseAllWindows(bool animate = true)
		{
			// ignore if no windows are open
			if (windowLayer.CurrentWindow == null)
				return;

			windowLayer.HideAll(animate); // relay
			OnAllWindowsClosed?.Invoke();
		}

		/// <summary>
		/// Why use this? It causes an error if it's not the current window.
		/// </summary>
		/// <seealso cref="CloseCurrentWindow"/>
		public void CloseWindow(string screenID, bool animate = true)
		{
			bool windowWasOpen = windowLayer.CurrentWindow != null;
			windowLayer.HideScreenByID(screenID, animate); // relay

			// raise event if the last window was closed
			if (windowWasOpen && windowLayer.CurrentWindow == null)
				OnAllWindowsClosed?.Invoke();
		}

		[Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
		public void CloseCurrentWindow(bool animate = true)
		{
			if (windowLayer.CurrentWindow != null)
			{
				//Debug.Log("Closing window: " + windowLayer.CurrentWindow.ScreenID);
				CloseWindow(windowLayer.CurrentWindow.ScreenID, animate); // relay
			}
		}

		/// <summary>
		/// Opens the Window with the given ID, with no Properties.
		/// </summary>
		[Button, DisableInEditorMode]
		public void OpenWindow(string screenID)
		{
			bool openingFirstWindow = windowLayer.CurrentWindow == null;
			windowLayer.ShowScreenByID(screenID);

			// raise event if was closed but now open
			if (openingFirstWindow && OnFirstWindowOpened != null)
				OnFirstWindowOpened();
		}

		/// <summary>
		/// Opens the Window and gives it Properties.
		/// </summary>
		public void OpenWindow(string screenID, IWindowProperties properties)
		{
			bool firstWindow = windowLayer.CurrentWindow == null;
			windowLayer.ShowScreenByID(screenID, properties);

			// raise event if was closed but now open
			if (firstWindow && OnFirstWindowOpened != null)
				OnFirstWindowOpened();
		}

		public bool IsWindowOpen(string screenID) => windowLayer.IsWindowOpen(screenID);

		#endregion Window Interface

		#region Panel Interface

		[Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
		public void HideAllPanels(bool animate = true)
		{
			panelLayer.HideAll(animate); // relay
		}

		/// <summary>
		/// Hide panel with given ID
		/// </summary>
		public void HidePanel(string screenID, bool animate)
		{
			panelLayer.HideScreenByID(screenID, animate); // relay
		}

		/// <summary>
		/// Hide panel with given ID
		/// </summary>
		[Button, DisableInEditorMode]
		public void HidePanel(string screenID)
		{
			panelLayer.HideScreenByID(screenID); // relay
		}

		/// <summary>
		/// Shows a panel by its ID, with no Properties.
		/// </summary>
		[Button, DisableInEditorMode]
		public void ShowPanel(string screenID)
		{
			panelLayer.ShowScreenByID(screenID); // relay
		}

		/// <summary>
		/// Shows a panel by its ID, giving Property Payload
		/// </summary>
		public void ShowPanel<TProps>(string screenID, TProps properties)
			where TProps : IPanelProperties
		{
			panelLayer.ShowScreenByID(screenID, properties); // relay
		}

		#endregion Panel Interface

		#region Screen Interface

		/// <summary>
		/// Hide all Panels and close all Windows.
		/// </summary>
		[Command("hide-all", MonoTargetType.Registry)]
		public void HideAll(bool animate = true)
		{
			CloseAllWindows(animate);
			HideAllPanels(animate);
		}

		/// <summary>
		/// Searches for a Window or Panel with given ID and opens it if found.
		/// </summary>
		[Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
		[Command("show", MonoTargetType.Registry)]
		public void ShowScreen([Console.ScreenID] string screenID)
		{
			if (!IsScreenRegistered(screenID, out Type type))
			{
				Debug.LogError($"Tried to {nameof(ShowScreen)}, but ID {screenID} is not registered.", this);
				return;
			}

			if (type == typeof(IWindow))
			{
				OpenWindow(screenID);
			}
			else if (type == typeof(IPanel))
			{
				ShowPanel(screenID);
			}
			else
			{
				Debug.LogError($"ScreenID is registered, but it {type} neither an {nameof(IWindow)} or {nameof(IPanel)}: {screenID}", this);
			}
		}

		[Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
		[Command("hide", MonoTargetType.Registry)]
		public void HideScreen([Console.ScreenID] string screenID, bool animate = true)
		{
			if (!IsScreenRegistered(screenID, out Type type))
			{
				Debug.LogError($"Tried to {nameof(ShowScreen)}, but ID {screenID} is not registered.", this);
				return;
			}

			if (type == typeof(IWindow))
			{
				CloseWindow(screenID, animate);
			}
			else if (type == typeof(IPanel))
			{
				HidePanel(screenID, animate);
			}
			else
			{
				Debug.LogError($"{nameof(screenID)} '{screenID}' is registered, " +
					$"but it is neither an {nameof(IWindow)} or " +
					$"{nameof(IPanel)}.", this);
			}
		}

		#endregion Screen Interface

		#region Screen Registration

		public void RegisterPanel<TPanel>(string screenID, TPanel panel)
			where TPanel : IPanel
		{
			panelLayer.RegisterScreen(screenID, panel);
		}

		public void RegisterWindow<TWindow>(string screenID, TWindow window)
			where TWindow : IWindow
		{
			windowLayer.RegisterScreen(screenID, window);
		}

		/// <summary>
		/// Generic "I don't know what this is, just register it".
		/// </summary>
		public void RegisterScreen(string screenID, IUIScreen screen,
			Transform screenTransform = null)
		{
			// screen is generic -- figure out if Panel or Window
			if (screen is IWindow window)
			{
				windowLayer.RegisterScreen(screenID, window); // register
				if (screenTransform)
					windowLayer.ReparentScreen(screen, screenTransform); // reparent
			}
			else if (screen is IPanel panel)
			{
				panelLayer.RegisterScreen(screenID, panel); // register
				if (screenTransform)
					panelLayer.ReparentScreen(screen, screenTransform); // reparent
			}
			else
			{
				Debug.LogError("[UIFrame] controller cannot be registered as it is neither a Panel nor Window: " + screenID, this);
			}
		}

		public void UnregisterPanel<TPanel>(string screenID, TPanel panel)
			where TPanel : IPanel
		{
			panelLayer.UnregisterScreen(screenID, panel);
		}

		public void UnregisterWindow<TWindow>(string screenID, TWindow window)
			where TWindow : IWindow
		{
			windowLayer.UnregisterScreen(screenID, window); // relay
		}

		#endregion Screen Registration

		#region Querries

		public bool IsPanelOpen(string screenID)
		{
			return panelLayer.IsPanelVisible(screenID); // relay
		}

		public bool IsScreenRegistered(string screenID)
		{
			return windowLayer.IsScreenRegistered(screenID)
				|| panelLayer.IsScreenRegistered(screenID);
		}

		public bool IsScreenRegistered(string screenID, out Type type)
		{
			if (windowLayer.IsScreenRegistered(screenID))
			{
				type = typeof(IWindow);
				return true;
			}

			if (panelLayer.IsScreenRegistered(screenID))
			{
				type = typeof(IPanel);
				return true;
			}

			type = null;
			return false;
		}

		[Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
		public void PrintRegisteredScreenIDs()
		{
			windowLayer.PrintRegisteredScreenIDs();
			panelLayer.PrintRegisteredScreenIDs();
		}

		public IEnumerable<string> GetRegisteredScreenIDs()
		{
			return windowLayer.GetRegisteredScreenIDs()
				.Concat(panelLayer.GetRegisteredScreenIDs());
		}

		#endregion Queries
	}
}
