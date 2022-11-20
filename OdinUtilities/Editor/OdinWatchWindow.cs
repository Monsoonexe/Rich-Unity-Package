/* Source: https://odininspector.com/community-tools/57E/odin-watch-window
 */

#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RichPackage.Editor
{
	public class OdinWatchWindow : OdinEditorWindow
	{
		[SerializeField]
		private List<TreeValuesHolder> _properties = new List<TreeValuesHolder>();
		private static OdinWatchWindow _instance;
		private bool _repaintSheduled;
		[SerializeField] private float _labelWidth = 200;
		private bool _showSettings;

		[MenuItem("Tools/Odin Inspector/Watch Window")]
		public static void ShowMenu()
		{
			_instance = GetWindow<OdinWatchWindow>();
			_instance.Show();
		}

		[InitializeOnLoadMethod]
		private static void OnPropertyContextMenu()
		{
			EditorApplication.contextualPropertyMenu += (menu, property) =>
			{
				property = property.Copy();
				menu.AddItem(new GUIContent("Watch"), false, () =>
				{
					ShowMenu();
					PropertyTree tree = PropertyTree.Create(new SerializedObject(property.serializedObject.targetObject));
					TreeValuesHolder holder = _instance._properties.FirstOrDefault(
						o => ReferenceEquals(o.Tree.WeakTargets[0], property.serializedObject.targetObject));
					if (holder == null)
					{
						holder = new TreeValuesHolder(tree);
						_instance._properties.Add(holder);
						tree.OnPropertyValueChanged += TreeOnOnPropertyValueChanged;
					}
					holder.ValuePaths.Add(property.propertyPath);
				});
			};
		}

		private void PlayModeStateChanged(PlayModeStateChange obj)
		{
			foreach (TreeValuesHolder holder in _properties)
			{
				holder.CheckRefresh();
			}
		}


		private void OnInspectorUpdate()
		{
			Repaint();
		}

		public static void AddWatch(InspectorProperty property)
		{
			ShowMenu();
			PropertyTree tree = Sirenix.OdinInspector.Editor.PropertyTree.Create(property.Tree.WeakTargets, new SerializedObject(property.Tree.UnitySerializedObject.targetObject));
			TreeValuesHolder holder = _instance._properties.FirstOrDefault(o => o.Tree.WeakTargets[0] == property.Tree.WeakTargets[0]);
			if (holder == null)
			{
				holder = new TreeValuesHolder(tree);
				_instance._properties.Add(holder);
				tree.OnPropertyValueChanged += TreeOnOnPropertyValueChanged;
			}
			holder.ValuePaths.Add(property.Path);
		}

		private static void TreeOnOnPropertyValueChanged(InspectorProperty property, int selectionindex)
		{
			if (!_instance._repaintSheduled)
				_instance.Repaint();
			_instance._repaintSheduled = true;
		}

		protected override void OnDisable()
		{
			string json = EditorJsonUtility.ToJson(this);
			EditorPrefs.SetString("OWW_props", json);
			base.OnDisable();
		}

		protected override void OnEnable()
		{
			_labelWidth = EditorPrefs.GetFloat("OWW_labelWidth", 200);
			string json = EditorPrefs.GetString("OWW_props", "");
			EditorJsonUtility.FromJsonOverwrite(json, this);
			EditorApplication.playModeStateChanged -= PlayModeStateChanged;
			EditorApplication.playModeStateChanged += PlayModeStateChanged;
			wantsMouseMove = true;

			for (int i = 0; i < _properties.Count; i++)
			{
				TreeValuesHolder holder = _properties[i];
				if (!holder.CheckRefresh())
				{
					_properties.RemoveAt(i--);
				}
			}
		}

		protected override void OnGUI()
		{
			_repaintSheduled = false;
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Clear"))
			{
				_properties.Clear();
			}

			Rect settingsRect = GUILayoutUtility.GetRect(24, 24, GUILayout.ExpandWidth(false)).AlignLeft(20).AlignCenterY(20);
			if (SirenixEditorGUI.IconButton(settingsRect, _showSettings ? EditorIcons.SettingsCog.Inactive : EditorIcons.SettingsCog.Active, "Settings"))
			{
				_showSettings = !_showSettings;
			}
			GUILayout.EndHorizontal();

			if (_showSettings)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Space(40);
				GUI.changed = false;
				Rect rect = GUILayoutUtility.GetRect(1, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
				_labelWidth = GUI.HorizontalSlider(rect, _labelWidth, rect.xMin, rect.xMax);
				if (GUI.changed)
					EditorPrefs.SetFloat("OWW_labelWidth", _labelWidth);
				EditorGUILayout.LabelField("Label Width", GUILayout.Width(70));
				GUILayout.EndHorizontal();
			}

			GUILayout.Space(5);
			bool first = true;

			if (_properties.Count == 0)
			{
				EditorGUILayout.LabelField("Right-click any property in an Inspector and select 'Watch' to make it show up here.", SirenixGUIStyles.MultiLineCenteredLabel);
			}

			GUIHelper.PushLabelWidth(_labelWidth - 30);

			for (int i = 0; i < _properties.Count; i++)
			{
				TreeValuesHolder holder = _properties[i];
				holder.CheckRefresh();
				if (!first)
					GUILayout.Space(5);
				first = false;

				Rect titleRect = SirenixEditorGUI.BeginBox("      " + holder.Tree.TargetType.Name);

				titleRect = titleRect.AlignTop(21);
				if (holder.ParentObject != null)
				{
					Rect alignRight = titleRect.AlignRight(200).AlignCenterY(16).AlignLeft(180);
					GUIHelper.PushGUIEnabled(false);
					SirenixEditorFields.UnityObjectField(alignRight, holder.ParentObject, typeof(GameObject), true);
					GUIHelper.PopGUIEnabled();
				}

				if (SirenixEditorGUI.IconButton(titleRect.AlignRight(20).AlignCenterY(18), EditorIcons.X))
				{
					_properties.RemoveAt(i--);
				}

				Rect titleDragDropRect = titleRect.AlignLeft(30).AlignCenter(20, 20);
				EditorIcons.List.Draw(titleDragDropRect);

				TreeValuesHolder treedragdrop = (TreeValuesHolder)DragAndDropUtilities.DragAndDropZone(titleDragDropRect, holder, typeof(TreeValuesHolder), false, false);
				if (treedragdrop != holder)
				{
					int treeDragDropIndex = _properties.IndexOf(treedragdrop);
					Swap(_properties, treeDragDropIndex, i);
				}

				if (holder.Tree.UnitySerializedObject?.targetObject == null)
				{
					EditorGUILayout.LabelField($"This component is no longer valid in the current context (loaded different scene?)", SirenixGUIStyles.MultiLineLabel);
				}
				else
				{
					holder.Tree.BeginDraw(true);
					for (int index = 0; index < holder.ValuePaths.Count; index++)
					{
						string path = holder.ValuePaths[index];
						GUILayout.BeginHorizontal();

						Rect rect1 = GUILayoutUtility.GetRect(EditorGUIUtility.singleLineHeight + 5, EditorGUIUtility.singleLineHeight + 3, GUILayout.ExpandWidth(false)).AlignRight(EditorGUIUtility.singleLineHeight + 2);

						EditorIcons.List.Draw(rect1);

						ValueDragDropHolder dragdrop = (ValueDragDropHolder)DragAndDropUtilities.DragAndDropZone(rect1, new ValueDragDropHolder(holder, index), typeof(ValueDragDropHolder), false, false);
						if (dragdrop.TreeValuesHolder == holder && dragdrop.Index != index)
						{
							string ptemp = holder.ValuePaths[index];
							holder.ValuePaths[index] = holder.ValuePaths[dragdrop.Index];
							holder.ValuePaths[dragdrop.Index] = ptemp;
						}

						InspectorProperty propertyAtPath = holder.Tree.GetPropertyAtPath(path) ?? holder.Tree.GetPropertyAtUnityPath(path);
						if (propertyAtPath != null)
						{
							propertyAtPath.Draw();
						}
                        else
                        {
							EditorGUILayout.LabelField($"Could not find property ({path})");
                        }

						if (SirenixEditorGUI.IconButton(EditorIcons.X))
						{
							holder.ValuePaths.RemoveAt(index--);
							if (holder.ValuePaths.Count == 0)
								_properties.RemoveAt(i--);
						}

						GUILayout.Space(3);
						GUILayout.EndHorizontal();
					}

					holder.Tree.EndDraw();
				}

				SirenixEditorGUI.EndBox();
			}

			GUIHelper.PopLabelWidth();
		}

		[Serializable]
		private class TreeValuesHolder
		{
			[NonSerialized]
			public PropertyTree Tree;
			[NonSerialized]
			public Object Target;
			public int InstanceID;
			public List<string> ValuePaths = new List<string>();
			[NonSerialized]
			public Object ParentObject;

			public TreeValuesHolder(PropertyTree tree)
			{
				Tree = tree;
				Target = (Object)tree.WeakTargets[0];
				InstanceID = Target.GetInstanceID();
				GetParentObject();
			}

			public bool CheckRefresh()
			{
				if (Tree == null || Tree.RootProperty == null || Target == null)
				{
					Target = EditorUtility.InstanceIDToObject(InstanceID);
					if (Target == null)
						return false;
					GetParentObject();
					Tree = Sirenix.OdinInspector.Editor.PropertyTree.Create(new List<Object> { Target }, ParentObject != null ? new SerializedObject(ParentObject) : null);

				}
				return true;
			}

			private void GetParentObject()
			{
				ParentObject = (Target as MonoBehaviour);
				if (ParentObject == null)
					ParentObject = (Target as Component);
				if (ParentObject == null)
					ParentObject = Target as ScriptableObject;
			}
		}

		private struct ValueDragDropHolder
		{
			public TreeValuesHolder TreeValuesHolder;
			public int Index;
			public ValueDragDropHolder(TreeValuesHolder treeValuesHolder, int index)
			{
				TreeValuesHolder = treeValuesHolder;
				Index = index;
			}
		}

		private void Swap<T>(IList<T> list, int indexA, int indexB)
		{
			T tmp = list[indexA];
			list[indexA] = list[indexB];
			list[indexB] = tmp;
		}

	}

	[DrawerPriority(100, 0, 0)]
	public class OdinWatchWindowContextMenuDrawer<T> : OdinValueDrawer<T>, IDefinesGenericMenuItems
	{
		public void PopulateGenericMenu(InspectorProperty property, GenericMenu genericMenu)
		{

			genericMenu.AddItem(new GUIContent("Watch"), false, () => OdinWatchWindow.AddWatch(property));
		}

		protected override void DrawPropertyLayout(GUIContent label)
		{
			this.CallNextDrawer(label);
		}
	}
}
#endif
