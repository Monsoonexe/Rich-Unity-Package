using UnityEngine;
using UnityEditor;
using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Drawers;

namespace RichPackage.Editor
{
	/// <summary>
	/// 
	/// </summary>
	[InitializeOnLoad]
	public static class Monobehaviour_ContextMenu
	{
		static Monobehaviour_ContextMenu()
		{
			EditorApplication.contextualPropertyMenu += OnContextMenuOpening;
			//PropertyContextMenuDrawer.FillUnityContextMenu();
		}

		private static void OnContextMenuOpening(GenericMenu menu, SerializedProperty property)
		{
			//var comp = (Component)property.exposedReferenceValue;
			//Debug.Log($"Inspecting type: {property.serializedObject.targetObject}.");
			//menu.AddItem(new GUIContent("GetComponent()"), on: false,
			//	() =>
			//	{
			//		property.objectReferenceValue = property.serializedObject
			//		.targetObject.CastTo<Component>()
			//		.GetComponent("Graphic");
			//	});
		}

		private static void Log()
		{
			Debug.Log("Hello world!");
		}
	}
}
