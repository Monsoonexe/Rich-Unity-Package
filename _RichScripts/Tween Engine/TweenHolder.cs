using UnityEngine.SceneManagement;

/// <summary>
/// I am a utility class to hold tweens so you don't have to!
/// </summary>
/// <seealso cref="ApexTweens"/>
public class TweenHolder : ApexMonoBehaviour
{
    private void Reset()
    { 
        SetDevDescription("I am a utility class to hold tweens" +
            " so you don't have to!");
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnLevelLoadedHandler;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLevelLoadedHandler;
    }

    private void OnLevelLoadedHandler(
        Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines();
    }
}
