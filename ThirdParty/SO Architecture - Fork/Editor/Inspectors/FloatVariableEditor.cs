using UnityEditor;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(FloatVariable))]
    public class FloatVariableEditor : BaseVariableEditor
    {
        private SerializedProperty _mantissaBehaviour;
        private SerializedProperty _decimalDigits;
        private FloatVariable TargetFloatVar { get => (FloatVariable)target; }

        protected override void OnEnable()
        {
            base.OnEnable();
            _mantissaBehaviour = serializedObject.FindProperty("mantissaBehaviour");
            _decimalDigits = serializedObject.FindProperty("decimalDigits");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawValue();
            DrawClampedFields();
            DrawReadonlyField();
            DrawMantissaBehaviour();
            DrawDeveloperDescription();
        }

        private void DrawMantissaBehaviour()
        {
            if (TargetFloatVar.ReadOnly) return;

            var behaviour = TargetFloatVar.mantissaBehaviour 
                = (FloatVariable.EMantissaBehaviour)
                EditorGUILayout.EnumPopup("MantissaBehaviour", 
                TargetFloatVar.mantissaBehaviour);

            //draw decimal points
            if(behaviour != FloatVariable.EMantissaBehaviour.Default)
            {
                using(var scope = new EditorGUI.ChangeCheckScope())
                {
                    EditorGUILayout.PropertyField(_decimalDigits);//draw 
                    if (scope.changed)
                    {
                        serializedObject.ApplyModifiedProperties();
                        TargetFloatVar.Value = TargetFloatVar.Value; //force update
                    }
                }
            }
        }
    }
}
