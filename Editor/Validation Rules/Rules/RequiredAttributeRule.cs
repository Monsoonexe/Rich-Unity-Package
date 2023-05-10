using UnityEngine;
using System.Reflection;
using RichPackage.Reflection;
using UnityEditor;
using Sirenix.OdinInspector;

namespace RichPackage.Editor.ValidationRules
{
	/// <summary>
	/// Ensures all fields decorated with <see cref="RequiredAttribute"/> are satisfied.
	/// </summary>
	/// <seealso cref="ValidationRulesWindow"/>
	public sealed class RequiredAttributeRule : AValidationRule
	{
		public static bool CheckResources = false;

		public override void Validate()
        {
			ValidateResources();
			ValidateActiveScene();
        }

		private void ValidateActiveScene()
        {
            var sceneObjects = Object.FindObjectsOfType<MonoBehaviour>();
            foreach (var sceneObject in sceneObjects)
            {
                CheckIssuesFromType(sceneObject);
            }
        }

		private void ValidateResources()
        {
			if (!CheckResources)
				return;

            var comps = Resources.FindObjectsOfTypeAll<Component>();
            foreach (var component in comps)
            {
                CheckIssuesFromType(component);
            }

            var scriptableObjects = Resources.FindObjectsOfTypeAll<ScriptableObject>();
            foreach (var scriptableObject in scriptableObjects)
            {
                CheckIssuesFromType(scriptableObject);
            }
        }

		private void CheckIssuesFromType(Object src)
		{
			var fields = ReflectionUtility.GetFieldsWithAttributeInherited(
				src, typeof(RequiredAttribute));

			foreach (var field in fields)
			{
				CheckIssue(src, field);
			}
		}

		private void CheckIssue(Object src, FieldInfo info)
		{
			object value = info.GetValue(src);
			if (value is null)
			{
				CreateIssue(info, src);
			}
		}

		private void CreateIssue(FieldInfo field, Object obj)
		{
			string message = $"Field '{field.Name}' ({field.FieldType}) on '{obj.GetType()}' is required.";

			Issues.Add(new ValidationRuleIssue(message, MessageType.Error,
				new ValidationRuleAction("Select Object", () =>
				{
					SelectObject(obj, field);
				})));
		}
	}
}