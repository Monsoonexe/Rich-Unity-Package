using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

namespace RichPackage.Editor
{
    public static class SceneMenu
    {
        [MenuItem("Scenes/Reload Current Scene")]
        public static void ReloadCurrentScene()
            => LoadScene(SceneManager.GetActiveScene());

        public static void LoadScene(Scene scene)
            => LoadScene(scene.path);

        public static void LoadScene(string scenePath)
        {
            //prompt to save
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            //actually change scenes
            EditorSceneManager.OpenScene(scenePath);
        }

        public static void LoadScene(SceneVariable sceneVar)
            => LoadScene(sceneVar.Value.SceneName);

        //----------EXAMPLE ENTRY-----------
        //[MenuItem("Scenes/Start Menu Scene")]
        //private static void LoadStartMenuScene()
        //   => LoadScene("Assets/Scenes/StartMenu.unity");
    }

}
