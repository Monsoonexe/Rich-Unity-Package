using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

/// <summary>
/// 
/// </summary>
public class LevelController : RichMonoBehaviour
{
    //member functions for events to hook onto.

    public void LoadLevel(SceneVariable sceneVariable)
        => SceneManager.LoadScene(sceneVariable.Value.SceneIndex);

    public void LoadLevel(string name)
        => SceneManager.LoadScene(name);

    public void LoadLevel(int index)
        => SceneManager.LoadScene(index);

    [Button]
    public static void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    [Button]
    public static void LoadPreviousLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }

    [Button]
    public static void ReloadCurrentLevel()
        => SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
}
