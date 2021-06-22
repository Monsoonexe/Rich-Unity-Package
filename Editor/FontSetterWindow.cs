using UnityEngine;
using UnityEditor;
using TMPro;

/// <summary>
/// I help assign a Font Asset to all TextMeshPro objects in a Scene.
/// </summary>
public class FontSetterWindow : EditorWindow
{
	public static FontSetterWindow Instance { get; private set; }

    private TMP_FontAsset workingFont;

    private TextMeshProUGUI[] sceneTexts;
    
    //main
    private void OnGUI()
    {
        //draw field for working font
        workingFont = EditorGUILayout.ObjectField(//draw an object field
            "New Font: ", //label
            workingFont, //give it this item to start with
            typeof(TMP_FontAsset), //restrict to type
            allowSceneObjects: false)
            as TMP_FontAsset; // cast

        if(workingFont && GUILayout.Button("Change Fonts on all TMP Texts"))
        {
            //gather all elements
            if (sceneTexts == null || sceneTexts.Length == 0)
                sceneTexts = FindObjectsOfType<TextMeshProUGUI>();

            var textsCount = sceneTexts.Length;

            for(var i = 0; i < textsCount; ++i)
            {
                var text = sceneTexts[i];
                text.font = workingFont;
            }

            SceneView.RepaintAll(); // refresh visuals with new fonts
            Debug.LogFormat("Changed Font on {0} TMPText elements to {1}",
                textsCount, workingFont.name);
        }
        EditorGUILayout.LabelField("If you don't see a change after a moment,");
        EditorGUILayout.LabelField("resize a window and the UI will be repainted.");
    }
    
    private void OnEnable()
    {
        //gather all elements
        if (sceneTexts == null)
            sceneTexts = FindObjectsOfType<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        //release dynamic memory to prevent Editor bloat
        sceneTexts = null;
    }

    [MenuItem("RichUtilities/Font Setter Window")]
	private static void Init()
	{
		Instance = EditorWindow.GetWindow<FontSetterWindow>(true, "FontSetterWindow", true);
		Instance.Show();
	}
}
