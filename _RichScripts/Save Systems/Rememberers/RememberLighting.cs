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
        public bool rememberRange = false;
        public bool rememberEnabled = false;

        #region Unity Messages

        protected override void Reset()
        {
            SetDevDescription("Saves/loads the properties of the target light.");
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: true, includeName: true, includeType: true);

            // set default
            target = GetComponentInChildren<Light>();
        }

        #endregion Unity Messages

        #region Save/Load

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
        }

        [System.Serializable]
        public class Memento : AState
        {
            public Color color;
            public float intensity;
            public float range;
            public bool enabled;
        }

#endregion Save/Load

        #region Editor
#if UNITY_EDITOR

        [UnityEditor.MenuItem("CONTEXT/" + nameof(Light) + "/Add Rememberer")]
        private static void AddRememberer(UnityEditor.MenuCommand command)
        {
            var t = (Light)command.context; // the thing clicked on
            t.gameObject.AddComponent<RememberLighting>()
                .target = t; // assign this thing as the thing to be saved
        }

#endif
        #endregion Editor
    }
}
