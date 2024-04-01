using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

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
        
        [MenuItem(CompilerLockMenu)]
        public static void ToggleCompilerLock()
        {
            // get current state
            bool locked = Menu.GetChecked(CompilerLockMenu);

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

        public static string GetGuidFromAsset(UnityEngine.Object asset)
        {
            return AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString();
        }

        public static IEnumerable<T> LoadAllAssetsInFolder<T>(string folder)
            where T : UnityEngine.Object
        {
            return Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly)
                .Where(f => !f.QuickEndsWith(".meta"))
                .Where(f => AssetDatabase.GetMainAssetTypeAtPath(f) == typeof(T))
                .Select(f => AssetDatabase.LoadAssetAtPath<T>(f));
        }
    }
}
