using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.InventorySystem.Items
{
    /// <summary>
    /// Base class for performing item use logic.
    /// </summary>
    public abstract class AItemUseLogic : RichScriptableObject, IItemUseLogic
    {
        [field: SerializeField, LabelText(nameof(Name))]
        public string Name { get; private set; }

        public abstract void Use(Item item); // TODO - whatever
    }
}
