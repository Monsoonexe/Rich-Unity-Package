using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

/// <summary>
/// 
/// </summary>
public class LevelController : RichMonoBehaviour
{
    public static int currentSceneIndex = 0;

    //member functions for events to hook onto.

    public void LoadLevel(SceneVariable sceneVariable)
        => SceneManager.LoadScene(sceneVariable.Value.SceneIndex);

    public void LoadLevel(string name)
        => SceneManager.LoadScene(name);

    public void LoadLevel(int index)
        => SceneManager.LoadScene(index);

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    [Button]
    public static void LoadNextLevel()
    {
        ++currentSceneIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    [Button]
    public static void LoadPreviousLevel()
    {
        --currentSceneIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    [Button]
    public static void ReloadCurrentLevel()
        => SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
}
