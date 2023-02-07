
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace RichPackage.Application
{
	/// <summary>
	/// Contains utilites for scripting symbols that exist in this project.
	/// </summary>
	public static partial class ScriptingSymbols
	{
        public const string UNITY_EDITOR = "UNITY_EDITOR";

		public static void PrintPresent()
		{
			PrintUnityEditor();
        }

        [Conditional(UNITY_EDITOR)]

        private static void PrintUnityEditor() => Debug.Log(UNITY_EDITOR);
    }
}
