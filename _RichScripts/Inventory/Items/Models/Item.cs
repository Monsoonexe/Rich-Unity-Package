using RichPackage.Audio;
using RichPackage.InventorySystem.Currency;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.InventorySystem
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ItemStack"/>
    public partial class Item : RichScriptableObject
    {
        /// <summary>
        /// The maximum anything can be stacked.
        /// </summary>
        public const int MaxStackSize = 9999;

        [Title("Item")]
        [SerializeField]
        [Tooltip("Unique id that identifies item (every 'sword of fallen' will have same id).")]
        protected UniqueID id;

        /// <summary>
        /// Unique id that identifies item (every 'sword of fallen' will have same id).
        /// </summary>
        public UniqueID Id { get { return id; } } // public readonly serialized property

        [field: SerializeField, LabelText(nameof(SingleName), true)]
        public string SingleName { get; private set; } = "Enter an item name";

        [field: SerializeField, LabelText(nameof(PluralName), true)]
        public string PluralName { get; private set; } = "Enter an item name";

        [TextArea(1, 60)]
        [SerializeField]
        [Tooltip("General player-facing description (a potion that makes you invisible).")]
        protected string description = "Please enter an in-game description.";

        /// <summary>
        /// "General player-facing description (a potion that makes you invisible)."
        /// </summary>
        public string Description { get => description; }

        [SerializeField]
        [Tooltip("Sprite that represents like items in Inventory.")]
        protected Sprite icon;

        /// <summary>
        /// Sprite that represents like items in Inventory.
        /// //
        /// </summary>
        public Sprite Icon { get => icon; }

        [SerializeField, Tooltip("The Item's physical form, if it were to be instantiated.")]
        private GameObject itemPrefab;

        /// <summary>
        /// The Item's physical form, if it were to be instantiated.
        /// </summary>
        public GameObject ItemPrefab { get => itemPrefab; }

        [Title("Settings")]
        [SerializeField]
        protected bool isUnique = false;
        public bool IsUnique { get => isUnique; }

        [SerializeField]
        protected bool isKeyItem = false;
        public bool IsKeyItem { get => isKeyItem; }

        [SerializeField]
        [Tooltip("Usually key or quest items cannot be destroyed.")]
        protected bool isDroppable = true;
        public bool IsDroppable { get => isDroppable; }

        [Range(1, MaxStackSize)]
        [SerializeField]
        [Tooltip("How many of this item can be stacked in Inventory? (usually Tools and Weapons are 1.")]
        protected int maximumStack = MaxStackSize;

        /// <summary>
        /// "How many of this item can be stacked in Inventory? (usually Tools and Weapons are 1."
        /// </summary>
        public virtual int MaximumStacks { get => maximumStack; } // public readonly serialized property    

        [field: SerializeField, LabelText(nameof(Rarity))]
        public ItemRarity Rarity { get; private set; }

        [Title("Currency")]
        [field: SerializeField, LabelText(nameof(BuyPrice))]
        public CurrencyAmount BuyPrice { get; private set; }

        [field: SerializeField, LabelText(nameof(SellPrice))]
        public CurrencyAmount SellPrice { get; private set; }

        [Title("Audio")]
        [SerializeField]
        protected RichAudioClipReference itemAudio;
        public RichAudioClipReference ItemAudio { get => itemAudio; }

        protected virtual void Reset()
        {
            id = new UniqueID(name);
        }

        public override string ToString() => SingleName + " " + Id.ToString();

#if UNITY_EDITOR//AssetDatabase is Editor only

        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                id = UniqueID.New;
        }

    #endif

    }
}
