using UnityEngine;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

namespace RichPackage.Audio
{
    /// <summary>
    /// A throw-it-together class to get music playing.
    /// </summary>
    public class SceneMusic : RichMonoBehaviour
    {
        [Header("---Resources---")]
        [Required]
        [Expandable]
        public AudioClipVariable music;

        public AudioClip Clip
        {
            get => music.Value;
            set => music.Value = value;
        }

        //runtime data
        private AudioID audioID;

        private void Reset()
        {
            SetDevDescription("A throw-it-together class to get music playing.");
        }

        private void OnEnable()
        {
            PlaySound();
        }

        private void OnDisable()
        {
            StopSound();
        }

        public void PlaySound()
        {
            audioID = AudioManager.PlayBackgroundTrack(
                music, music.Options);
        }

        public void StopSound()
        {
            AudioManager.StopSFX(audioID);
        }
    }
}
