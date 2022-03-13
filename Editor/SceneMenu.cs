using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using ScriptableObjectArchitecture;

/* Make a new file called ProjectSceneMenu.cs for entries that are specific to
 * your project. This way you don't override this file in the git submodule, 
 * which affects ALL projects.
 * 
 * e.g.
	public static class BattleForgeSceneMenu
	{
		[MenuItem("Scenes/Main Game")]
		public static void LoadBonusMainGameScene()
			=> SceneMenu.LoadScene("Assets/Scenes/MainGameScene.unity");
	}
 * 
 */

namespace RichPackage.Editor
{
    public static class SceneMenu
    {        
        private const string scriptFilePath = "Assets/RichUnityPackage/Editor/SceneMenu.cs";

        #region Menu Items

        [MenuItem("Scenes/Open SceneMenu.cs")]
        public static void OpenSceneMenuScript()
        {
            var scriptFile = AssetDatabase.LoadAssetAtPath<MonoScript>(
                scriptFilePath);

            if(scriptFile == null)
            {
                //complain
                Debug.Log($"SceneMenu.cs not found! Check path at {scriptFilePath}." +
					$"You can also double-click this log message to open the file.");
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

		#region Functions

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
	}
}
