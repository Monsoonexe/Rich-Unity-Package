using UnityEngine;

/// <summary>
/// Handy class to do fancy things like myClip.PlayBackgroundTrack();
/// </summary>
public static class AudioClip_Extensions
{
    public static AudioID PlayBackgroundTrack(this AudioClip clip,
        AudioOptions options = default)
        => AudioManager.PlayBackgroundTrack(clip, options);

    /// <summary>
    /// Different way to play a SFX if you don't want to use AudioOptions.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="loop"></param>
    /// <param name="pitchShift"></param>
    /// <param name="crossfade"></param>
    /// <param name="priority"></param>
    /// <param name="volume"></param>
    /// <param name="duration">If 'duration' LT 0, then it will be length of clip.</param>
    /// <returns></returns>
    public static AudioID PlaySFX(this AudioClip clip, bool loop = false,
            bool pitchShift = true, float crossfade = 0.0f, int priority = 128,
            float volume = 1.0f, float duration = 0.0f)
        => AudioManager.PlaySFX(clip, loop, pitchShift, crossfade, priority, volume, duration);

    /// <summary>
    /// Play the given clip. If 'duration' LT 0, then it will be length of clip.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="options"></param>
    public static AudioID PlaySFX(this AudioClip clip, AudioOptions options)
        => AudioManager.PlaySFX(clip, options);

    public static void StopSFX(this AudioID id, float fadeOutDuration = 0.0f)
        => AudioManager.StopSFX(id, fadeOutDuration);

    public static void RestartSFX(this AudioID id)
        => AudioManager.RestartSFX(id);
}
