using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Saves/Loads game variables that exist as a component on a game object.
    /// </summary>
    /// <seealso cref="GameObjectVariables"/>
    public class RememberGameObjectVariables : ASaveableMonoBehaviour
    {
        [SerializeField]
        protected UniqueID saveId;

        [Required, Tooltip("The thing that gets remembered.")]
        public GameObjectVariables target;

        protected override void Reset()
        {
            SetDevDescription("Saves/loads the properties of the target variables component.");
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: false);
            target = GetComponentInChildren<GameObjectVariables>();
        }

        #region Save/Load

        public override UniqueID SaveID { get => saveId; protected set => saveId = value; }

        public override void LoadState(ISaveStore saveFile)
        {
            if (saveFile.KeyExists(SaveID))
            {
                target.Table.Clear();
                saveFile.LoadInto(SaveID, target.Table);
            }
        }

        public override void SaveState(ISaveStore saveFile)
        {
            saveFile.Save(SaveID, target.Table);
        }

        #endregion Save/Load
    }
}
