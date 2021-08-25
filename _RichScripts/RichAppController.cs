﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Signals;

/// <summary>
/// 
/// </summary>
public class RichAppController : RichMonoBehaviour
{
    /// <summary>
    /// Time.deltaTime that has been cached and un-marshalled.
    /// </summary>
    public static float DeltaTime { get; private set; }

    /// <summary>
    /// Time.time that has been cached and un-marshalled.
    /// </summary>
    public static float Time { get; private set; }
    
    public static float FixedDeltaTime { get; private set; }

    private void Reset()
    {
        SetDevDescription("I control app-level stuff");
    }

    protected override void Awake()
    {
        GlobalSignals.Get<RequestQuitGameSignal>().AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        GlobalSignals.Get<RequestQuitGameSignal>().RemoveListener(QuitGame);
    }

    public void QuitGame()
    {
        GlobalSignals.Get<GameIsQuittingSignal>().Dispatch();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Update()
    {
        //cache time values to de-marshal them once
        DeltaTime = UnityEngine.Time.deltaTime;
        Time = UnityEngine.Time.time;
    }
    
    private void FixedUpdate()
    {
        //cache time values to de-marshal them once
        FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
    }

    public static void ReloadCurrentLevel()
        => SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
}

/// <summary>
/// Request that the app be closed.
/// </summary>
public class RequestQuitGameSignal : ASignal { }

/// <summary>
/// Dispatched when the Player has requested to close the app.
/// </summary>
public class GameIsQuittingSignal : ASignal { }