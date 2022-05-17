using System.Text;
using UnityEngine;
using ScriptableObjectArchitecture;

namespace RichPackage.InventorySystem
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ItemStack"/>
    [CreateAssetMenu(fileName = "_Item",
                    menuName = "ScriptableObjects/Item")]
    public class Item : RichScriptableObject//data
    {   //please update Constructors when adding properties!
        public const int MAX_STACK_AMOUNT = 9999;

        [Header("---Item---")]
        [SerializeField]
        [Tooltip("Unique id that identifies item (every 'sword of fallen' will have same id).")]
        protected string id;
        /// <summary>
        /// Unique id that identifies item (every 'sword of fallen' will have same id).
        /// </summary>
        public string ID { get { return id; } } // public readonly serialized property  

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
        protected string itemDescription = "Please enter an in-game description.";
        /// <summary>
        /// "General player-facing description (a potion that makes you invisible)."
        /// </summary>
        public string ItemDescription { get => itemDescription; }

        [SerializeField]
        [Tooltip("Sprite that represents like items in Inventory.")]
        protected Sprite icon;
        /// <summary>
        /// Sprite that represents like items in Inventory.
        /// </summary>
        public Sprite Icon { get => icon; }

        [SerializeField]
        [Tooltip("The Item's physical form, if it were to be instantiated.")]
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

        [Header("---Audio---")]
        [SerializeField]
        protected AudioClipReference itemAudio;
        public AudioClipReference ItemAudio { get => itemAudio; }

        /// <summary>
        /// Special, itemized description of item.
        /// </summary>
        /// <returns></returns>
        public virtual string GetDescription()
        {
            return itemDescription;
        }

        #region Constructors

        /// <summary>
        /// This should be treated like the 'new' operator. Creates a run-time instance.
        /// To create a .asset, consider Editor_Construct()
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        /// <param name="sprite"></param>
        /// <param name="maxStack"></param>
        /// <param name="clip"></param>
        public static Item Construct(string name, string description,
            Sprite sprite, int maxStack, AudioClipReference clip,
            GameObject itemPrefab)
        {
            var newItem = CreateInstance<Item>(); //new

            newItem.itemName = name;
            newItem.itemDescription = description;
            newItem.icon = sprite;
            newItem.maximumStack = maxStack;
            newItem.itemAudio = clip;
            newItem.itemPrefab = itemPrefab;

            return newItem;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="itemToCopy"></param>
        /// <returns>Copy of given item</returns>
        public static Item Construct(Item itemToCopy)
        {
            var newItem = CreateInstance<Item>();

            newItem.itemName = itemToCopy.itemName;
            newItem.itemDescription = itemToCopy.itemDescription;
            newItem.icon = itemToCopy.icon;
            newItem.maximumStack = itemToCopy.maximumStack;
            newItem.itemAudio = itemToCopy.itemAudio;
            newItem.itemPrefab = itemToCopy.itemPrefab;

            return newItem;
        }

        #endregion

    #if UNITY_EDITOR//AssetDatabase is Editor only

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnValidate()
        {
            if(string.IsNullOrEmpty(id))
                id = GetInstanceID().ToString();
        }

    #endif

    }
}
