/* TODO - cache string keys
 * assert components are children of target
 */

using Sirenix.OdinInspector;
using System.Collections.Generic;
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

        [Required]
        public List<Component> components = new List<Component>();

        [Title("Settings")]
        public bool rememberActive = false;
        public bool rememberName = false;
        public bool rememberTag = false;
        public bool rememberLayer = false;

        protected override void Reset()
        {
            SetDevDescription("Saves/loads properties of a GameObject.");
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: true, includeName: true, includeType: true);

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

        public override void SaveState(ISaveStore saveFile)
        {
            if (target == null)
                return;

            base.SaveState(saveFile);
            SaveComponents(saveFile);
        }

        public override void LoadState(ISaveStore saveFile)
        {
            if (!saveFile.KeyExists(SaveID))
                return;

            saveFile.LoadInto(SaveID, SaveData);
            LoadStateInternal();
            LoadComponents(saveFile);
        }

        private void SaveComponents(ISaveStore saveFile)
        {
            string prefix = GetKeyPrefix();
            for (int i = 0; i < components.Count; i++)
            {
                Component c = components[i];
                UnityEngine.Assertions.Assert.IsNotNull(c);

                string key = prefix + c.GetType().Name; // TODO - cache keys
                saveFile.Save(key, (object)c); // let ES3 figure out the concrete type
            }
        }

        private void LoadComponents(ISaveStore saveFile)
        {
            string prefix = GetKeyPrefix();
            for (int i = 0; i < components.Count; i++)
            {
                Component c = components[i];
                UnityEngine.Assertions.Assert.IsNotNull(c);

                string key = prefix + c.GetType().Name; // TODO - cache keys
                saveFile.LoadInto(key, (object)c); // let ES3 figure out the concrete type
            }
        }

        private string GetKeyPrefix() => SaveID + "-";

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
