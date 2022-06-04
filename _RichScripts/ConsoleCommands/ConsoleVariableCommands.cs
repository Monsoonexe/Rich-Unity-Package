﻿using UnityEngine;
using QFSW.QC;
using System.Collections.Generic;

namespace RichPackage.ConsoleCommands
{
	/// <summary>
	/// Stores variables in a dictionary. get-T and set-T.
	/// </summary>
	public class ConsoleVariableCommands
	{
		private static readonly Dictionary<string, object> variableTable = new Dictionary<string, object>(16);

		#region Set Variables

		[Command(aliasOverride: "set-bool")]
		public static bool Setbool(string identifier, bool value)
			=> SetInternal(identifier, value);

		[Command(aliasOverride: "set-int")]
		public static int SetInt(string identifier, int value)
			=> SetInternal(identifier, value);

		[Command(aliasOverride: "set-float")]
		public static float SetFloat(string identifier, float value)
			=> SetInternal(identifier, value);

		[Command(aliasOverride: "set-string")]
		public static string SetString(string identifier, string value)
			=> SetInternal(identifier, value);

		[Command(aliasOverride: "set-gameObject")]
		public static GameObject SetGameObject(string identifier, GameObject value)
			=> SetInternal(identifier, value);

		private static T SetInternal<T>(string identifier, T value)
		{
			variableTable[identifier] = value;
			return value;
		}

		#endregion Set Variables

		#region Get Variables

		[Command(aliasOverride: "get-bool")]
		public static bool GetBool(string identifier)
			=> GetInternal<bool>(identifier);

		[Command(aliasOverride: "get-int")]
		public static int GetInt(string identifier)
			=> GetInternal<int>(identifier);

		[Command(aliasOverride: "get-float")]
		public static float GetFloat(string identifier)
			=> GetInternal<float>(identifier);

		[Command(aliasOverride: "get-string")]
		public static string GetString(string identifier)
			=> GetInternal<string>(identifier);

		[Command(aliasOverride: "get-gameObject")]
		public static GameObject GetGameObject(string identifier)
			=> GetInternal<GameObject>(identifier);

		private static T GetInternal<T>(string identifier)
			=> (T)variableTable[identifier];

		#endregion Get Variables

		[Command(aliasOverride: "rem-var")]
		public static void RemoveVariable(string identifier)
			=> variableTable.Remove(identifier);
	}
}
