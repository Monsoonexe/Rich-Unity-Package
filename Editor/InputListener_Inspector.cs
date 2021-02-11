using UnityEngine;
using UnityEditor;

namespace ProjectEmpiresEdge.CustomInspector
{
    [CustomEditor(typeof(InputListenerBase), true)]
    public class InputListener_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //only available in playMode
            if (EditorApplication.isPlaying)
            {
                var selectedTarget = target as InputListenerBase;

                GUILayout.Space(10);

                if (GUILayout.Button("Trigger"))
                {
                    selectedTarget.PerformAction();
                }
            }
        }
    }
}
