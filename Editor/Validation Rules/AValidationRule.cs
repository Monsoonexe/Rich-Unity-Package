using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor.ValidationRules
{
    /// <seealso cref="ValidationRulesWindow"/>
    public abstract class AValidationRule : IValidationRule
    {
        public string SaveName
        {
            get => ValidationRuleUtility.SaveNameRoot + GetType().Name;
        }

        public bool Ignore
        {
            get => ApexEditorUtility.Settings.Get(SaveName, false);
            set
            {
                if (value)
                {
                    ApexEditorUtility.Settings.Set(SaveName, true);
                }
                else
                {
                    ApexEditorUtility.Settings.Delete(SaveName);
                }
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
        
    }
}
