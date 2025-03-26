using RichPackage.GuardClauses;
using RichPackage.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor
{
    public static partial class RichEditorUtility
    {
        /// <summary>
        /// The primary menu item folder name for tools and helpers.
        /// </summary>
        /// <remarks>Ends in a backslash.</remarks>
        public const string MenuName = "Tools/RichPackage/";
        public const string WindowMenuName = MenuName + "Windows/";

        private const string CompilerLockMenu = "Tools/Lock Compiler";

        /// <summary>
        /// Uses the 'gate' strategy instead of Unity's 'depth' strategy.
        /// </summary>
        public static bool CompilerIsLocked
        {
            get => Menu.GetChecked(CompilerLockMenu);
            set 
            {
                if (value != CompilerIsLocked)
                {
                    ToggleCompilerLock();
                }
            }
        }
        
        [MenuItem(CompilerLockMenu)]
        public static void ToggleCompilerLock()
        {
            // https://docs.unity3d.com/ScriptReference/EditorApplication.LockReloadAssemblies.html
            // get current state (we can't know actual state. This is just a guess).
            bool locked = CompilerIsLocked;

            // flip
            Menu.SetChecked(CompilerLockMenu, !locked);

            if (locked)
            {
                EditorApplication.UnlockReloadAssemblies();
            }
            else
            {
                EditorApplication.LockReloadAssemblies();
            }
        }

        [MenuItem("Tools/Reload Domain")]
        public static void ReloadDomain() => EditorUtility.RequestScriptReload();

        public static IEnumerable<T> LoadAllAssetsInFolder<T>(string folder)
            where T : UnityEngine.Object
        {
            return RichAssetDatabaseUtilities.LoadAllAssetsInFolder<T>(folder);
        }

        /// <summary>
        /// Finds all serializable fields on <paramref name="target"/> of type <typeparamref name="T"/>
        /// and attempts to assign missing fields to assets found in <paramref name="assetFolder"/> based
        /// on matching asset name and field name.
        /// </summary>
        /// <typeparam name="T">Asset field type.</typeparam>
        /// <param name="target">The collection to modify and operate on.</param>
        /// <param name="assetFolder">Folder with root '/Assets' holding possible assets to assign to <paramref name="target"/>'s fields.</param>
        /// <returns>The count of items assigned.</returns>
        public static int AssingMissingAssetFieldsFromFolder<T>(UnityEngine.Object target, string assetFolder)
            where T : UnityEngine.Object
        {
            GuardAgainst.ArgumentIsNull(target, nameof(target));

            Type targetType = target.GetType();
            var unassignedFields = ReflectionUtility.EnumerateMemberFields<T>(target)
                .Where(f => f.GetValue(target) as UnityEngine.Object == null) // 'missing' counts as 'unassigned'
                .ToArray();
            var assets = LoadAllAssetsInFolder<T>(assetFolder)
                .ToArray();

            int count = 0;
            foreach (FieldInfo field in unassignedFields)
            {
                string fieldName = field.Name;
                T asset = assets
                    .Where(task => task.name.Contains(fieldName))
                    .FirstOrDefault();

                if (asset == null)
                {
                    Debug.LogWarning($"Asset with name '{fieldName}' could not be found.");
                    continue;
                }

                field.SetValue(target, asset);
                count++;
            }

            EditorUtility.SetDirty(target);
            // AssetDatabase.SaveAssetIfDirty(target); 2020
            AssetDatabase.SaveAssets();
            return count;
        }
    }
}
