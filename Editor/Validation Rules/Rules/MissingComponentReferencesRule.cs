using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor.ValidationRules
{
    /// <summary>
    /// Validate no Missing Components on game objects.
    /// </summary>
    public sealed class MissingComponentReferencesRule : AValidationRule
    {
        public override void Validate()
        {
			// resources
            Validate(Resources.FindObjectsOfTypeAll<GameObject>());

			// scene
			Validate(Object.FindObjectOfType<GameObject>());
        }

        private void Validate(IEnumerable<GameObject> gameObjects)
        {
	        foreach (var obj in gameObjects)
	        {
		        Validate(obj);
	        }
		}

        private void Validate(GameObject obj)
		{
			var query = obj.GetComponents<Component>()
				.Where(c => c == null);

			foreach (var comp in query)
			{
				CreateIssue(obj);
			}
		}
		
        private void CreateIssue(GameObject obj)
        {
	        void SelectObject() => Selection.activeGameObject = obj;

	        void PrintPath() => Debug.Log(obj.scene.name + "/" + obj.GetFullPath());

			void Fix()
			{
				int count = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
				Debug.Log($"Removed {count} missing objects from {obj}.", obj);
			}
			
			string message = $"Missing component on object {obj.name}.";
			Issues.Add(new ValidationRuleIssue(message, MessageType.Warning,
				new ValidationRuleAction("Select Object", SelectObject),
				new ValidationRuleAction("Print Path", PrintPath),
				new ValidationRuleAction("Fix (remove)", Fix)));
		}
    }
}
