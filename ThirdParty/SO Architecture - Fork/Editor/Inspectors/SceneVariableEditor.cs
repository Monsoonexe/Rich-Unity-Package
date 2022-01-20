using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(SceneVariable))]
    internal sealed class SceneVariableEditor : BaseVariableEditor
    {
        // UI
        private const string SCENE_NOT_ASSIGNED_WARNING = "Please assign a scene as the current serialized values for " +
                                             "the scene do not resolve to an asset in the project.";
        private const string SCENE_NOT_IN_BUILD_SETTINGS_WARNING =
            "Scene assigned is not currently in the Build Settings";
        private const string SCENE_NOT_ENABLED_IN_BUILD_SETTINGS_WARNING =
            "Scene assigned is present in build settings, but not enabled.";

        // Serialized Properties
        private const string SCENE_INFO_PROPERTY = "_value";

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawValue();
			EditorGUILayout.Space(15);
			DrawDeveloperDescription();
			EditorGUILayout.Space(10);
			var sceneVariable = (SceneVariable)target;
			//button to open scene
			if (!string.IsNullOrEmpty(sceneVariable.Value.ScenePath))
			{
				if (!Application.isPlaying && GUILayout.Button("Open Scene"))
				{
					//prompt to save
					if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					{
						//actually change scenes
						EditorSceneManager.OpenScene(sceneVariable.Value.ScenePath);
					}//if "cancel", do not change scenes.
				}
			}
		}
        protected override void DrawValue()
        {
            var sceneVariable = (SceneVariable)target;
            var sceneInfoProperty = serializedObject.FindProperty(SCENE_INFO_PROPERTY);
            if (sceneVariable.Value.Scene == null)
            {
                EditorGUILayout.HelpBox(SCENE_NOT_ASSIGNED_WARNING, MessageType.Warning);
			}
			else if (!sceneVariable.Value.IsSceneEnabled)
			{
				EditorGUILayout.HelpBox(SCENE_NOT_ENABLED_IN_BUILD_SETTINGS_WARNING, MessageType.Warning);
			}
			else if (!sceneVariable.Value.IsSceneInBuildSettings)
            {
                EditorGUILayout.HelpBox(SCENE_NOT_IN_BUILD_SETTINGS_WARNING, MessageType.Warning);
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(sceneInfoProperty);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        public override bool RequiresConstantRepaint()
        {
            return true;
        }
    }
}