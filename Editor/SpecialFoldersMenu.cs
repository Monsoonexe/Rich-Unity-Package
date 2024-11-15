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
	/// Contains paths and MenuItems for common file paths in unity.
	/// </summary>
	public static class SpecialFoldersMenu
	{
		private const string MenuPath = RichEditorUtility.MenuName + "Special Folders/";
        private const string MenuPath_Library = MenuPath + "Library/";

        private static string LibraryFolder => Application.dataPath + "/../Library";
        private static string ScriptAssembliesFolder => LibraryFolder + "/ScriptAssemblies";

        [MenuItem(MenuPath + "'dataPath' (Assets)", priority = 1)]
		public static void OpenDataPath()
		{
			Process.Start(Application.dataPath);
		}

		[MenuItem(MenuPath + "'persistentDataPath' (AppData)", priority = 2)]
		public static void OpenPersistentDataPath()
		{
			Process.Start(Application.persistentDataPath);
		}

		[MenuItem(MenuPath + "'streamingAssetsPath' (Streaming Assets)", priority = 3)]
		public static void OpenStreamingAssetsPath()
		{
			//ensure it exists if it hasn't been created yet.
			if (!Directory.Exists(Application.streamingAssetsPath))
				AssetDatabase.CreateFolder(Application.dataPath, "StreamingAssets"); // Unity's default location
			Process.Start(Application.streamingAssetsPath);
		}

		[MenuItem(MenuPath + "'temporaryCachePath' (Temp)", priority = 4)]
		public static void OpenTemporaryCachePath()
		{
			Process.Start(Application.temporaryCachePath);
		}

        [MenuItem(MenuPath + "'applicationContentsPath' (Installation)", priority = 5)]
        public static void OpenApplicationContentsPath()
        {
            Process.Start(EditorApplication.applicationContentsPath);
        }

        [MenuItem(MenuPath_Library + "Library")]
        public static void OpenLibraryFolder()
        {
            Process.Start(LibraryFolder);
        }

        [MenuItem(MenuPath_Library + "Script Assemblies")]
        public static void OpenScriptAssembliesPath()
        {
            Process.Start(ScriptAssembliesFolder);
        }
	}
}
