using DG.Tweening;
using UnityEngine;

namespace RichPackage.Audio
{
    public interface IAudioPlayer
    {
        bool IsBackgroundTrackPlaying { get; }

        void MuteBackgroundTrack(bool muted);
        void PauseBackgroundTrack();
        void PauseBackgroundTrack(float fadeDuration);
        AudioID PlayAmbientSfx(AudioClip clip, AudioOptions options);
        AudioID PlayMusic(AudioClip clip);
        AudioID PlayMusic(AudioClip clip, in AudioOptions options);
        void PlayOneShot(AudioClip clip, bool loop = false, bool pitchShift = true, int priority = 128, float volume = 1, float duration = -1);
        void PlayOneShot(AudioClip clip, AudioOptions options);
        void RestartMusic();
        AudioID RestartSfx(AudioID key);
        void ResumeMusic();
        void ResumeMusic(float fadeDuration);
        void StopAllMusic();
        void StopAllMusic(float fadeDuration, Ease fadeEase = Ease.Linear);
        void StopAllSFX();
        void StopAllSFX(float fadeOutDuration);
        void StopSFX(AudioID key, float fadeOutDuration = 0, Ease fadeEase = Ease.Linear);
        void ToggleMuteBackgroundTrack();
    }
}