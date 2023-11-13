using UnityEditor;

namespace RichPackage.Editor
{
    /// <summary>
    /// 
    /// </summary>
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
    }
}
