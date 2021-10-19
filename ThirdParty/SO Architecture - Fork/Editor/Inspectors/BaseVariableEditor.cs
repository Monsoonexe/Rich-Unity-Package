using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(BaseVariable<>), true)]
    public class BaseVariableEditor : UnityEditor.Editor
    {
        private BaseVariable Target { get { return (BaseVariable)target; } }
        protected bool IsClampable { get { return Target.Clampable; } }
        protected bool IsClamped { get { return Target.IsClamped; } }
        protected bool IsInitializeable { get => Target.IsInitializeable; }

        private SerializedProperty _valueProperty;
        private SerializedProperty _initializeValueProperty;
        private SerializedProperty _initialize;
        private SerializedProperty _developerDescription;
        private SerializedProperty _readOnly;
        private SerializedProperty _raiseWarning;
        private SerializedProperty _isClamped;
        private SerializedProperty _minValueProperty;
        private SerializedProperty _maxValueProperty;

        //animations
        private AnimBool _raiseWarningAnimation;
        private AnimBool _isClampedVariableAnimation;
        private AnimBool _isInitializeableAnimation;

        private const string READONLY_TOOLTIP = "Should this value be changable during runtime? Will still be editable in the inspector regardless";

        protected virtual void OnEnable()
        {
            _developerDescription = serializedObject.FindProperty("DeveloperDescription");
            _valueProperty = serializedObject.FindProperty("_value");
            _initializeValueProperty = serializedObject.FindProperty("_initialValue");
            _initialize = serializedObject.FindProperty("_initialize");
            _readOnly = serializedObject.FindProperty("_readOnly");
            _raiseWarning = serializedObject.FindProperty("_raiseWarning");
            _isClamped = serializedObject.FindProperty("_isClamped");
            _minValueProperty = serializedObject.FindProperty("_minClampedValue");
            _maxValueProperty = serializedObject.FindProperty("_maxClampedValue");

            _raiseWarningAnimation = new AnimBool(_readOnly.boolValue);
            _raiseWarningAnimation.valueChanged.AddListener(Repaint);

            _isClampedVariableAnimation = new AnimBool(_isClamped.boolValue);
            _isClampedVariableAnimation.valueChanged.AddListener(Repaint);

            _isInitializeableAnimation = new AnimBool(_initialize.boolValue);
            _isInitializeableAnimation.valueChanged.AddListener(Repaint);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawValue();
            DrawClampedFields();
            DrawReadonlyField();
            DrawInitializeFields();
            DrawDeveloperDescription();
        }
        protected virtual void DrawValue()
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                string content = "Cannot display value. No PropertyDrawer for (" + Target.Type + ") [" + Target.ToString() + "]";
                GenericPropertyDrawer.DrawPropertyDrawerLayout(Target.Type, new GUIContent("Value"), _valueProperty, new GUIContent(content, content));

                if (scope.changed)
                {
                    // Value changed, raise events
                    serializedObject.ApplyModifiedProperties(); //upate with value
                    Target.Raise();
                }
            }
        }
        protected void DrawInitializeFields()
        {
            if (!IsInitializeable || _readOnly.boolValue)
                return;

            EditorGUILayout.PropertyField(_initialize);
            _isInitializeableAnimation.target = _initialize.boolValue;

            using (var anim = new EditorGUILayout.FadeGroupScope(_isInitializeableAnimation.faded))
            {
                if(anim.visible)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        EditorGUILayout.PropertyField(_initializeValueProperty);
                    }
                }
            }
        }
        protected void DrawClampedFields()
        {
            if (!IsClampable)
                return;

            EditorGUILayout.PropertyField(_isClamped);
            _isClampedVariableAnimation.target = _isClamped.boolValue && !_readOnly.boolValue;

            using (var anim = new EditorGUILayout.FadeGroupScope(_isClampedVariableAnimation.faded))
            {
                if(anim.visible)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        EditorGUILayout.PropertyField(_minValueProperty);
                        EditorGUILayout.PropertyField(_maxValueProperty);
                    }
                }                
            }
        }
        protected void DrawReadonlyField()
        {
            if (IsClamped)
            {
                _readOnly.boolValue = false;
                return;
            }

            EditorGUILayout.PropertyField(_readOnly, new GUIContent("Read Only", READONLY_TOOLTIP));

            _raiseWarningAnimation.target = _readOnly.boolValue;
            using (var fadeGroup = new EditorGUILayout.FadeGroupScope(_raiseWarningAnimation.faded))
            {
                if (fadeGroup.visible)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_raiseWarning);
                    EditorGUI.indentLevel--;
                }
            }
        }
        protected void DrawDeveloperDescription()
		{
			using (var scope = new EditorGUI.ChangeCheckScope())
			{
				EditorGUILayout.PropertyField(_developerDescription);
				if(scope.changed)
					serializedObject.ApplyModifiedProperties();
			}
        }
    }
}