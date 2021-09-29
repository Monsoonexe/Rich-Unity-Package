using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Signals;

/// <summary>
/// I control the application-level behaviour.
/// </summary>
/// <seealso cref="GameController"/>
public class RichAppController : RichMonoBehaviour
{
    private static RichAppController instance;
    public static RichAppController Instance => instance;

    /// <summary>
    /// Time.deltaTime that has been cached and un-marshalled.
    /// </summary>
    public static float DeltaTime { get; private set; }

    /// <summary>
    /// Time.time that has been cached and un-marshalled.
    /// </summary>
    public static float Time { get; private set; }
    
    public static float FixedDeltaTime { get; private set; }

    /// <summary>
    /// Another way to subscribe to SceneManager.sceneLoaded event.
    /// </summary>
    public event Action OnLevelLoaded;

    private void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
    {
        OnLevelLoaded?.Invoke();
    }

    private void Reset()
    {
        SetDevDescription("I control app-level stuff");
    }

    protected override void Awake()
    {
        base.Awake();
        instance = this;//low-key singleton
        GlobalSignals.Get<RequestQuitGameSignal>().AddListener(QuitGame);
        SceneManager.sceneLoaded += SceneLoadedHandler;
    }

    private void OnDestroy()
    {
        GlobalSignals.Get<RequestQuitGameSignal>().RemoveListener(QuitGame);
        SceneManager.sceneLoaded -= SceneLoadedHandler;
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
