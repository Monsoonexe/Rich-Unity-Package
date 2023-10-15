using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor.ValidationRules
{
    /// <seealso cref="ValidationRulesWindow"/>
    public abstract class AValidationRule : IValidationRule
    {
        private bool ignored;

        public string SaveName
        {
            get => ValidationRuleUtility.SaveNameRoot + GetType().Name;
        }

        public bool Ignore
        {
            // TODO - save these values
            get => ignored;
            set
            {
                ignored = value;
            }
        }
        
        public List<ValidationRuleIssue> Issues { get; protected set; } = new List<ValidationRuleIssue>();

		public abstract void Validate();
        
        protected static void SelectObject(Object obj)
        {
            Selection.activeObject = obj;
        }

        protected void SelectObject(Object obj, FieldInfo field)
        {
	        SelectObject(obj);
	        Highlighter.Highlight("Inspector", ObjectNames.NicifyVariableName(field.Name));
        }
     
        /// <param name="asset">The asset to select.</param>
        /// <param name="actionName">Gets displayed in on the button text.</param>
        public static ValidationRuleAction CreateSelectObjectAction(Object asset,
            string actionName = "Select Object")
        {
            return new ValidationRuleAction(actionName, () =>
            {
                SelectObject(asset);
            });
        }
    }
}
