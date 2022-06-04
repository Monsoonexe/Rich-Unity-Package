using UnityEngine;
using QFSW.QC;

namespace RichPackage.ConsoleCommands
{
	/// <summary>
	/// 
	/// </summary>
	public static class CommandUtility
	{
		[Command(aliasOverride: "get-component",
			description: "Calls `GetComponent<T>()` on given `gameObject`.")]
		public static TComp GetComponent<TComp>(GameObject gameObject)
			where TComp : Component
			=> gameObject.GetComponent<TComp>();

		[Command("GameObject.Find"), Command(aliasOverride: "find-obj"), 
			Command(aliasOverride: "find")]
		public static GameObject FindGameObject(string identifier)
		{
			GameObject gameObject = GameObject.Find(identifier);

			if (gameObject == null)
				Debug.Log($"<{identifier}> not found. Check spelling and presence.");
			else
				Debug.Log($"<{identifier}> found.");

			return gameObject;
		}

		[Command(aliasOverride: "register-component", 
			description: "Finds a GameObject named 'gameObjectName`," +
			" gets the Component of type `<TComp>`, and registers it to the " +
			"QuantumRegistry.")]
		public static void RegisterComponent<TComp>(string gameObjectName)
			where TComp : Component
		{
			QuantumRegistry.RegisterObject(
				FindGameObject(gameObjectName).GetComponent<TComp>());
		}
	}
}
