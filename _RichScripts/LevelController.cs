using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

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

    public void LoadNextLevel_()
        => LoadNextLevel();//forward call to static member

    public void LoadPreviousLevel_()
        => LoadPreviousLevel();//forward call to static member

    public void ReloadCurrentLevel_()
        => ReloadCurrentLevel();//forward call to static member

    public static void LoadNextLevel()
    {
        ++currentSceneIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public static void LoadPreviousLevel()
    {
        --currentSceneIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public static void ReloadCurrentLevel()
        => SceneManager.LoadScene(
            SceneManager.GetActiveScene().name);
}
