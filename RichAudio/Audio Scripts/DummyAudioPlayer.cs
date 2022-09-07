// consider logging in here

using DG.Tweening;
using UnityEngine;

namespace RichPackage.Audio
{
    /// <summary>
    /// Null-object pattern.
    /// </summary>
    internal class DummyAudioPlayer : IAudioPlayer
    {
        public bool IsBackgroundTrackPlaying => true;

        public void MuteBackgroundTrack(bool muted)
        {
             
        }

        public void PauseBackgroundTrack()
        {
             
        }

        public void PauseBackgroundTrack(float fadeDuration)
        {
             
        }

        public AudioID PlayAmbientSfx(AudioClip clip, AudioOptions options)
        {
            return AudioID.Invalid;
        }

        public AudioID PlayMusic(AudioClip clip)
        {
            return AudioID.Invalid;

        }

        public AudioID PlayMusic(AudioClip clip, in AudioOptions options)
        {
            return AudioID.Invalid;

        }

        public void PlayOneShot(AudioClip clip, bool loop = false, bool pitchShift = true, int priority = 128, float volume = 1, float duration = -1)
        {
             
        }

        public void PlayOneShot(AudioClip clip, AudioOptions options)
        {
             
        }

        public void RestartMusic()
        {
             
        }

        public AudioID RestartSfx(AudioID key)
        {
            return AudioID.Invalid;

        }

        public void ResumeMusic()
        {
             
        }

        public void ResumeMusic(float fadeDuration)
        {
             
        }

        public void StopAllMusic()
        {
             
        }

        public void StopAllMusic(float fadeDuration, Ease fadeEase = Ease.Linear)
        {
             
        }

        public void StopAllSFX()
        {
             
        }

        public void StopAllSFX(float fadeOutDuration)
        {
             
        }

        public void StopSFX(AudioID key, float fadeOutDuration = 0, Ease fadeEase = Ease.Linear)
        {
             
        }

        public void ToggleMuteBackgroundTrack()
        {
             
        }
    }
}
