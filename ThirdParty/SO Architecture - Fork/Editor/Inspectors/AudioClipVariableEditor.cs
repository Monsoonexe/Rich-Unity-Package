using UnityEditor;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(AudioClipVariable))]
    public class AudioClipVariableEditor : BaseVariableEditor
    {
        private SerializedProperty _audioClipOptions;

        protected override void OnEnable()
        {
            base.OnEnable();
            _audioClipOptions = serializedObject.FindProperty("defaultOptions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawValue();
            //DrawClampedFields(); //not clampable
            DrawOptions();
            EditorGUILayout.Space(5);
            DrawReadonlyField();
            DrawDeveloperDescription();
        }

        private void DrawOptions()
        {
            EditorGUILayout.PropertyField(_audioClipOptions);
        }
    }
}
