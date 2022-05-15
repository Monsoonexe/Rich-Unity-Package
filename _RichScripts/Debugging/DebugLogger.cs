﻿using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Debugging
{
	/// <summary>
	/// Logs to <see cref="Debug.Log"/> in <see cref="MonoBehaviour"/> form.
	/// </summary>
	/// <remarks>Helpful for debugging events.</remarks>
	public class DebugLogger : RichMonoBehaviour
	{
		private const string BUTTON_GROUP_TAG = "b";

		/// <summary>
		/// Log a message to the console with Debug.Log(string).
		/// </summary>
		[Title("Log Helpers")]
		[Button, FoldoutGroup(BUTTON_GROUP_TAG)]
		public void Log(string message) => Debug.Log(message);

		/// <summary>
		/// Log a message to the console with Debug.Log(string, object) and gives self as context.
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG)]
		public void LogSelf(string message) => Debug.Log(message, this);

		/// <summary>
		/// Log a message to the console with Debug.LogError(string).
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG)]
		public void LogError(string message) => Debug.LogError(message);

		/// <summary>
		/// Log a message to the console with Debug.LogError(string, object) and gives self as context.
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG)]
		public void LogErrorSelf(string message) => Debug.LogError(message, this);

		/// <summary>
		/// Log a message to the console with Debug.LogWarning(string).
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG)]
		public void LogWarning(string message) => Debug.LogWarning(message);

		/// <summary>
		/// Log a message to the console with Debug.LogWarning(string, object) and gives self as context.
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG)]
		public void LogWarningSelf(string message) => Debug.LogWarning(message, this);

	}

}
