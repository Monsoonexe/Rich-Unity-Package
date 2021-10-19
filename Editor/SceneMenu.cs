using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;

namespace RichPackage.Editor
{
    using Debug = UnityEngine.Debug;
    public static class SceneMenu
    {        
        private const string scriptFilePath = "Assets/RichPackage/Editor/SceneMenu.cs";

        #region Menu Items
        [MenuItem("Scenes/Open SceneMenu.cs")]
        public static void OpenSceneMenuScript()
        {
            var scriptFile = AssetDatabase.LoadAssetAtPath<MonoScript>(
                scriptFilePath);

            if(scriptFile == null)
            {
                //complain
                Debug.Log("SceneMenu.cs not found! Check path at "
                + scriptFilePath);
            }
            else
            {
                //open script in default IDE [Visual Studio]
                AssetDatabase.OpenAsset(scriptFile);
            }
        }

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
			if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				//actually change scenes
				EditorSceneManager.OpenScene(scenePath);
			}//if "cancel", do not change scenes.
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
