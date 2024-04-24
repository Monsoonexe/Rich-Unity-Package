using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using ModestTree;

namespace RichPackage.Editor
{
    public static class RichAssetDatabaseUtilities
    {
        public static IEnumerable<T> LoadAllAssetsInFolder<T>(string folder)
            where T : Object
        {
            return Directory.EnumerateFiles(folder, "*", SearchOption.TopDirectoryOnly)
                .Where(f => !f.QuickEndsWith(".meta"))
                .Where(f => AssetDatabase.GetMainAssetTypeAtPath(f) == typeof(T))
                .Select(f => AssetDatabase.LoadAssetAtPath<T>(f));
        }

        public static string GetGuidFromAsset(Object asset)
        {
            return AssetDatabase.GUIDFromAssetPath(AssetDatabase.GetAssetPath(asset)).ToString();
        }

        public static T LoadAssetFromGuid<T>(string guid)
            where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid));
        }

        public static string GetSourceFileForScriptableObject(ScriptableObject scriptableObject)
        {
            return AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(scriptableObject));
        }

        public static string GetSourceFileForMonoBehavior(MonoBehaviour behaviour)
        {
            return AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(behaviour));
        }

        [MenuItem("Assets/Copy Guid")]
        private static void CopyGuidToClipboard(MenuCommand menuCommand)
        {
            Object obj = menuCommand.context;
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long _))
            {
                Debug.LogError("I just can't do it!");
                return;
            }

            GUIUtility.systemCopyBuffer = guid;
        }

        [MenuItem("Assets/Force Reserialize")]
        private static void ForceReserialize(MenuCommand menuCommand)
        {
            string path = AssetDatabase.GetAssetPath(menuCommand.context);
            AssetDatabase.ForceReserializeAssets(path.Yield());
        }
    }
}
