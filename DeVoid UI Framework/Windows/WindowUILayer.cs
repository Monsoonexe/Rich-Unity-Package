using System;
using System.Collections.Generic;
using UnityEngine;

public class WindowUILayer : AUILayer<IWindowController>
{
    [SerializeField]
    private WindowParaLayer priorityParaLayer;

    public IWindowController CurrentWindow { get; private set; } // readonly

    [SerializeField]
    private string currentWindowID = "none";

    private Queue<WindowHistoryEntry> windowQueue;
    private Stack<WindowHistoryEntry> windowHistory;

    public event Action RequestScreenBlock;

    public event Action RequestScreenUnblock;

    private List<IUIScreenController> screensTransitioning;

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

    #endregion

    #region Show/Hide Screens

    private void DoShow(IWindowController screen, IWindowProperties properties)
    {
        DoShow(new WindowHistoryEntry(screen, properties));
    }

    private void DoShow(WindowHistoryEntry entry)
    {
        //validate
        if(entry.screen == CurrentWindow)
        {
            Debug.LogErrorFormat("[WindowUILayer] Requested windowID ({0}) " + 
                "is already open! This causes undefined behavior as duplicate entries " + 
                "are created.", CurrentWindow.ScreenID);
            return;
        }
        else if(CurrentWindow != null // if there is already a Window
            && CurrentWindow.HideOnForegroundLost // and it will relinquish the foreground
            && !entry.screen.IsPopup) //and not a pop-up, which are always on top
        {
            CurrentWindow.Hide(true);
        }
        
        windowHistory.Push(entry);
        AddTransition(entry.screen);

        //darken everything else if popup
        if (entry.screen.IsPopup)
        {
            priorityParaLayer.DarkenBackground();
        }

        entry.Show(); // show it!

        CurrentWindow = entry.screen;//set as active window
        currentWindowID = CurrentWindow.ScreenID;
    }

    public override void HideScreen(IWindowController screen, bool animate = true)
    {
        //verify screen being closed is current window
        if(screen == CurrentWindow)
        {
            //remove from browser history
            var removedWindowEntry = windowHistory.Pop();

            //do transition
            AddTransition(screen);

            //hide and animate
            screen.Hide(animate);

            //no active window
            CurrentWindow = null;
            currentWindowID = null;

            //show next in queue
            if(windowQueue.Count > 0)
            {
                //Debug.LogFormat("if( windowQueue.Count ({0}) > 0", windowQueue.Count);
                ShowNextInQueue();
            }
            //show next from history
            else if(windowHistory.Count > 0)
            {
                //Debug.LogFormat("if( windowHistory.Count ({0}) > 0", windowHistory.Count);
                ShowPreviousInHistory();
            }
            // else all closedHid
        }
        else
        {
            Debug.LogErrorFormat("[WindowUILayer] Hide requested on WindowId {0} "
                + "but that's not the currently open one ({1})! Ignoring request.",
                screen.ScreenID, CurrentWindow != null ? CurrentWindow.ScreenID 
                : "current is null.");
        }
    }

    /// <summary>
    /// Close all Windows.
    /// </summary>
    /// <param name="animate"></param>
    public override void HideAll(bool animate = true)
    {
        base.HideAll(animate);

        CurrentWindow = null; // no active window
        currentWindowID = null;

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
            EnqueueWindow(screen, properties);
        }
        else
        {
            DoShow(screen, windowProperties);
        }
    }

    #endregion

    #region Screen History

    private void EnqueueWindow<TProp>(IWindowController screen, TProp properties)
        where TProp : IScreenProperties
    {
        windowQueue.Enqueue(new WindowHistoryEntry(screen, (IWindowProperties)properties));
    }

    private bool ShouldEnqueue(IWindowController controller, IWindowProperties windowProperties)
    {
        //Don't enqueue if the Window is empty
        if(CurrentWindow == null && windowQueue.Count == 0)
        {
            return false;
        }

        //if the property intends to override default properties
        if(windowProperties != null && windowProperties.SuppressPrefabProperties)
        {
            return windowProperties.WindowQueuePriority != WindowPriorityENUM.ForceForeground;
        }

        //enqueu if it doesn't HAVE to be at the foreground
        if(controller.WindowPriority != WindowPriorityENUM.ForceForeground)
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
        //validate stack not empty
        if(windowHistory.Count > 0)
        {
            DoShow(windowHistory.Pop());
        }
        //maybe count is 0 sometimes.
    }

    /// <summary>
    /// Dequeue Window.
    /// </summary>
    private void ShowNextInQueue()
    {
        if(windowQueue.Count > 0)
        {
            DoShow(windowQueue.Dequeue());
        }
    }

    #endregion

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

    #endregion

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

    #endregion

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
        IWindowController window = controller as IWindowController;

        if (window != null) // if window exists
        {
            if (window.IsPopup)
            {
                priorityParaLayer.AddScreen(screenTransform);
                return; // don't reparent popups
            }
        }
        else
        {
            Debug.LogErrorFormat("ERROR! Screen {0} is not a Window!", screenTransform.name);

        }

        base.ReparentScreen(controller, screenTransform);
    }

}
