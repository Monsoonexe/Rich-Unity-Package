using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using RichPackage;
using Sirenix.OdinInspector;
using QFSW.QC;
using System.Collections.Generic;

/// <summary>
/// This is the central hub for all things UI. All your calls should be directed at this.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
[SelectionBase]
[CommandPrefix("uiframe.")]
public class UIFrame : RichMonoBehaviour
{
    private const string ButtonGroup = "Functions";

    [Tooltip("True if automatically initialized, False if manually initialized.")]
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

    #region Events

    public event Action OnFirstWindowOpened;

    public event Action OnAllWindowsClosed;
    
    #endregion Events

    #region Unity Messages

    protected override void Reset()
	{
		base.Reset();
        SetDevDescription("This is the central hub for all things UI. All your calls should be directed at this.");
        mainCanvas = GetComponent<Canvas>();
        myGraphicRaycaster = GetComponent<GraphicRaycaster>();
        windowLayer = GetComponentInChildren<WindowUILayer>();
        panelLayer = GetComponentInChildren<PanelUILayer>();
	}

	protected override void Awake()
    {
        //init self
        base.Awake();
        mainCanvas = gameObject.GetComponentIfNull(mainCanvas);
        myGraphicRaycaster = gameObject.GetComponentIfNull(myGraphicRaycaster);

        //init others
        if (initializeOnAwake)
            Initialize();
    }

    protected void Start()
    {
        OnRequestScreenUnblock();
    }

	#endregion Unity Messages

	/// <summary>
	/// Block UI interactability
	/// </summary>
	private void OnRequestScreenBlock()
    {
        raycastBlocker.enabled = true; // block with a clear image
        myGraphicRaycaster.enabled = false; // disable raycast receiver for children of this canvas
    }

    /// <summary>
    /// Re-enable UI interactability
    /// </summary>
    private void OnRequestScreenUnblock()
    {
        raycastBlocker.enabled = false;
        myGraphicRaycaster.enabled = true;
    }

    public virtual void Initialize()
    {
        // init panel
        panelLayer.Initialize();

        // init window
        windowLayer.Initialize();
        windowLayer.RequestScreenBlock += OnRequestScreenBlock; // subscribe to events
        windowLayer.RequestScreenUnblock += OnRequestScreenUnblock; // subscribe to events
    }

    /// <summary>
    /// Returns <see langword="true"/> if a Window is Transitioning.
    /// </summary>
    public bool IsTransitioning => windowLayer.IsScreenTransitionInProgress;

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
        windowLayer.CurrentWindow.OnWindowOpen();

        // raise event if was closed but now open
        if (openingFirstWindow && OnFirstWindowOpened != null)
            OnFirstWindowOpened();
    }

    /// <summary>
    /// Opens the Window and gives it Properties.
    /// </summary>
    public void OpenWindow<TProps>(string screenID, TProps properties)
        where TProps : IWindowProperties
    {
        bool firstWindow = windowLayer.CurrentWindow == null;
        windowLayer.ShowScreenByID(screenID, properties);
        windowLayer.CurrentWindow.OnWindowOpen();

        // raise event if was closed but now open
        if (firstWindow && OnFirstWindowOpened != null)
            OnFirstWindowOpened();
    }

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
    public void ShowScreen([ScreenID] string screenID)
    {
        Type type;

        if (IsScreenRegistered(screenID, out type))
        {
            if (type == typeof(IWindowController))
            {
                OpenWindow(screenID);
            }
            else if (type == typeof(IPanelController))
            {
                ShowPanel(screenID);
            }
            else
            {
                Debug.LogError($"ScreenID is registered, but it is neither an IWindowController or IPanelController: {screenID}", this);
            }
        }
        else
        {
            Debug.LogError($"Tried to {nameof(ShowScreen)}, but ID {screenID} is not registered.", this);
        }
    }

    [Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
	[Command("hide", MonoTargetType.Registry)]
    public void HideScreen(string screenID)
	{
        Type type;

        if (IsScreenRegistered(screenID, out type))
        {
            if (type == typeof(IWindowController))
            {
                CloseWindow(screenID);
            }
            else if (type == typeof(IPanelController))
            {
                HidePanel(screenID);
            }
            else
            {
                Debug.LogError($"ERROR! {nameof(screenID)} <{screenID}> is registered, " +
					$"but it is neither an {nameof(IWindowController)} or " +
					$"{nameof(IPanelController)}.", this);
            }
        }
        else
        {
            Debug.LogError($"ERROR! Tried to {nameof(ShowScreen)}, but ID {screenID} is not registered.", this);
        }
    }

    #endregion Screen Interface

	#region Screen Registration

	public void RegisterPanel<TPanel>(string screenID, TPanel controller)
        where TPanel : IPanelController
    {
        panelLayer.RegisterScreen(screenID, controller);
    }

    public void RegisterWindow<TWindow>(string screenID, TWindow controller)
        where TWindow : IWindowController
    {
        windowLayer.RegisterScreen(screenID, controller);
    }

    /// <summary>
    /// Generic "I don't know what this is, just register it".
    /// </summary>
    /// <param name="screenID"></param>
    /// <param name="controller"></param>
    /// <param name="screenTransform"></param>
    public void RegisterScreen(string screenID, 
        IUIScreenController controller, Transform screenTransform)
    {
        //screen is generic -- figure out if Panel or Window
        if(controller is IWindowController)
        {
            var windowController = controller as IWindowController;

            windowLayer.RegisterScreen(screenID, windowController); // register
            if (screenTransform)
            {
                windowLayer.ReparentScreen(controller, screenTransform); // reparent
            }
        }
        else if(controller is IPanelController)
        {
            var panelController = controller as IPanelController;

            panelLayer.RegisterScreen(screenID, panelController); // register
            if (screenTransform)
            {
                panelLayer.ReparentScreen(controller, screenTransform); // reparent
            }
        }
        else
        {
            Debug.LogError("[UIFrame] controller cannot be registered as it is neither a Panel nor Window: " + screenID, this);
        }
    }

    public void UnregisterPanel<TPanel>(string screenID, TPanel controller)
        where TPanel : IPanelController
    {
        panelLayer.UnregisterScreen(screenID, controller); // realy
    }

    public void UnregisterWindow<TWindow>(string screenID, TWindow controller)
        where TWindow : IWindowController
    {
        windowLayer.UnregisterScreen(screenID, controller); // relay
    }

    #endregion Screen Registration

    #region Querries

    public bool IsPanelOpen(string screenID)
    {
        return panelLayer.IsPanelVisible(screenID); // relay
    }

    public bool IsScreenRegistered(string screenID)
    {
        return (windowLayer.IsScreenRegistered(screenID))
            || (panelLayer.IsScreenRegistered(screenID));
    }

    public bool IsScreenRegistered(string screenID, out Type type)
    {
        if (windowLayer.IsScreenRegistered(screenID))
        {
            type = typeof(IWindowController);
            return true;
        }
        else if (panelLayer.IsScreenRegistered(screenID))
        {
            type = typeof(IPanelController);
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
