using UnityEngine;

namespace RichPackage.Audio
{
    public class GlobalAudioHub : AudioHub
    {
        private static GlobalAudioHub instance;

        protected override void Awake()
        {
            base.Awake();
            Singleton.TakeOrDestroy(this, ref instance,
                dontDestroyOnLoad: isPersistentThroughScenes);
        }

        private void OnDestroy()
        {
            Singleton.Release(this, ref instance);
        }

        public static void PlayGlobalSFX(string clipTag)
        {
            Debug.Assert(instance != null,
                "[GlobalAudioHub] No instance in Scene.");
            instance.PlayAudioClipSFX(clipTag);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("RichUtilities/Audio/GlobalAudioHub")]
#endif
        public static void ConstructGlobal()
        {
            if (instance != null)
            {
                Debug.Log($"[{nameof(GlobalAudioHub)}] GlobalAudioHub already Scene.",
                    instance);
                return;
            }
            //set name
            string newName = null;

#if UNITY_EDITOR
            newName = "Global AudioHub";
#endif
            instance = Construct<GlobalAudioHub>(newName);
        }

    }
}
