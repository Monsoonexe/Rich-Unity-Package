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

            var beh = TargetFloatVar.mantissaBehaviour 
                = (FloatVariable.EMantissaBehaviour)
                EditorGUILayout.EnumPopup("MantissaBehaviour", 
                TargetFloatVar.mantissaBehaviour);

            //draw decimal points
            if(!(beh == FloatVariable.EMantissaBehaviour.Default
                || beh == FloatVariable.EMantissaBehaviour.FloorToInt
                || beh == FloatVariable.EMantissaBehaviour.CeilingToInt
                || beh == FloatVariable.EMantissaBehaviour.RoundToInt))
            {
                EditorGUILayout.PropertyField(_decimalDigits);
                serializedObject.ApplyModifiedProperties();
            }
        }
        
    }
}
