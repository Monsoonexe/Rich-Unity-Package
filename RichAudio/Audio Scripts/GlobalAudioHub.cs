using UnityEngine;

namespace RichPackage.Audio
{
    public class GlobalAudioHub : AudioHub
    {
        private static GlobalAudioHub instance;

        protected override void Awake()
        {
            base.Awake();
            if (!InitSingleton(this, ref instance, dontDestroyOnLoad: isPersistentThroughScenes))
            {
                Destroy(this);
                return;
            }
        }

        public static void PlayGlobalSFX(string clipTag)
        {
            Debug.Assert(instance != null,
                "[GlobalAudioHub] No instance in Scene.");
            instance.PlayAudioClipSFX(clipTag);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Audio Manager/GlobalAudioHub")]
#endif
        public static void ConstructGlobal()
        {
            if (instance != null)
            {
                Debug.Log("[GlobalAudioHub] GlobalAudioHub already Scene.", instance);
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
