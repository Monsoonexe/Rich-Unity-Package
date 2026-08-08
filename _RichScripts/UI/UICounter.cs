using RichPackage.Pooling;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace RichPackage.UI
{
    /// <summary>
    /// Shows up to MAX icons. If VALUE > MAX, show a single icon
    /// and an additional UI element (e.g. X5 lives). 
    /// Like the life counter in Mario.
    /// Pairs well with LayoutGroups.
    /// </summary>
    /// <seealso cref="SpawnCounter"/>
    [RequireComponent(typeof(GameObjectPool))]
    public class UICounter : VariableUIElement<IntVariable>
    {
        /// <summary>
        /// If 'value' is greater than this, only spawn 1 
        /// and enable UI element.
        /// </summary>
        [Title("Options")]
        [Min(1)]
        [Tooltip("If 'value' is greater than this, only spawn 1 and enable UI element.")]
        public int maxSpawns = 5;

        public string prefix = "X";
        public string suffix = " lives";

        [Title("Scene Refs")]
        [SerializeField, Required]
        private TextMeshProUGUI textReadout;

        // member Components
        [SerializeField, Required]
        private GameObjectPool objectPool;
        private readonly List<GameObject> activeItems = new List<GameObject>();

        protected override void Reset()
        {
            SetDevDescription("Shows up to MAX icons. If VALUE > MAX, show a single icon. " +
                "and an additional UI element (e.g. X5 lives). " +
                "Like the life counter in Mario.");

            objectPool = GetComponent<GameObjectPool>();

            // configure objectPool
            objectPool.startingAmount = maxSpawns; //spawn all at once
            objectPool.maxAmount = maxSpawns;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
        }

        protected override void Awake()
        {
            base.Awake();

            // configure objectPool
            objectPool.InitPool(maxSpawns, maxSpawns);
        }

        public override void UpdateUI()
        {
            // which display method?
            if (targetData.Value > maxSpawns)
            {
                // setup text element like: X5 lives
                textReadout.text = StringBuilderCache.Rent()
                    .Append(prefix)
                    .Append(targetData.Value.ToStringCached())
                    .Append(suffix)
                    .ToStringAndReturn();

                // make change from AAAAAAAAA to A x5 lives
                if (activeItems.Count != 1) // only need to do this once 
                {
                    while (activeItems.Count > 0)
                    {
                        Release(activeItems.Last());
                    }

                    //show a single item
                    var item = objectPool.Depool();
                    activeItems.Add(item);
                    textReadout.enabled = true;
                }
            }
            else //standard method
            {
                textReadout.enabled = false;
                
                // need to create more
                while (targetData.Value > activeItems.Count)
                {
                    GameObject item = objectPool.Depool(); // (spawn)
                    if (item == null)
                        break; // pool is exhausted
                    activeItems.Add(item);
                }

                // have too many
                while (targetData.Value < activeItems.Count)
                {
                    Release(activeItems.Last());
                }
            }
        }

        private void Release(GameObject obj)
        {
            objectPool.Enpool(obj); // return to pool (despawn)
            activeItems.Remove(obj);
        }
    }
}
