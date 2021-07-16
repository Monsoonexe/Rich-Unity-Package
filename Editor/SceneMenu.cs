using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

namespace RichPackage.Editor
{
    public static class SceneMenu
    {
        #region Menu Items

        [MenuItem("Scenes/Reload Current Scene")]
        public static void ReloadCurrentScene()
            => LoadScene(SceneManager.GetActiveScene());

        #endregion

        #region Internal Functions

        /// <summary>
        /// Load a scene and prompt User to save the scene.
        /// </summary>
        public static void LoadScene(string scenePath)
        {
            //prompt to save
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

            //actually change scenes
            EditorSceneManager.OpenScene(scenePath);
        }

        /// <summary>
        /// Load a scene and prompt User to save the scene.
        /// </summary>
        public static void LoadScene(Scene scene)
            => LoadScene(scene.path);

        /// <summary>
        /// Load a scene and prompt User to save the scene.
        /// </summary>
        public static void LoadScene(SceneVariable sceneVar)
            => LoadScene(sceneVar.Value.SceneName);

        #endregion

        //----------EXAMPLE ENTRY-----------
        //[MenuItem("Scenes/Start Menu Scene")]
        //private static void LoadStartMenuScene()
        //   => LoadScene("Assets/Scenes/StartMenu.unity");
    }

}
