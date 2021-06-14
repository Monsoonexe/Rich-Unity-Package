﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for UI Layers. Layers implement custom logic for Screens when opening, closing, etc.
/// </summary>
/// <seealso cref="PanelUILayer"/>
/// <seealso cref="WindowUILayer"/>
public abstract class AUILayer<TScreen> : RichMonoBehaviour 
    where TScreen : IUIScreenController
{

    /// <summary>
    /// Collection of Screens.
    /// </summary>
    protected Dictionary<string, TScreen> registeredScreens;

    /// <summary>
    /// Shows given Screen.
    /// </summary>
    /// <param name="screen"></param>
    public abstract void ShowScreen(TScreen screen);

    /// <summary>
    /// Shows given Screen and passes in given Properties.
    /// </summary>
    /// <typeparam name="TProps">Type of data payload</typeparam>
    /// <param name="screen"></param>
    /// <param name="properties">the data payload</param>
    public abstract void ShowScreen<TProps>(TScreen screen, TProps properties) 
        where TProps : IScreenProperties;

    /// <summary>
    /// Hides given Screen.
    /// </summary>
    /// <param name="screen"></param>
    public abstract void HideScreen(TScreen screen, bool animate = true);

    /// <summary>
    /// Inits this Layer.
    /// </summary>
    public virtual void Initialize()
    {
        registeredScreens = new Dictionary<string, TScreen>();
    }

    /// <summary>
    /// Reparents given Transform to this Layer.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="screenTransform"></param>
    public virtual void ReparentScreen(IUIScreenController controller, 
        Transform screenTransform)
    {   //it's okay to have parameters that don't get used this go around.
        screenTransform.SetParent(transform, false);
    }

    #region Screen Registration

    public bool IsScreenRegistered(string screenID)
    {
        return registeredScreens.ContainsKey(screenID);
    }

    /// <summary>
    /// Register a Screen this this Layer.
    /// </summary>
    /// <param name="screenID"></param>
    /// <param name="controller"></param>
    public void RegisterScreen(string screenID, TScreen controller)
    {
        if (!registeredScreens.ContainsKey(screenID))
        {
            ProcessScreenRegister(screenID, controller);
        }
        else
        {
            Debug.LogError("[AUILayer] ID already registered: " + screenID, this);
        }
    }

    public void UnregisterScreen(string screenID, TScreen controller)
    {
        if (registeredScreens.ContainsKey(screenID))
        {
            ProcessScreenUnregister(screenID, controller);
        }
        else
        {
            Debug.LogError("[AUILayer] Given screenID not registered to this Layer: " 
                + screenID, this);
        }
    }

    protected virtual void ProcessScreenRegister(string screenID, TScreen controller)
    {
        //give controller ID
        controller.ScreenID = screenID;

        //register
        registeredScreens.Add(screenID, controller);

        //subscribe to destroy events
        controller.OnScreenDestroyed += OnScreenDestroyed;
    }

    protected virtual void ProcessScreenUnregister(string screenID, TScreen controller)
    {
        controller.OnScreenDestroyed -= OnScreenDestroyed;
        registeredScreens.Remove(screenID);
    }

    #endregion

    #region Show/Hide Screens

    public void ShowScreenByID(string screenID)
    {
        TScreen controller; 

        if(registeredScreens.TryGetValue(screenID, out controller))
        {
            ShowScreen(controller);
        }
        else
        {
            Debug.LogError("[AUILayer] screenID: " + screenID 
                + " not registered to this Layer!", this);
        }
    }

    public void ShowScreenByID<TProps>(string screenID, TProps properties) 
        where TProps : IScreenProperties
    {
        TScreen controller;

        if (registeredScreens.TryGetValue(screenID, out controller))
        {
            ShowScreen(controller, properties);
        }
        else
        {
            Debug.LogError("[AUILayer] screenID: " + screenID 
                + " not registered to this Layer!", this);
        }

    }

    public void HideScreenByID(string screenID, bool animate = true)
    {
        TScreen controller;

        if (registeredScreens.TryGetValue(screenID, out controller))
        {
            HideScreen(controller, animate);
        }
        else
        {
            Debug.LogError("[AUILayer] screenID: " + screenID + " not registered to this Layer!", this);
        }
    }

    public virtual void HideAll(bool animateWhenHiding = true)
    {
        foreach(var screenEntry in registeredScreens)
        {
            screenEntry.Value.Hide(animateWhenHiding);
        }
    }

    #endregion

    private void OnScreenDestroyed(IUIScreenController screen)//why not just take a TScreen?
    {
        if (!string.IsNullOrEmpty(screen.ScreenID)
            && registeredScreens.ContainsKey(screen.ScreenID))
        {
            UnregisterScreen(screen.ScreenID, (TScreen)screen);
        }
    }
}
