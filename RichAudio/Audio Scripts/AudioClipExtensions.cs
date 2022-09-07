using System.Runtime.CompilerServices;
using RichPackage;
using RichPackage.Audio;

namespace UnityEngine
{
    /// <summary>
    /// Handy class to do fancy things like `myAudioClip.PlayOneShot();`
    /// </summary>
    public static class AudioClip_Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioID PlayMusic(this AudioClip clip,
            AudioOptions options = default)
            => UnityServiceLocator.AudioPlayer.PlayMusic(clip, options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlayOneShot(this AudioClip clip, bool loop = false,
                bool pitchShift = true, int priority = 128,
                float volume = 1.0f, float duration = 0.0f)
            => UnityServiceLocator.AudioPlayer.PlayOneShot(clip, loop, pitchShift, priority, volume, duration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PlayOneShot(this AudioClip clip, AudioOptions options)
            => UnityServiceLocator.AudioPlayer.PlayOneShot(clip, options);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopSfx(this AudioID id, float fadeOutDuration = 0.0f)
            => UnityServiceLocator.AudioPlayer.StopSFX(id, fadeOutDuration);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioID RestartSFX(this AudioID id)
            => UnityServiceLocator.AudioPlayer.RestartSfx(id);
    }
}
