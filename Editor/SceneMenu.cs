using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class SceneMenu
{
    [MenuItem("Scenes/Reload Current Scene")]
    public static void ReloadCurrentScene()
        => LoadScene(SceneManager.GetActiveScene());

    public static void LoadScene(Scene scene)
        => LoadScene(scene.path);

    public static void LoadScene(string scenePath)
        => EditorSceneManager.OpenScene(scenePath);

    //e.g. 
    //[MenuItem("Scenes/Start Menu Scene")]
    //private static void LoadStartMenuScene()
    //   => LoadScene("Assets/Scenes/StartMenu.unity");
}
