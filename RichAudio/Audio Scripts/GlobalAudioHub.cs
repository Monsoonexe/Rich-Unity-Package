using UnityEngine;

namespace RichPackage.Audio
{
    public class GlobalAudioHub : AudioHub
    {
        private static GlobalAudioHub s_instance;

        protected override void Awake()
        {
            base.Awake();
            Singleton.TakeOrDestroy(this, ref s_instance,
                dontDestroyOnLoad: isPersistentThroughScenes);
        }

        private void OnDestroy()
        {
            Singleton.Release(this, ref s_instance);
        }

        public static void PlayGlobalSFX(string clipTag)
        {
            Debug.Assert(s_instance != null,
                "[GlobalAudioHub] No instance in Scene.");
            s_instance.PlayAudioClipSFX(clipTag);
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/RichPackage/Audio/GlobalAudioHub")]
#endif
        public static void ConstructGlobal()
        {
            if (s_instance != null)
            {
                Debug.Log($"[{nameof(GlobalAudioHub)}] GlobalAudioHub already Scene.",
                    s_instance);
                return;
            }
            //set name
            string newName = null;

#if UNITY_EDITOR
            newName = "Global AudioHub";
#endif
            s_instance = Construct<GlobalAudioHub>(newName);
        }

    }
}
