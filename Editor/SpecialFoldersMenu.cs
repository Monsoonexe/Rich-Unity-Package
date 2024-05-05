/*
 * Process.Start("C:\\someFilePath"); will open that file with the default program
 * according to its file extension. In the case of a folder, it's opened with FileExplorer (or Finder on Mac and whatever in Linux)
 * 
 */

using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor
{
	/// <summary>
	/// 
	/// </summary>
	public static class SpecialFoldersMenu
	{
		private const string MenuPath = RichEditorUtility.MenuName + "Special Folders/";

		[MenuItem(MenuPath + nameof(OpenDataPath) + " (Assets)")]
		public static void OpenDataPath()
		{
			Process.Start(Application.dataPath);
		}

		[MenuItem(MenuPath + nameof(OpenPersistentDataPath))]
		public static void OpenPersistentDataPath()
		{
			Process.Start(Application.persistentDataPath);
		}

		[MenuItem(MenuPath + nameof(OpenStreamingAssetsPath))]
		public static void OpenStreamingAssetsPath()
		{
			//ensure it exists if it hasn't been created yet.
			if (!Directory.Exists(Application.streamingAssetsPath))
				AssetDatabase.CreateFolder(Application.dataPath, "StreamingAssets"); //Unity's default location
			Process.Start(Application.streamingAssetsPath);
		}

		[MenuItem(MenuPath + nameof(OpenTemporaryCachePath))]
		public static void OpenTemporaryCachePath()
		{
			Process.Start(Application.temporaryCachePath);
		}

        [MenuItem(MenuPath + nameof(OpenScriptAssembliesPath))]
        public static void OpenScriptAssembliesPath()
        {
            Process.Start("Library/ScriptAssemblies");
        }
	}
}
