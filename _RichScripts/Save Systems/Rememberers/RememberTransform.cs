using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Saves/loads the properties of a <see cref="Transform"/>.
    /// </summary>
    public class RememberTransform : ASaveableMonoBehaviour<RememberTransform.Memento>
    {
        [Required]
        public Transform target;

        protected override void Reset()
        {
            SetDevDescription("Saves/loads the properties of the target transform.");
            SaveID = UniqueIdUtilities.CreateIdFrom(this, true);

            // set default
            target = transform;
        }

        protected override void LoadStateInternal()
        {
            SaveData.properties.Load(target); // from memento into object
        }

        protected override void SaveStateInternal()
        {
            SaveData.properties.Store(target); // from object to memento
        }

        [System.Serializable]
        public class Memento : AState
        {
            public TransformProperties properties;
        }
    }
}
