using UnityEditor;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomPropertyDrawer(typeof(SceneInfo))]
    internal sealed class SceneInfoPropertyDrawer : PropertyDrawer
    {
        private const string SCENE_PREVIEW_TITLE = "Preview (Read-Only)";
        private const string SCENE_PATH_PROPERTY = "_scenePath";
        private const string SCENE_INDEX_PROPERTY = "_sceneIndex";
        private const string SCENE_ENABLED_PROPERTY = "_isSceneEnabled";
        private const string SCENE_NAME_PROPERTY = "_sceneName";
        private const string SCENE_DESCRIPTION_PROPERTY = "_sceneDescription";
        private const string SCENE_ICON_PROPERTY = "_icon";
        private const int FIELD_COUNT = 7;

        public override void OnGUI(Rect propertyRect, SerializedProperty property, GUIContent label)
        {
            var scenePathProperty = property.FindPropertyRelative(SCENE_PATH_PROPERTY);
            var sceneIndexProperty = property.FindPropertyRelative(SCENE_INDEX_PROPERTY);
            var enabledProperty = property.FindPropertyRelative(SCENE_ENABLED_PROPERTY);
            var nameProperty = property.FindPropertyRelative(SCENE_NAME_PROPERTY);
            var descriptionProperty = property.FindPropertyRelative(SCENE_DESCRIPTION_PROPERTY);
            var iconProperty = property.FindPropertyRelative(SCENE_ICON_PROPERTY);

            EditorGUI.BeginProperty(propertyRect, new GUIContent(property.displayName), property);
            EditorGUI.BeginChangeCheck();

            // Draw Object Selector for SceneAssets
            var sceneAssetRect = new Rect
            {
                position = propertyRect.position,
                size = new Vector2(propertyRect.width,
                    EditorGUIUtility.singleLineHeight)
            };

            var oldSceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(
                scenePathProperty.stringValue);
            var sceneAsset = EditorGUI.ObjectField(sceneAssetRect, oldSceneAsset, typeof(SceneAsset), false);
            var sceneAssetPath = AssetDatabase.GetAssetPath(sceneAsset);
            if (scenePathProperty.stringValue != sceneAssetPath)
            {
                scenePathProperty.stringValue = sceneAssetPath;
            }

            if (string.IsNullOrEmpty(scenePathProperty.stringValue))
            {
                sceneIndexProperty.intValue = -1;
                enabledProperty.boolValue = false;
            }

            // Draw preview fields for scene information.
            var titleLabelRect = sceneAssetRect;
            titleLabelRect.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(titleLabelRect, SCENE_PREVIEW_TITLE);
            EditorGUI.BeginDisabledGroup(true);
            var pathRect = titleLabelRect;
            pathRect.y += EditorGUIUtility.singleLineHeight;

            var indexRect = pathRect;
            indexRect.y += EditorGUIUtility.singleLineHeight;

            var enabledRect = indexRect;
            enabledRect.y += EditorGUIUtility.singleLineHeight;

            var nameRect = enabledRect;
            nameRect.y += EditorGUIUtility.singleLineHeight;

            var descriptionRect = nameRect;
            descriptionRect.y += EditorGUIUtility.singleLineHeight;

            var iconRect = descriptionRect;
            iconRect.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(pathRect, scenePathProperty);
            EditorGUI.PropertyField(indexRect, sceneIndexProperty);
            EditorGUI.PropertyField(enabledRect, enabledProperty);
            EditorGUI.EndDisabledGroup();
            EditorGUI.PropertyField(nameRect, nameProperty);
            EditorGUI.PropertyField(descriptionRect, descriptionProperty);
            EditorGUI.PropertyField(iconRect, iconProperty);
            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(
            SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * FIELD_COUNT;
        }
    }
}
