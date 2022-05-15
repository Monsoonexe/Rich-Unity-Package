using UnityEngine;
using NaughtyAttributes;

namespace RichPackage.DiceSystem
{
    [CreateAssetMenu(fileName = "ScriptableDie",
        menuName = "ScriptableObjects/Die")]
    public class ScriptableDie : RichScriptableObject<Die>
    {
        //Die already exists on superclass.

        [SerializeField]
        private string dieName = "die name";
        public string DieName { get => dieName; }

        [SerializeField]
        [Required]
        [ShowAssetPreview]
        private GameObject diePrefab = null;
        public GameObject Prefab { get => diePrefab; }

        public static implicit operator GameObject(ScriptableDie a)
            => a.diePrefab;

        //can also add weighted pair version
    }
}
