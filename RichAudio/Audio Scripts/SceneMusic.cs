using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
    /// <summary>
    /// A throw-it-together class to get music playing.
    /// </summary>
    public class SceneMusic : RichMonoBehaviour
    {
        [Title("Options")]
        public float crossfade = 1.0f;

        [Title("Resources")]
        [Required]
        public RichAudioClip music;

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
            AudioManager.StopSFX(audioID, crossfade);
        }
    }
}
