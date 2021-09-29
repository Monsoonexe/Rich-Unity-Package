using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

/// <summary>
/// I control the events and behaviour of this specifc level.
/// </summary>
public class LevelController : RichMonoBehaviour
{
    //member functions for events to hook onto.

    private void Reset()
    {
        SetDevDescription("I control the events and behaviour of this specifc level.");
    }

    public void LoadLevel(SceneVariable sceneVariable)
        => SceneManager.LoadScene(sceneVariable.Value.SceneIndex);

    public void LoadLevel(string name)
        => SceneManager.LoadScene(name);

    public void LoadLevel(int index)
        => SceneManager.LoadScene(index);

    [Button(null, EButtonEnableMode.Playmode)]
    public static void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    [Button(null, EButtonEnableMode.Playmode)]
    public static void LoadPreviousLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }

    [Button(null, EButtonEnableMode.Playmode)]
    public static void ReloadCurrentLevel()
        => SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
}
