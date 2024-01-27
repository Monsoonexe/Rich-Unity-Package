using UnityEditor;
using UnityEngine;

namespace RichPackage.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public class Transform_ContextMenu
    {
        [MenuItem("CONTEXT/Transform/Alphabetize Children")]
        private static void AlphabetizeChildren(MenuCommand command)
        {
            var t = command.context.CastTo<Transform>();
            t.AlphabetizeChildren();
            EditorUtility.SetDirty(t);
        }
    }
}
