using System;
using System.Diagnostics;
using UnityEngine;

namespace RichPackage.Debug
{
    using Debug = UnityEngine.Debug;//clarify which static class

    /// <summary>
    /// 
    /// </summary>
    public static class RichDebug
    {
        /// <summary>
        /// Only Logs in Editor. Removed from Builds.
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void EditorLog(string message)
        {
            Debug.Log(message);
        }

        /// <summary>
        /// Only Logs in Editor. Removed from Builds.
        /// </summary>
        [Conditional("UNITY_EDITOR")]
        public static void EditorLog(string message, UnityEngine.Object context)
        {
            Debug.Log(message, context);
        }

        /// <summary>
        /// This call gets stripped when built.
        /// Do not put any mutable code in here or you might get inconsistent
        /// results in your build.
        /// </summary>
        /// <param name="editorAction"></param>
        [Conditional("UNITY_EDITOR")]
        public static void OnlyDoInEditor(Action editorAction)
            => editorAction();
    }
}
