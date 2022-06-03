using System;
using UnityEngine;
using UnityEngine.UI;
using RichPackage;
using Sirenix.OdinInspector;

/// <summary>
/// This is the central hub for all things UI. All your calls should be directed at this.
/// </summary>
[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
[SelectionBase]
public class UIFrame : RichMonoBehaviour
{
    private const string ButtonGroup = "bg";

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

    public Camera UICamera { get => mainCanvas.worldCamera; }

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
        //init panel
        panelLayer.Initialize();

        //init window
        windowLayer.Initialize();
        windowLayer.RequestScreenBlock += OnRequestScreenBlock; // subscribe to events
        windowLayer.RequestScreenUnblock += OnRequestScreenUnblock; // subscribe to events
    }

    /// <summary>
    /// Returns true if a Window is Transitioning.
    /// </summary>
    /// <returns></returns>
    public bool IsTransitioning()
    {
        //what about panelLayer?
        return windowLayer.IsScreenTransitionInProgress;
    }

    #region Hide/Show Windows

    public void CloseAllWindows(bool animate = true)
    {
        windowLayer.HideAll(animate); // relay
    }

    /// <summary>
    /// Why use this? It causes an error if it's not the current window.
    /// </summary>
    /// <param name="screenID"></param>
    /// <seealso cref="CloseCurrentWindow"/>
    public void CloseWindow(string screenID, bool animate = true)
    {
        windowLayer.HideScreenByID(screenID, animate); // relay
    }

    public void CloseCurrentWindow(bool animate = true)
    {
        if (windowLayer.CurrentWindow != null)
        {
            //Debug.Log("Closing window: " + windowLayer.CurrentWindow.ScreenID);
            CloseWindow(windowLayer.CurrentWindow.ScreenID, animate); // relay
        }
        //else
        //{
        //    Debug.Log("No current window!");
        //}
    }

    /// <summary>
    /// Opens the Window with the given ID, with no Properties
    /// </summary>
    /// <param name="screenID"></param>
    public void OpenWindow(string screenID)
    {
        windowLayer.ShowScreenByID(screenID); // relay
        windowLayer.CurrentWindow.OnWindowOpen();
    }

    /// <summary>
    /// Opens the Window and gives it Properties.
    /// </summary>
    /// <typeparam name="TProps"></typeparam>
    /// <param name="screenID"></param>
    /// <param name="properties"></param>
    public void OpenWindow<TProps>(string screenID, TProps properties)
        where TProps : IWindowProperties
    {
        windowLayer.ShowScreenByID(screenID, properties);
        windowLayer.CurrentWindow.OnWindowOpen();
    }

    #endregion

    #region Hide/Show Panels

    [Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
    public void HideAllPanels(bool animate = true)
    {
        panelLayer.HideAll(animate); // relay
    }

    /// <summary>
    /// Hide panel with given ID
    /// </summary>
    /// <param name="screenID"></param>
    public void HidePanel(string screenID, bool animate)
    {
        panelLayer.HideScreenByID(screenID, animate); // relay
    }

    /// <summary>
    /// Hide panel with given ID
    /// </summary>
    /// <param name="screenID"></param>
    [Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
    public void HidePanel(string screenID)
    {
        panelLayer.HideScreenByID(screenID); // relay
    }

    /// <summary>
    /// Shows a panel by its ID, with no Properties.
    /// </summary>
    /// <param name="screenID"></param>
    [Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
    public void ShowPanel(string screenID)
    {
        panelLayer.ShowScreenByID(screenID); // relay
    }

    /// <summary>
    /// Shows a panel by its ID, giving Property Payload
    /// </summary>
    /// <typeparam name="TProps"></typeparam>
    /// <param name="screenID"></param>
    /// <param name="properties"></param>
    /// <seealso cref="IPanelProperties"/>
    public void ShowPanel<TProps>(string screenID, TProps properties)
        where TProps : IPanelProperties
    {
        panelLayer.ShowScreenByID(screenID, properties); // relay
    }

    #endregion

    /// <summary>
    /// Hide all Panels and close all Windows.
    /// </summary>
    /// <param name="animate"></param>
    public void HideAll(bool animate = true)
    {
        CloseAllWindows(animate);
        HideAllPanels(animate);
    }

    /// <summary>
    /// Searches for a Window or Panel with given ID and opens it if found.
    /// </summary>
    /// <param name="screenID"></param>
    [Button, DisableInEditorMode, FoldoutGroup(ButtonGroup)]
    public void ShowScreen(string screenID)
    {
        Type type;

        if (IsScreenRegistered(screenID, out type))
        {
            if (type is IWindowController)
            {
                OpenWindow(screenID);
            }
            else if (type is IPanelController)
            {
                ShowPanel(screenID);
            }
            else
            {
                Debug.LogError($"ERROR! ScreenID is registered, but it is neither an IWindowController or IPanelController: {screenID}", this);
            }
        }
        else
        {
            Debug.LogError($"ERROR! Tried to {nameof(ShowScreen)}, but ID {screenID} is not registered.", this);
        }
    }

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


    #endregion

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="screenID"></param>
    /// <param name="type"></param>
    /// <returns></returns>
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

    #endregion

}
