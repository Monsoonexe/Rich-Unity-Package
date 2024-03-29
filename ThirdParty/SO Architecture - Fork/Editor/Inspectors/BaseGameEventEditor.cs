﻿using UnityEditor;

namespace ScriptableObjectArchitecture.Editor
{
    public abstract class BaseGameEventEditor : UnityEditor.Editor
    {
        private IStackTraceObject Target { get { return (IStackTraceObject)target; } }
        private SerializedProperty DeveloperDescription { get { return serializedObject.FindProperty("DeveloperDescription"); } }

        private StackTrace _stackTrace;

        protected abstract void DrawRaiseButton();

        protected virtual void OnEnable()
        {
            _stackTrace = new StackTrace(Target);
            _stackTrace.OnRepaint.AddListener(Repaint);
        }
        public override void OnInspectorGUI()
        {
            DrawRaiseButton();

            if (!SOArchitecture_Settings.Instance.EnableDebug)
                EditorGUILayout.HelpBox("Debug mode disabled\nStack traces will not be filed on raise!", MessageType.Warning);

            _stackTrace.Draw();
			DrawDeveloperDescription();
		}
		protected void DrawDeveloperDescription()
		{
			using (var scope = new EditorGUI.ChangeCheckScope())
			{
				EditorGUILayout.PropertyField(DeveloperDescription);
				if (scope.changed)
					serializedObject.ApplyModifiedProperties();
			}
		}
	}
}