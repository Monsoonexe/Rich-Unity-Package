using RichPackage.Collections;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Variables that get saved to a <see cref="GameObject"/>.
    /// </summary>
    public class GameObjectVariables : RichMonoBehaviour
    {
        [SerializeField]
        private VariableTable table;

        public VariableTable Table => table;

        #region Editor
#if UNITY_EDITOR

        [UnityEditor.MenuItem("CONTEXT/" + nameof(GameObjectVariables) + "/Add Rememberer")]
        private static void AddRememberer(UnityEditor.MenuCommand command)
        {
            var g = (GameObjectVariables)command.context; // the thing clicked on
            g.gameObject.AddComponent<RememberGameObjectVariables>()
                .target = g; // assign this thing as the thing to be saved
        }

#endif
#endregion Editor
    }
}
