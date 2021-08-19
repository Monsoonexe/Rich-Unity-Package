using UnityEngine;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

namespace ProjectEmpiresEdge.Audio
{
    public class SceneMusic : RichMonoBehaviour
    {
        [Header("---Resources---")]
        [Required]
        public AudioClipReference music;
        public AudioClip Clip {get => music.Value; set => music.Value = value;}

        [Header("---Settings---")]
        public bool playOnAwake = true;

        public AudioOptions options = AudioOptions.DefaultBGM;

        private AudioID audioID;

        private void Start()
        {
            if (playOnAwake)
            {
                PlaySound();
            }
        }

        private void OnDisable()
        {
            StopSound();
        }

        public void PlaySound()
        {
            audioID = AudioManager.PlayBackgroundTrack(music, options);
        }

        public void StopSound()
        {
            AudioManager.StopSFX(audioID);
        }
    }
}
