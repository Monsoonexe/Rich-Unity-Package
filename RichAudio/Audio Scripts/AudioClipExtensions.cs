using System.Runtime.CompilerServices;
using UnityEngine;

namespace RichPackage.Audio
{
    /// <summary>
    /// Handy class to do fancy things like `myAudioClip.PlayOneShot();`
    /// </summary>
    public static class AudioClip_Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioID PlayMusic(this AudioClip clip,
            AudioOptions options = default)
            => AudioManager.Instance.PlayMusic(clip, options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlayOneShot(this AudioClip clip, bool loop = false,
                bool pitchShift = true, float crossfade = 0.0f, int priority = 128,
                float volume = 1.0f, float duration = 0.0f)
            => AudioManager.Instance.PlaySFX(clip, loop, pitchShift, crossfade, priority, volume, duration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlayOneShot(this AudioClip clip, AudioOptions options)
            => AudioManager.Instance.PlaySFX(clip, options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopSfx(this AudioID id, float fadeOutDuration = 0.0f)
            => AudioManager.Instance.StopSFX(id, fadeOutDuration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioID RestartSFX(this AudioID id)
            => AudioManager.Instance.RestartSfx(id);
    }
}
