using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Debugging
{
	/// <summary>
	/// Logs to <see cref="Debug.Log"/> in <see cref="MonoBehaviour"/> form.
	/// </summary>
	/// <remarks>Helpful for debugging events.</remarks>
	public class DebugLogger : RichMonoBehaviour
	{
		private const string BUTTON_GROUP_TAG = "Log Helpers";

		/// <summary>
		/// Log a message to the console with Debug.Log(string).
		/// </summary>
		[Title("Log Helpers")]
		[Button, FoldoutGroup(BUTTON_GROUP_TAG),
            HideInCallstack]
		public void Log(string message) => Debug.Log(message);

        /// <summary>
        /// Logs <see langword="this"/>'s <see cref="UnityEngine.Object.name"/>.
        /// </summary>
        [HideInCallstack]
		public void LogSelf() => Debug.Log(name, this);

		/// <summary>
		/// Log a message to the console with Debug.Log(string, object) and gives self as context.
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG),
            HideInCallstack]
		public void LogSelf(string message) => Debug.Log(message, this);

		/// <summary>
		/// Log a message to the console with Debug.LogError(string).
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG),
            HideInCallstack]
		public void LogError(string message) => Debug.LogError(message);

        /// <summary>
        /// Logs <see langword="this"/>'s <see cref="UnityEngine.Object.name"/>.
        /// </summary>
        [HideInCallstack]
        public void LogErrorSelf() => Debug.LogError(name, this);

		/// <summary>
		/// Log a message to the console with Debug.LogError(string, object) and gives self as context.
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG),
            HideInCallstack]
		public void LogErrorSelf(string message) => Debug.LogError(message, this);

		/// <summary>
		/// Log a message to the console with Debug.LogWarning(string).
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG),
            HideInCallstack]
		public void LogWarning(string message) => Debug.LogWarning(message);

        /// <summary>
        /// Logs <see langword="this"/>'s <see cref="UnityEngine.Object.name"/>.
        /// </summary>
        [HideInCallstack]
        public void LogWarningSelf() => Debug.LogWarning(name, this);

		/// <summary>
		/// Log a message to the console with Debug.LogWarning(string, object) and gives self as context.
		/// </summary>
		[Button, FoldoutGroup(BUTTON_GROUP_TAG),
            HideInCallstack]
		public void LogWarningSelf(string message) => Debug.LogWarning(message, this);
	}
}
