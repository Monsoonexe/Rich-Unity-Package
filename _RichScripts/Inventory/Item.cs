using RichPackage.Audio;
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
        public const int MAX_STACK_AMOUNT = 9999;

        [Title("Item")]
        [SerializeField]
        [Tooltip("Unique id that identifies item (every 'sword of fallen' will have same id).")]
        protected UniqueID id;

        /// <summary>
        /// Unique id that identifies item (every 'sword of fallen' will have same id).
        /// </summary>
        public UniqueID ID { get { return id; } } // public readonly serialized property  

        [SerializeField]
        [Tooltip("Usually key or quest items cannot be destroyed.")]
        protected bool isDestroyable = true;
        public bool IsDestroyable { get => isDestroyable; }

        [SerializeField]
        [Tooltip("Player-facing item name (Sword of the Fallen).")]
        protected string itemName = "Holy Hand Grenade";
        /// <summary>
        /// "Player-facing item name (Sword of the Fallen)"
        /// </summary>
        public string ItemName { get => itemName; }

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
        /// </summary>
        public Sprite Icon { get => icon; }

        [SerializeField, Tooltip("The Item's physical form, if it were to be instantiated.")]
        private GameObject itemPrefab;
        /// <summary>
        /// The Item's physical form, if it were to be instantiated.
        /// </summary>
        public GameObject ItemPrefab { get => itemPrefab; }

        [Range(1, MAX_STACK_AMOUNT)]
        [SerializeField]
        [Tooltip("How many of this item can be stacked in Inventory? (usually Tools and Weapons are 1.")]
        protected int maximumStack = MAX_STACK_AMOUNT;
        /// <summary>
        /// "How many of this item can be stacked in Inventory? (usually Tools and Weapons are 1."
        /// </summary>
        public virtual int MaximumStacks { get => maximumStack; } // public readonly serialized property    

        [Title("Audio")]
        [SerializeField]
        protected RichAudioClipReference itemAudio;
        public RichAudioClipReference ItemAudio { get => itemAudio; }

        /// <summary>
        /// Special, itemized description of item.
        /// </summary>
        /// <returns></returns>
        public virtual string GetDescription()
        {
            return description;
        }

    #if UNITY_EDITOR//AssetDatabase is Editor only

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
                id = UniqueID.New;
        }

    #endif

    }
}
