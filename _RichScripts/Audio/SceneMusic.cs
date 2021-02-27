using UnityEngine;

namespace ProjectEmpiresEdge.Audio
{
    public class SceneMusic : RichMonoBehaviour
    {
        [Header("---Scene Audio---")]
        [SerializeField] private AudioClip music;

        [SerializeField] private bool playOnAwake = false;

        [SerializeField] private AudioOptions options = new AudioOptions();

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
