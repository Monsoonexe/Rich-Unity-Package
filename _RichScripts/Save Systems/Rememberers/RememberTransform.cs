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
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeName: true, includeType: true);

            // set default
            target = transform;
        }

        protected override void LoadStateInternal()
        {
            target.SetPositionAndRotation(SaveData.properties);
        }

        protected override void SaveStateInternal()
        {
            SaveData.properties = target.GetPositionAndRotation();
        }

        [System.Serializable]
        public class Memento : AState
        {
            public TransformProperties properties;
        }

        #region Editor
#if UNITY_EDITOR

        [UnityEditor.MenuItem("CONTEXT/" + nameof(Transform) + "/Add Rememberer")]
        private static void AddRememberer(UnityEditor.MenuCommand command)
        {
            var t = (Transform)command.context; // the thing clicked on
            t.gameObject.AddComponent<RememberTransform>()
                .target = t; // assign this thing as the thing to be saved
        }

#endif
        #endregion Editor
    }
}
