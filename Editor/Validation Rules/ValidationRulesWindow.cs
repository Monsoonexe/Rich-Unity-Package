using RichPackage;
using RichPackage.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor.ValidationRules
{
    /// <summary>
    /// An editor window that helps visualize and fix common configuration problems in the software.
    /// </summary>
    /// <seealso cref="IValidationRule"/>
    public sealed class ValidationRulesWindow : OdinEditorWindow
    {
        internal static event Action<List<IValidationRule>> OnIssuesUpdated;

        private static List<IValidationRule> allRules = new List<IValidationRule>(); 
        private static List<IValidationRule> activeRules = new List<IValidationRule>();
        private Vector2 scrollPosition;
        public static ValidationRulesWindow Instance { get; private set; }
        private static bool showRules = true;

        private static bool needsUpdate = false;
        private static Type[] _allGameRuleTypes = Type.EmptyTypes;
        private static IValidationRule[] _rules = Array.Empty<IValidationRule>();

        [MenuItem("Editor tools/Windows/Validation Rules Wizard", isValidateFunction: false, 2)]
        public static void ShowWindow()
        {
            Instance = GetWindow<ValidationRulesWindow>(true, "Validation Wizard", true);
            Instance.minSize = new Vector2(400, 500);

            CheckForIssues();
            Instance.Repaint();
        }

        private static void InitializeOnLoadMethod()
        {
            needsUpdate = true;
            UpdateRules();
        }

        private static IEnumerable<IValidationRule> GetAllRules()
        {
            if (_rules != null && _rules.Length > 0)
            {
                return _rules;
            }

            _allGameRuleTypes = ReflectionUtility.GetAllTypesThatImplement(typeof(IValidationRule), true);
            var rules = new List<IValidationRule>(_allGameRuleTypes.Length);
            foreach (var type in _allGameRuleTypes)
            {
                rules.Add((IValidationRule)Activator.CreateInstance(type));
            }

            //for (int i = rules.Count - 1; i >= 0; i--)
            //{
            //    var hideAttribute = (HidesGameRuleAttribute)rules[i].GetType()
	           //     .GetCustomAttributes(typeof(HidesGameRuleAttribute), true)
	           //     .FirstOrDefault();
            //    if (hideAttribute != null)
            //    {
            //        rules.RemoveAll(o => o.GetType() == hideAttribute.type);
            //    }
            //}

            _rules = rules.ToArray();
            return _rules;
        }

        private static IEnumerable<IValidationRule> GetAllActiveRules()
        {
			return GetAllRules().Where(o => o.Ignore == false);
        }

        private static void UpdateRules()
        {
            allRules.Clear();
            allRules.AddRange(GetAllRules());
            foreach (IValidationRule rule in allRules)
            {
                rule.Issues?.Clear();
            }

            activeRules.Clear();
            activeRules.AddRange(GetAllActiveRules());
        }

        private static void RunRuleValidation(IValidationRule rule)
		{
			try
			{
				rule.Validate();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}
		}

        private static void CheckForIssues()
        {
            needsUpdate = false;
            UpdateRules();

            for (int i = 0; i < activeRules.Count; i++)
            {
	            float progress = (i + 1f) / activeRules.Count;
	            EditorUtility.DisplayProgressBar("Scanning...", "Searching project for issues...", progress);
                RunRuleValidation(activeRules[i]);
            }

            EditorUtility.ClearProgressBar();
            OnIssuesUpdated?.Invoke(activeRules);
		}

        private static void DrawAllRules()
		{
			EditorGUILayout.BeginVertical();

			foreach (IValidationRule rule in allRules)
			{
                DrawRule(rule);
			}

			EditorGUILayout.EndVertical();
		}

        private static void DrawRule(IValidationRule rule)
		{
			if (rule.Ignore)
			{
				GUI.color = Color.grey;
			}

			EditorGUI.BeginChangeCheck();
			bool result = EditorGUILayout.ToggleLeft(rule.SaveName, !rule.Ignore);
			if (EditorGUI.EndChangeCheck())
			{
				rule.Ignore = !result;
			}

			GUI.color = Color.white;
		}
        
        private void DrawIssues(IValidationRule rule)
		{
			List<ValidationRuleIssue> issues = rule.Issues;
			for (int i = issues.Count - 1; i >= 0; i--)
	        {
		        ValidationRuleIssue issue = issues[i];
		        EditorGUILayout.HelpBox(issue.message, issue.messageType);

		        GUILayout.BeginHorizontal("Toolbar");
		        foreach (ValidationRuleAction action in issue.actions)
		        {
			        bool isFixAction = action.name.ContainsIgnoreCase("fix");

					if (isFixAction)
			        {
				        GUI.color = Color.green;
			        }

                    // Try to fix the issue
			        if (GUILayout.Button(action.name, "toolbarbutton"))
			        {
                        bool issueFixed;
				        try
				        {
					        action.action();
					        issueFixed = true;

				        }
				        catch (Exception ex)
				        {
                            Debug.LogError($"Could not fix {issue} with {action} due to {ex.Message}.");
                            issueFixed = false;
				        }

                        // remove issue if fixed
				        if (isFixAction && issueFixed)
				        {
					        issues.RemoveAt(i);
				        }
			        }

			        GUI.color = Color.white;
		        }

		        if (issue.messageType < MessageType.Error)
		        {
			        GUI.color = Color.yellow;
			        if (GUILayout.Button("Ignore", "toolbarbutton"))
			        {
				        rule.Ignore = true;
				        issues.RemoveAt(i);
			        }
			        GUI.color = Color.white;
		        }

		        GUILayout.EndHorizontal();
	        }
		}

        protected override void OnEnable()
        {
            base.OnEnable();
            InitializeOnLoadMethod();
		}

        protected override void OnGUI()
        {
            base.OnGUI();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            DrawHeaderToolbar();

            if (showRules)
            {
	            DrawAllRules();
            }

            if (needsUpdate)
            {
                EditorGUILayout.HelpBox("Not scanned. Hit Force rescan", MessageType.Warning);
            }
            else if (activeRules.Sum(o => o.Ignore == false && o.Issues.Count != 0 ? 1 : 0) == 0)
            {
                EditorGUILayout.HelpBox("No problems found...", MessageType.Info);
            }

            foreach (IValidationRule rule in activeRules)
            {
                DrawIssues(rule);
            }

            GUILayout.EndScrollView();
            GUIUtility.ExitGUI(); // fixes "EndLayoutGroup should be called first" Unity bug.
		}

        private void DrawHeaderToolbar()
        {
            // Draw "Show Rules" button
            GUILayout.BeginHorizontal("Toolbar");

            if (GUILayout.Button("Show Rules", "toolbarbutton"))
            {
                showRules = !showRules;
            }

            // Draw "Force Rescan" button
            GUI.color = Color.green;
            if (GUILayout.Button("Force Rescan", "toolbarbutton"))
            {
                CheckForIssues();
                Repaint();
            }

            GUI.color = Color.white;

            GUILayout.EndHorizontal();
        }
    }
}
