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
    }
}
