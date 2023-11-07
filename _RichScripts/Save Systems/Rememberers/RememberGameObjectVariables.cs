using RichPackage.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// 
    /// </summary>
    public class RememberGameObjectVariables : ASaveableMonoBehaviour<RememberGameObjectVariables.Memento>
    {
        [Required]
        public GameObjectVariables target;

        protected override void Reset()
        {
            SetDevDescription("Saves/loads the properties of the target variables component.");
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: false);
        }

        #region Save/Load

        protected override void SaveStateInternal()
        {
            saveData.table.Clear();
            target.Table.CopyTo(saveData.table);
        }

        protected override void LoadStateInternal()
        {
            target.Table.Clear();
            target.Table.CopyFrom(saveData.table);
        }

        /// <summary>
        /// The save data structure.
        /// </summary>
        [System.Serializable]
        public class Memento : AState
        {
            public VariableTable table = new VariableTable();
        }

        #endregion Save/Load
    }
}
