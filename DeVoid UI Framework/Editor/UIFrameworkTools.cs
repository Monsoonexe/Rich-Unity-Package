using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RichPackage.UI.Framework.Editor
{
	public static class UIFrameworkTools
	{
		public const string MenuPath = "Tools/DeVoid/";

		[MenuItem("Assets/Create/deVoid UI/UI Frame in Scene", priority = 2)]
		public static void CreateUIFrameInScene()
		{
			CreateUIFrame();
		}

		[MenuItem("Assets/Create/deVoid UI/UI Frame Prefab", priority = 1)]
		public static void CreateUIFramePrefab()
		{
			GameObject frame = CreateUIFrame();

			string prefabPath = GetCurrentPath();
			prefabPath = EditorUtility.SaveFilePanel("UI Frame Prefab", prefabPath, "UIFrame", "prefab");

			if (prefabPath.StartsWith(Application.dataPath))
			{
				prefabPath = "Assets" + prefabPath.Substring(Application.dataPath.Length);
			}

			if (!string.IsNullOrEmpty(prefabPath))
			{
				CreateNewPrefab(frame, prefabPath);
			}

			Object.DestroyImmediate(frame);
		}

		private static GameObject CreateUIFrame()
		{
			int uiLayer = LayerMask.NameToLayer("UI");
			var root = new GameObject("UIFrame");
			var camera = new GameObject("UICamera");

			var cam = camera.AddComponent<Camera>();
			cam.clearFlags = CameraClearFlags.Depth;
			cam.cullingMask = LayerMask.GetMask("UI");
			cam.orthographic = true;
			cam.nearClipPlane = 0;
			cam.farClipPlane = 5;

			var frame = root.AddComponent<UIFrame>();
			var canvas = root.GetOrAddComponent<Canvas>();
			root.layer = uiLayer;

			// ScreenSpaceCamera allows you to have things like 3d models, particles
			// and post-fx rendering out of the box (shader/render order limitations still apply)
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = cam;

			cam.transform.SetParent(root.transform, false);
			cam.transform.localPosition = new Vector3(0f, 0f, -1500f);

			var screenScaler = root.GetOrAddComponent<CanvasScaler>();
			screenScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			screenScaler.referenceResolution = new Vector2(1920, 1080);

			var graphicRaycaster = root.GetOrAddComponent<GraphicRaycaster>();

			// event system
			var eventSystem = new GameObject("EventSystem");
			eventSystem.transform.SetParent(root.transform, false);
			eventSystem.AddComponent<EventSystem>();
			eventSystem.AddComponent<StandaloneInputModule>();

			// Creating the layers
			var panelLayerGO = CreateRect("PanelLayer", root, uiLayer);
			var panelLayer = panelLayerGO.AddComponent<PanelUILayer>();

			var windowLayerGO = CreateRect("WindowLayer", root, uiLayer);
			var windowLayer = windowLayerGO.AddComponent<WindowUILayer>();

			var prioPanelLayer = CreateRect("PriorityPanelLayer", root, uiLayer);

			var windowParaLayerGO = CreateRect("PriorityWindowLayer", root, uiLayer);
			var windowParaLayer = windowParaLayerGO.AddComponent<WindowParaLayer>();
			// setting the para layer via reflection
			SetPrivateField(windowLayer, windowParaLayer, "priorityParaLayer");

			SetPrivateField(frame, panelLayer, "panelLayer");
			SetPrivateField(frame, windowLayer, "windowLayer");

			var darkenGO = CreateRect("DarkenBG", windowParaLayer.gameObject, uiLayer);
			var darkenImage = darkenGO.AddComponent<Image>();
			darkenImage.color = new Color(0f, 0f, 0f, 0.75f);
			// setting the BG darkener via reflection
			SetPrivateField(windowParaLayer, darkenGO, "darkenBackgroundObject");
			darkenGO.SetActive(false);

			var tutorialPanelLayer = CreateRect("TutorialPanelLayer", root, uiLayer);

			// Rigging all the Panel Para-Layers on the Panel Layer
			var prioList = new List<PanelPriorityLayerListEntry>();
			prioList.Add(new PanelPriorityLayerListEntry(EPanelPriority.Default, panelLayer.transform));
			prioList.Add(new PanelPriorityLayerListEntry(EPanelPriority.Priority, prioPanelLayer.transform));
			prioList.Add(new PanelPriorityLayerListEntry(EPanelPriority.SuperPriority, tutorialPanelLayer.transform));
			var panelPrios = new PanelPriorityLayerList(prioList);

			SetPrivateField(panelLayer, panelPrios, "priorityLayers");

			// raycast blocker
			var raycastBlockerObject = new GameObject("Raycast Blocker");
			raycastBlockerObject.transform.SetParent(root.transform, false);
			var blocker = raycastBlockerObject.AddComponent<Image>();
			blocker.rectTransform.anchoredPosition = new Vector3(0, 0, 0);
			StretchToFillParent(blocker.rectTransform);
			SetPrivateField(frame, blocker, "raycastBlocker");

			return root;
		}

		public static string GetCurrentPath()
		{
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			if (path == "")
			{
				path = "Assets";
			}
			else if (Path.GetExtension(path) != "")
			{
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}

			return path;
		}

		private static void StretchToFillParent(RectTransform rectTransform)
		{
			// equivalent to using the Alt + Shift + Stretch pivot preset
			// and zeroing all the rect properties
			rectTransform.anchorMin = Vector3.zero;
			rectTransform.anchorMax = Vector3.one;
			rectTransform.pivot = new Vector2(0.5f, 0.5f);
			rectTransform.offsetMin = Vector2.zero;
			rectTransform.offsetMax = Vector2.zero;
		}

		private static void SetPrivateField(object target, object value, string fieldName)
		{
			var flags = System.Reflection.BindingFlags.NonPublic
				| System.Reflection.BindingFlags.Instance;
			target.GetType()
				.GetField(fieldName, flags)
				.SetValue(target, value);
		}

		private static GameObject CreateRect(string name, GameObject parentGO, int layer)
		{
			var parent = parentGO.GetComponent<RectTransform>();
			var newRect = new GameObject(name, typeof(RectTransform));
			newRect.layer = layer;
			var rt = newRect.GetComponent<RectTransform>();

			rt.anchoredPosition = parent.position;
			rt.anchorMin = new Vector2(0, 0);
			rt.anchorMax = new Vector2(1, 1);
			rt.pivot = new Vector2(0.5f, 0.5f);
			rt.transform.SetParent(parent, false);
			rt.sizeDelta = Vector3.zero;

			return newRect;
		}

		private static void CreateNewPrefab(GameObject obj, string localPath)
		{
			PrefabUtility.SaveAsPrefabAsset(obj, localPath);
		}
	}
}
