using RichPackage.GuardClauses;
using UnityEngine;
using UnityEngine.Assertions;

namespace RichPackage.SaveSystem
{
    /// <seealso cref="RememberTransform"/>
    public static class RememberUtilities
    {
        /// <summary>
        /// Save all components on and under <paramref name="obj"/>.
        /// </summary>
        public static void Save(GameObject obj, bool recursive = false)
        {
            GuardAgainst.ArgumentIsNull(obj, nameof(obj));
            
            var saveSystem = SaveSystem.Instance;
            Assert.IsTrue(saveSystem, "Expected a save system to exist!");
            
            foreach (var saveable in obj.EnumerateComponents<ISaveable>(recursive))
            {
                saveSystem.Save(saveable);
            }
        }

        /// <summary>
        /// Load all components on and under <paramref name="obj"/>.
        /// </summary>
        public static void Load(GameObject obj, bool recursive = false)
        {
            GuardAgainst.ArgumentIsNull(obj, nameof(obj));

            var saveSystem = SaveSystem.Instance;
            Assert.IsTrue(saveSystem, "Expected a save system to exist!");

            foreach (var saveable in obj.EnumerateComponents<ISaveable>(recursive))
            {
                saveSystem.Load(saveable);
            }
        }
    }
}
