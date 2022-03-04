using UnityEngine;
using UnityConsole;

public static class PlayerPrefsEditor
{
	[ConsoleCommand]
	public static void SetInt(string key, int value)
	{
		PlayerPrefs.SetInt(key, value);
		Debug.Log($"Saved {key}: {value}.");
	}

	[ConsoleCommand]
	public static void SetString(string key, string value)
	{
		PlayerPrefs.SetString(key, value);
		Debug.Log($"Saved {key}: {value}.");
	}

	[ConsoleCommand]
	public static void SetFloat(string key, float value)
	{
		PlayerPrefs.SetFloat(key, value);
		Debug.Log($"Saved {key}: {value}.");
	}

	[ConsoleCommand]
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

	[ConsoleCommand]
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

	[ConsoleCommand]
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

	[ConsoleCommand]
	public static void HasKey(string key)
	{
		bool has = PlayerPrefs.HasKey(key);
		Debug.Log($"Key <{key}> exists: {has}.");
	}

	[ConsoleCommand]
	public static void DeleteKey(string key)
	{
		bool contains = PlayerPrefs.HasKey(key);
		if (contains)
		{
			PlayerPrefs.DeleteKey(key);
		}
		Debug.Log(contains ? $"Key <{key}> was deleted." : $"Key <{key}> not found.");
	}

	public static void DeleteAllKeys()
	{
		PlayerPrefs.DeleteAll();
		Debug.Log("Deleted all keys in PlayerPrefs");
	}
}
