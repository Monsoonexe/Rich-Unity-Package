/* Source: https://odininspector.com/community-tools/540/scriptableobject-creator
 * 
 */

#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Reflection;

namespace RichPackage.Editor
{
    public class ScriptableObjectCreator : OdinMenuEditorWindow
    {
        private static HashSet<Type> scriptableObjectTypes;

        private ScriptableObject previewObject;
        private string targetFolder;
        private Vector2 scroll;
        private string assetName;

        private static HashSet<Type> GatherTypes()
        {
            const AssemblyTypeFlags flags
                = AssemblyTypeFlags.UserTypes
                | AssemblyTypeFlags.PluginTypes;

            HashSet<Type> types = AssemblyUtilities.GetTypes(flags)
                .Where(t => t.IsClass && typeof(ScriptableObject).IsAssignableFrom(t))
                .ToHashSet();
            types.AddRange(Assembly.GetAssembly(typeof(ScriptableObjectArchitecture.SOArchitectureBaseObject)).GetTypes()
                .Where(t => t.IsClass && typeof(ScriptableObject).IsAssignableFrom(t)));
            return types;
        }

        [MenuItem("Assets/Create Scriptable Object", priority = -1000),
            MenuItem("Tools/Odin/Create Scriptable Object")]
        private static void OpenWindow()
        {
			// handle generation
			if (scriptableObjectTypes == null)
                scriptableObjectTypes = GatherTypes();

            var path = "Assets";
            var obj = Selection.activeObject;
            if (obj && AssetDatabase.Contains(obj))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (!Directory.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
            }

            var window = CreateInstance<ScriptableObjectCreator>();
            window.ShowUtility();
            window.position = GUIHelper.GetEditorWindowRect()
                .AlignCenter(800, 500);
            window.titleContent = new GUIContent(path);
            window.targetFolder = path.Trim('/');
        }

        private Type SelectedType
        {
            get
            {
                var m = this.MenuTree.Selection.LastOrDefault();
                return m?.Value as Type;
            }
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            if (scriptableObjectTypes == null)
                scriptableObjectTypes = GatherTypes();

            this.MenuWidth = 270;
            this.WindowPadding = Vector4.zero;

            OdinMenuTree tree = new OdinMenuTree(false);
            tree.Config.DrawSearchToolbar = true;
            tree.DefaultMenuStyle = OdinMenuStyle.TreeViewStyle;
            tree.AddRange(scriptableObjectTypes.Where(x => !x.IsAbstract), GetMenuPathForType)
                .AddThumbnailIcons();
            tree.SortMenuItemsByName();
            tree.Selection.SelectionConfirmed += x => this.CreateAsset();
            tree.Selection.SelectionChanged += e =>
            {
                if (this.previewObject && !AssetDatabase.Contains(this.previewObject))
                {
                    DestroyImmediate(this.previewObject);
                }

                if (e != SelectionChangedType.ItemAdded)
                {
                    return;
                }

                var t = this.SelectedType;
                if (t != null && !t.IsAbstract)
                {
                    assetName = this.MenuTree.Selection.First().Name.Remove(" ") + "_";
                    this.previewObject = CreateInstance(t);
                }
            };

            return tree;
        }

        private string GetMenuPathForType(Type t)
        {
            if (t != null && t != typeof(System.Object) && t != typeof(UnityEngine.Object)
                && t != typeof(ScriptableObject))
            {
                string name = t.Name.Split('`').First().SplitPascalCase();
                return GetMenuPathForType(t.BaseType) + "/" + name;
            }

            return string.Empty;
        }

        protected override IEnumerable<object> GetTargets()
        {
            yield return this.previewObject;
        }

        protected override void DrawEditor(int index)
        {
            this.scroll = GUILayout.BeginScrollView(this.scroll);
            {
                base.DrawEditor(index);
            }
            GUILayout.EndScrollView();

            if (this.previewObject)
            {
                GUILayout.FlexibleSpace();
                SirenixEditorGUI.HorizontalLineSeparator(3);

                GUILayout.Label("Create Asset", new GUIStyle(SirenixGUIStyles.SectionHeader));
                DrawNewAssetNamePreviewField();
                if (GUILayout.Button("Create Asset", GUILayoutOptions.Height(30)))
                {
                    this.CreateAsset();
                }
            }
        }

        private void DrawNewAssetNamePreviewField()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Asset Name: ", GUILayoutOptions.Width(80));
            assetName = GUILayout.TextField(assetName);
            GUILayout.EndHorizontal();
        }

        private void CreateAsset()
        {
            if (this.previewObject)
            {
                string path = $"{targetFolder}/{assetName}.asset";
                path = AssetDatabase.GenerateUniqueAssetPath(path); // item 1, item 2, etc.
                AssetDatabase.CreateAsset(this.previewObject, path);
                AssetDatabase.Refresh();
                var item = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
                Selection.activeObject = item;
                // EditorApplication.delayCall += this.Close;
            }
        }

    }
}

#endif
