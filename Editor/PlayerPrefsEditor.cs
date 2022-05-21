using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using QFSW.QC;

//https://docs.unity3d.com/ScriptReference/PlayerPrefs.html

namespace RichPackage.Editor
{
	/// <summary>
	/// Utility for CRUD operations on PlayerPrefs.
	/// </summary>
	public class PlayerPrefsEditor : OdinEditorWindow
	{
		private const string MenuPath = "RichUtilities/PlayerPrefs/";

		public static PlayerPrefsEditor Instance { get; private set; }

		[MenuItem(MenuPath + "PlayerPrefsEditor")]
		private static void Init()
		{
			Instance = (PlayerPrefsEditor)EditorWindow.GetWindow(typeof(PlayerPrefsEditor));
			Instance.Show();
		}

		[Command, Button]
		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
			Debug.Log($"Saved {key}: {value}.");
		}

		[Command, Button]
		public static void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
			Debug.Log($"Saved {key}: {value}.");
		}

		[Command, Button]
		public static void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
			Debug.Log($"Saved {key}: {value}.");
		}

		[Command, Button]
		public static void GetInt(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				int value = PlayerPrefs.GetInt(key);
				Debug.Log($"Key <{key}>: <{value}>.");
			}
			else
			{
				Debug.Log($"Key <{key}> does not exist.");
			}
		}

		[Command, Button]
		public static void GetFloat(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				float value = PlayerPrefs.GetFloat(key);
				Debug.Log($"Key <{key}>: <{value}>.");
			}
			else
			{
				Debug.Log($"Key <{key}> does not exist.");
			}
		}

		[Command, Button]
		public static void GetString(string key)
		{
			if (PlayerPrefs.HasKey(key))
			{
				string value = PlayerPrefs.GetString(key);
				Debug.Log($"Key <{key}>: <{value}>.");
			}
			else
			{
				Debug.Log($"Key <{key}> does not exist.");
			}
		}

		[Command, Button]
		public static void HasKey(string key)
		{
			bool has = PlayerPrefs.HasKey(key);
			Debug.Log($"Key <{key}> exists: {has}.");
		}

		[Command, Button]
		public static void DeleteKey(string key)
		{
			bool contains = PlayerPrefs.HasKey(key);
			if (contains)
			{
				PlayerPrefs.DeleteKey(key);
			}
			Debug.Log(contains ? $"Key <{key}> was deleted." : $"Key <{key}> not found.");
		}

		[Command, Button, MenuItem(MenuPath + nameof(DeleteAllKeys))]
		public static void DeleteAllKeys()
		{
			PlayerPrefs.DeleteAll();
			Debug.Log("Deleted all keys in PlayerPrefs");
		}
	}
}
