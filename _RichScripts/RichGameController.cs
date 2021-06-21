using UnityEngine;
using UnityEngine.SceneManagement;
using Signals;

/// <summary>
/// 
/// </summary>
public class RichGameController : RichMonoBehaviour
{
    protected override void Awake()
    {
        GlobalSignals.Get<RequestQuitGameSignal>().AddListener(QuitGame);
    }

    private void OnDestroy()
    {
        GlobalSignals.Get<RequestQuitGameSignal>().RemoveListener(QuitGame);
    }

    private void OnEnable()
    {
        
    }

    public void QuitGame()
    {
        GlobalSignals.Get<GameIsQuittingSignal>().Dispatch();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public static void ReloadCurrentLevel()
        => SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);

}

/// <summary>
/// Request that the app be closed.
/// </summary>
public class RequestQuitGameSignal : ASignal { }

/// <summary>
/// Dispatched when the Player has requested to close the app.
/// </summary>
public class GameIsQuittingSignal : ASignal { }
