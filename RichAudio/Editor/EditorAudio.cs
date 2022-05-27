using UnityEngine;
using Sirenix.OdinInspector;
using RichPackage.Audio;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RichPackage.Editor
{
    /// <summary>
    /// 
    /// </summary>
    [System.Obsolete("EditorAudio doesn't work.")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(AudioSource))]
    public class EditorAudio : RichMonoBehaviour
    {
        private static EditorAudio Instance;
        private AudioSource myAudioSource;

        [InlineProperty, HideLabel, Title(nameof(testClip))]
        public RichAudioClipReference testClip = new RichAudioClipReference();

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I help play audio in the Editor!" +
                " Make sure I'm tagged 'EditorOnly'.");
            myAudioSource = GetComponent<AudioSource>();
            Instance = this;
        }

        protected override void Awake()
        {
            base.Awake();

#if !UNITY_EDITOR
            //should be tagged EditorOnly so this doesn't happen.
            Destroy(this.gameObject);
            return;
#endif
            Instance = this;
            myAudioSource = gameObject.GetComponentIfNull(myAudioSource);
        }


#if UNITY_EDITOR
        [MenuItem("RichUtilities/Audio/EditorAudio")]
#endif
        public static void CreateInstance()
        {
            Instance = new GameObject(typeof(EditorAudio).Name)
                .AddComponent<EditorAudio>();
            Instance.tag = "EditorOnly";
            Instance.myAudioSource = Instance.gameObject.GetComponent<AudioSource>();
        }

        [Button, HorizontalGroup("A")]
        private void Play()
        {
            myAudioSource.PlayOneShot(testClip);
        }

        [Button, HorizontalGroup("A")]
        private void Stop()
		{
            myAudioSource.Stop();
		}
	}
}
