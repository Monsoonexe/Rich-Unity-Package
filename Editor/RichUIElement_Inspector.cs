using UnityEngine;
using UnityEditor;

namespace ProjectEmpiresEdge.CustomInspector
{
    [CustomEditor(typeof(RichUIElement), true)]
    public class RichUIElement_Inspector : Editor
    {
        public const string showButtonName = "Show";
        public const string hideButtonName = "Hide";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var selectedTarget = target as RichUIElement;
            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button(showButtonName))
            {
                selectedTarget.Show();
            }

            else if (GUILayout.Button(hideButtonName))
            {
                selectedTarget.Hide();
            }

            GUILayout.EndHorizontal();
        }
    }
}