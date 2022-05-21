using UnityEngine;

namespace RichPackage.Audio
{
    /// <summary>
    /// A Mono that will play audio from static AudioManager.
    /// Helpful for rigging events and prefabs to audio that aren't
    /// yet aware of an AudioManager in a scene.
    /// </summary>
    /// <seealso cref="AudioManager"/>
    public class VirtualAudioSource : RichMonoBehaviour
    {
        public RichAudioClip clip;
        public bool playOnEnable = false;
        public bool stopOnDisable = false;

        private AudioID audioID;

        private void OnEnable()
        {
            if (playOnEnable)
                PlaySFX(clip.AudioClip, clip.Options);
        }

        private void OnDisable()
        {
            if (stopOnDisable)
                StopSFX();
        }

        public AudioID PlayBackgroundTrack()
            => audioID = AudioManager.PlayBackgroundTrack(clip);

        public AudioID PlayBackgroundTrack(AudioClip clip)
            => audioID = AudioManager.PlayBackgroundTrack(clip);

        public AudioID PlayBackgroundTrack(AudioClip clip,
            AudioOptions options)
            => audioID = AudioManager.PlayBackgroundTrack(clip, options);

        public void PlaySFX()
            => audioID = AudioManager.PlaySFX(clip.AudioClip, clip.Options);

        public void PlaySFX(AudioClip clip)
            => audioID = AudioManager.PlaySFX(clip);

        public void PlaySFX(RichAudioClip clipRef)
            => audioID = AudioManager.PlaySFX(
                clipRef.AudioClip, clipRef.Options);

        /// <summary>
        /// Play the given clip. If 'duration' LT 0, then it will be length of clip.
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="options"></param>
        public AudioID PlaySFX(AudioClip clip, AudioOptions options)
            => audioID = AudioManager.PlaySFX(clip, options);

        public void PlaySFXAndDestroy()
        {
            PlaySFX();
            stopOnDisable = false;
            Destroy(this);
        }

        public void StopSFX()
            => AudioManager.StopSFX(audioID, fadeOutDuration: 0);

    }
}
