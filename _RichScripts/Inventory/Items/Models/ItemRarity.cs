using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.InventorySystem
{
    public class ItemRarity : RichScriptableObject
    {
        [field: SerializeField, LabelText(nameof(Id))]
        public UniqueID Id { get; private set; }

        [field: SerializeField, LabelText(nameof(Name))]
        public string Name { get; private set; }

        [field: SerializeField, LabelText(nameof(Color))]
        public Color Color { get; private set ; } = Color.white;

        protected virtual void Reset()
        {
            Name = name.Split('_').Last();
            Id = new UniqueID(Name);
        }
    }
}
