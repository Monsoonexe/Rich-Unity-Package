using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using TMPro;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using RichPackage.GuardClauses;

namespace RichPackage.Editor
{
    /// <summary>
    /// I help assign a Font Asset to all TextMeshPro objects in a Scene.
    /// </summary>
    public class FontSetterWindow : OdinEditorWindow
    {
        public static FontSetterWindow Instance { get; private set; }

        [SerializeField, Required]
        private TMP_FontAsset workingFont;

        [SerializeField, ListDrawerSettings(Expanded = true, ShowItemCount = true, ShowPaging = true),
            PropertyOrder(2)]
        private List<TextMeshProUGUI> sceneTexts;

        [MenuItem(RichEditorUtility.WindowMenuName + "Font Setter Window")]
        private static void Init()
        {
            Instance = EditorWindow.GetWindow<FontSetterWindow>(false, "FontSetterWindow", true);
            Instance.Show();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //gather all elements
            if (sceneTexts == null)
                sceneTexts = new List<TextMeshProUGUI>();
        }

        protected override void OnDisable()
        {
            //release dynamic memory to prevent Editor bloat
            sceneTexts = null;
            base.OnDisable();
        }

        [PropertySpace(22)]
        [Button(ButtonSizes.Large)]
        public void GatherRefs()
        {
            //gather all elements
            if (sceneTexts == null || sceneTexts.Count == 0)
                sceneTexts.AddRange(FindObjectsOfType<TextMeshProUGUI>());
        }


        [Button(ButtonSizes.Large, Style = ButtonStyle.Box)]
        public void GatherFromChildren(Transform parent)
        {
            sceneTexts.Clear();

            sceneTexts.AddRange(parent.GetComponentsInChildren<TextMeshProUGUI>());
        }

        [Button(ButtonSizes.Large), GUIColor(0, 1, 0.2f)]
        public void Modify()
        {
            //validate
            GuardAgainst.ArgumentIsNull(workingFont);

            //log
            Debug.Log($"Setting font {workingFont.name}.", workingFont);

            //work
            var textsCount = sceneTexts.Count;

            for (var i = 0; i < textsCount; ++i)
            {
                var text = sceneTexts[i];
                text.font = workingFont;
                EditorUtility.SetDirty(text);
                Debug.Log($"-Modified font on {text.name}.", text);
            }
        }
    }
}
