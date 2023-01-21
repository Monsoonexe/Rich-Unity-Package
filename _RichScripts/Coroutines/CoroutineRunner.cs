using UnityEngine;

namespace RichPackage.Coroutines
{
    /// <summary>
    /// A cohort whose only purpose is to run coroutines.
    /// </summary>
    public class CoroutineRunner : RichMonoBehaviour
    {
        protected override void Reset()
        {
            SetDevDescription("My only job is to run coroutines for those that cannot!");
        }

        public bool ScenePersistent { get; private set; }

        /// <summary>
        /// Creates a new instance of a <see cref="CoroutineRunner"/>.
        /// </summary>
        /// <param name="scenePersistent">Should this be set to not destroy on load?</param>
        /// <returns>A new instance.</returns>
        public static CoroutineRunner CreateNew(bool scenePersistent)
        {
            var go = new GameObject(nameof(CoroutineRunner));
            
            if (scenePersistent)
                DontDestroyOnLoad(go);

            // create and initialize
            var instance = go.AddComponent<CoroutineRunner>();
            instance.ScenePersistent = scenePersistent;

            return instance;
        }
    }
}
