using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityConsole;

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

    [Button, DisableInEditorMode]
    public void LoadLevel(SceneVariable sceneVariable)
        => SceneManager.LoadScene(sceneVariable.Value.SceneIndex);

    [Button, DisableInEditorMode]
    public void LoadLevel(string name)
        => SceneManager.LoadScene(name);

    [Button, DisableInEditorMode]
    public void LoadLevel(int index)
        => SceneManager.LoadScene(index);

    #region Static Interface

    [Button("LoadLevelImmediately"), DisableInEditorMode, ConsoleCommand("loadLevel")]
    public static void LoadLevel_(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    [Button, DisableInEditorMode, ConsoleCommand("loadNextLevel")]
    public static void LoadNextLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    [Button, DisableInEditorMode, ConsoleCommand("loadPreviousLevel")]
    public static void LoadPreviousLevel()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex - 1);
    }

    [Button, DisableInEditorMode, ConsoleCommand("reloadLevel")]
    public static void ReloadCurrentLevel()
        => SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);

    #endregion
}
