using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Saves/loads the properties of a <see cref="Light"/>
    /// </summary>
    public class RememberLighting : ASaveableMonoBehaviour<RememberLighting.Memento>
    {
        [Required]
        public Light target;

        [Title("Settings")]
        public bool rememberColor = false;
        public bool rememberIntensity = false;
        // public bool rememberIndirectMultiplier = false;
        public bool rememberRange = false;
        public bool rememberEnabled = false;
        public bool rememberGameObjectActive = false;

        #region Unity Messages

        protected override void Reset()
        {
            SetDevDescription("Saves/loads the properties of the target light.");
            SaveID = UniqueIdUtilities.CreateIdFrom(this, true);

            // set default
            target = GetComponent<Light>();
        }

        #endregion Unity Messages

        protected override void LoadStateInternal()
        {
            if (rememberColor)
                target.color = SaveData.color;
            if (rememberIntensity)
                target.intensity = SaveData.intensity;
            if (rememberRange)
                target.range = SaveData.range;
            if (rememberEnabled)
                target.enabled = SaveData.enabled;
            if (rememberGameObjectActive)
                target.gameObject.SetActive(SaveData.gameObjectActive);
        }

        protected override void SaveStateInternal()
        {
            if (rememberColor)
                SaveData.color = target.color;
            if (rememberIntensity)
                SaveData.intensity = target.intensity;
            if (rememberRange)
                SaveData.range = target.range;
            if (rememberEnabled)
                SaveData.enabled = target.enabled;
            if (rememberGameObjectActive)
                SaveData.gameObjectActive = target.gameObject.activeSelf;
        }

        public class Memento : AState
        {
            public Color color;
            public float intensity;
            // public float indirectMultiplier;
            public float range;
            public bool enabled;
            public bool gameObjectActive;
        }
    }
}
