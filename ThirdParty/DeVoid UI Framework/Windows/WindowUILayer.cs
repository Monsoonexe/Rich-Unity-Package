using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <seealso cref="AWindowController"/>
/// <seealso cref="PanelUILayer"/>
public class WindowUILayer : AUILayer<IWindowController>
{
    [SerializeField, Required]
    private WindowParaLayer priorityParaLayer;

    public IWindowController CurrentWindow { get; private set; } // readonly

    public event Action RequestScreenBlock;

    public event Action RequestScreenUnblock;

    private List<IUIScreenController> screensTransitioning;
    private Queue<WindowHistoryEntry> windowQueue;
    private Stack<WindowHistoryEntry> windowHistory;

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

    protected override void ProcessScreenRegister(string screenID,
        IWindowController controller)
    {
        base.ProcessScreenRegister(screenID, controller);

        controller.OnTransitionInFinishedCallback += OnInAnimationFinished;
        controller.OnTransitionOutFinishedCallback += OnOutAnimationFinished;
        controller.CloseRequest += OnCloseRequestedByWindow;
    }

    protected override void ProcessScreenUnregister(string screenID,
        IWindowController controller)
    {
        base.ProcessScreenUnregister(screenID, controller);

        controller.OnTransitionInFinishedCallback -= OnInAnimationFinished;
        controller.OnTransitionOutFinishedCallback -= OnOutAnimationFinished;
        controller.CloseRequest -= OnCloseRequestedByWindow;
    }

    #endregion Process Screens

    #region Show/Hide Screens

    private void DoShow(IWindowController screen, IWindowProperties properties)
    {
        DoShow(new WindowHistoryEntry(screen, properties));
    }

    private void DoShow(WindowHistoryEntry entry)
    {
        // validate
        if (entry.screen == CurrentWindow)
        {
            Debug.LogError($"[{nameof(WindowUILayer)}] Requested windowID ({CurrentWindow.ScreenID}) " +
                "is already open! This causes undefined behavior as duplicate entries " +
                "are created.");
            return;
        }

        // hide current window?
        if (CurrentWindow != null // if there is already a Window
            && CurrentWindow.HideOnForegroundLost // and it will relinquish the foreground
            && !entry.screen.IsPopup) //and not a pop-up, which are always on top
        {
            CurrentWindow.Hide(true);
        }

        windowHistory.Push(entry);
        AddTransition(entry.screen);

        // darken everything else if popup
        if (entry.screen.IsPopup)
        {
            priorityParaLayer.DarkenBackground();
        }

        entry.Show(); // show it!

        CurrentWindow = entry.screen; //set as active window
    }

    public override void HideScreen(IWindowController screen, bool animate = true)
    {
        // verify screen being closed is current window
        if (screen != CurrentWindow)
        {
            Debug.LogError($"{nameof(WindowUILayer)}] Hide requested on WindowId {screen.ScreenID} "
                + $"but that's not the currently open one ({CurrentWindowID})! " +
                "Ignoring request.");
            return;
        }

        // remove from browser history
        _ = windowHistory.Pop();

        // do transition
        AddTransition(screen);

        // hide and animate
        screen.Hide(animate);

        // no active window
        CurrentWindow = null;

        // show next in queue
        if (windowQueue.Count > 0)
        {
            //Debug.LogFormat("if( windowQueue.Count ({0}) > 0", windowQueue.Count);
            ShowNextInQueue();
        }
        // show next from history
        else if (windowHistory.Count > 0)
        {
            //Debug.LogFormat("if( windowHistory.Count ({0}) > 0", windowHistory.Count);
            ShowPreviousInHistory();
        }
        // else all closed/hid
    }

    /// <summary>
    /// Close all Windows.
    /// </summary>
    /// <param name="animate"></param>
    public override void HideAll(bool animate = true)
    {
        base.HideAll(animate);
        CurrentWindow = null; // no active window
        priorityParaLayer.RefreshDarken();
        windowHistory.Clear();
    }

    /// <summary>
    /// Show a Screen with default data payload.
    /// </summary>
    /// <param name="screen"></param>
    public override void ShowScreen(IWindowController screen)
    {
        ShowScreen<IWindowProperties>(screen, null);
    }

    /// <summary>
    /// Show Screen and load with given data payload.
    /// </summary>
    /// <typeparam name="TProp"></typeparam>
    /// <param name="screen"></param>
    /// <param name="properties"></param>
    public override void ShowScreen<TProp>(IWindowController screen, TProp properties)
    {
        var windowProperties = properties as IWindowProperties;

        if (ShouldEnqueue(screen, windowProperties))
        {
            EnqueueWindow(screen, windowProperties);
        }
        else
        {
            DoShow(screen, windowProperties);
        }
    }

    #endregion Show/Hide Screens

    #region Screen History

    private void EnqueueWindow(IWindowController screen, IWindowProperties properties)
    {
        windowQueue.Enqueue(new WindowHistoryEntry(screen, properties));
    }

    private bool ShouldEnqueue(IWindowController controller, IWindowProperties windowProperties)
    {
        // Don't enqueue if the Window is empty
        if (CurrentWindow == null && windowQueue.Count == 0)
        {
            return false;
        }

        // if the property intends to override default properties
        if (windowProperties != null && windowProperties.SuppressPrefabProperties)
        {
            return windowProperties.WindowQueuePriority != WindowPriorityENUM.ForceForeground;
        }

        // enqueu if it doesn't HAVE to be at the foreground
        if (controller.WindowPriority != WindowPriorityENUM.ForceForeground)
        {
            Debug.Log("Enqueueing screenID: " + controller.ScreenID);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Pop history.
    /// </summary>
    private void ShowPreviousInHistory()
    {
        // validate stack not empty
        if (windowHistory.Count > 0)
        {
            DoShow(windowHistory.Pop());
        }
        // maybe count is 0 sometimes.
    }

    /// <summary>
    /// Dequeue Window.
    /// </summary>
    private void ShowNextInQueue()
    {
        if (windowQueue.Count > 0)
        {
            DoShow(windowQueue.Dequeue());
        }
    }

    #endregion Screen History

    #region Event Responses

    private void OnCloseRequestedByWindow(IUIScreenController screen,
        bool animate = true)
    {
        HideScreen(screen as IWindowController, animate);
    }

    private void OnInAnimationFinished(IUIScreenController screen)
    {
        RemoveTransition(screen);
    }

    private void OnOutAnimationFinished(IUIScreenController screen)
    {
        RemoveTransition(screen);

        var window = screen as IWindowController;

        //darken screen if popup
        if (window.IsPopup)
        {
            priorityParaLayer.RefreshDarken();
        }
    }

    #endregion Event Responses

    #region Add/Remove Transitions

    private void AddTransition(IUIScreenController screen)
    {
        screensTransitioning.Add(screen);
        RequestScreenBlock?.Invoke();
    }

    private void RemoveTransition(IUIScreenController screen)
    {
        screensTransitioning.Remove(screen);
        if (!IsScreenTransitionInProgress)
        {
            RequestScreenUnblock?.Invoke();
        }
    }

    #endregion Add/Remove Transitions

    public override void Initialize()
    {
        registeredScreens = new Dictionary<string, IWindowController>();
        windowQueue = new Queue<WindowHistoryEntry>();
        windowHistory = new Stack<WindowHistoryEntry>();
        screensTransitioning = new List<IUIScreenController>();
    }

    public override void ReparentScreen(IUIScreenController controller,
        Transform screenTransform)
    {
        if (controller is IWindowController window)
        {
            if (window.IsPopup)
            {
                priorityParaLayer.AddScreen(screenTransform);
                return; // don't reparent popups
            }
        }
        else
        {
            Debug.LogError($"Screen {screenTransform.name} is not a Window!");
        }

        base.ReparentScreen(controller, screenTransform);
    }
}
