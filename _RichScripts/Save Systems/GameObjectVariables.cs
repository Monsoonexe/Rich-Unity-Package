using RichPackage.Collections;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Variables that get saved to a <see cref="GameObject"/>.
    /// </summary>
    public class GameObjectVariables : RichMonoBehaviour
    {
        protected const string ContextMenu = "CONTEXT/" + nameof(GameObjectVariables) + "/";

        [SerializeField]
        private VariableTable table;

        public VariableTable Table
        {
            get => table;
            protected set => table = value;
        }

        #region Editor
#if UNITY_EDITOR

        [UnityEditor.MenuItem(ContextMenu + "Add Rememberer")]
        protected static void AddRememberer(UnityEditor.MenuCommand command)
        {
            var g = (GameObjectVariables)command.context; // the thing clicked on
            g.gameObject.AddComponent<RememberGameObjectVariables>()
                .target = g; // assign this thing as the thing to be saved
        }

        [UnityEditor.MenuItem(ContextMenu + "Clear Table")]
        private static void ContextMenuClearTable(UnityEditor.MenuCommand command)
        {
            var g = (GameObjectVariables)command.context; // the thing clicked on
            g.table.Clear();
        }

#endif
#endregion Editor
    }
}
