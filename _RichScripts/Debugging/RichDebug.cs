using System;
using System.Diagnostics;

namespace RichPackage.Debugging
{
    using Debug = UnityEngine.Debug;//clarify which static class

    /// <summary>
    /// 
    /// </summary>
    public static class RichDebug
    {
        /// <summary>
        /// Only Logs in Editor. Call is removed from Builds.
        /// </summary>
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public static void EditorLog(string message)
            => Debug.Log(message);

        /// <summary>
        /// Only Logs in Editor. Call is removed from Builds.
        /// </summary>
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public static void EditorLog(string message, UnityEngine.Object context)
            => Debug.Log(message, context);

        /// <summary>
        /// This call gets stripped when built.
        /// Do not put any mutation code in here or you might get inconsistent
        /// results in your build.
        /// </summary>
        /// <param name="editorAction"></param>
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public static void OnlyDoInEditor(Action editorAction)
            => editorAction();
    }
}
