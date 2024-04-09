using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Saves/loads properties of a GameObject.
    /// </summary>
    /// <remarks>Note that the automatic behaviour of subscribing to events doesn't work if this object starts its life disabled.</remarks>
    public sealed class RememberGameObject : ASaveableMonoBehaviour<RememberGameObject.Memento>
    {
        [Required]
        public GameObject target;

        [Title("Settings")]
        public bool rememberActive = false;
        public bool rememberName = false;
        public bool rememberTag = false;
        public bool rememberLayer = false;

        protected override void Reset()
        {
            SetDevDescription("Saves/loads properties of a GameObject.");
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: true, includeType: true);

            // set default
            target = gameObject;
        }

        #region Save/Load

        protected override void LoadStateInternal()
        {
            if (rememberActive)
                target.SetActive(SaveData.activeSelf);
            if (rememberName)
                target.name = SaveData.name;
            if (rememberTag)
                target.tag = SaveData.tag;
            if (rememberLayer)
                target.layer = SaveData.layer;
        }

        protected override void SaveStateInternal()
        {
            if (rememberActive)
                SaveData.activeSelf = target.activeSelf;
            if (rememberName)
                SaveData.name = target.name;
            if (rememberTag)
                SaveData.tag = target.tag;
            if (rememberLayer)
                SaveData.layer = target.layer;
        }

        [System.Serializable]
        public class Memento : AState
        {
            public bool activeSelf;
            public int layer;
            public string name;
            public string tag;
        }

        #endregion Save/Load
    }
}
