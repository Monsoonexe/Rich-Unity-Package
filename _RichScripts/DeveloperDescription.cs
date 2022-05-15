using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Sticky-note-like MonoBehaviour that only lives in Editor.
    /// </summary>
    public class DeveloperDescription : MonoBehaviour
    {
        [TextArea, SerializeField]
        private string developerDescription;

        private void Awake()
        {
    #if !UNITY_EDITOR
            Destroy(this);
    #endif
        }
    }
}
