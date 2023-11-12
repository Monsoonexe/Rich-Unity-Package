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
    public static partial class SceneMenu
    {        
        public const string ScriptFilePath = "Assets/RichPackage/Editor/SceneMenu.cs";
        public const string MenuPath = "Scenes/";

        #region Menu Items

        [MenuItem(MenuPath + "Open SceneMenu.cs", priority = -5)]
        public static void OpenSceneMenuScript()
        {
            var scriptFile = AssetDatabase.LoadAssetAtPath<MonoScript>(
                ScriptFilePath);

            if(scriptFile == null)
            {
                //complain
                Debug.LogWarning($"SceneMenu.cs not found! Check path at {ScriptFilePath}." +
					$"You can also double-click this log message to open the file.");
            }
            else
            {
                //open script in default IDE [Visual Studio]
                AssetDatabase.OpenAsset(scriptFile);
            }
        }

        [MenuItem(MenuPath + "Reload Current Scene", priority = -5)]
        public static void ReloadCurrentScene()
            => LoadScene(SceneManager.GetActiveScene());

        #endregion Menu Items

        #region Functions

        /// <summary>
        /// Load a scene and prompt User to save the scene.
        /// </summary>
        public static void LoadScene(string scenePath)
		{
            if (!System.IO.Path.HasExtension(scenePath))
                scenePath += ".unity";
                
			// prompt to save
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

        #endregion Functions
    }
}
