using ScriptableObjectArchitecture;

/// <summary>
/// 
/// </summary>
public static class AudioClipVariable_Extensions
{
    /// <summary>
    /// Play the AudioClip with the attached AudioOptions.
    /// </summary>
    public static AudioID PlayBGM(this AudioClipVariable r)
        => AudioManager.PlayBackgroundTrack(r.Value, r.Options);

    /// <summary>
    /// Play the AudioClip with the given AudioOptions.
    /// </summary>
    public static AudioID PlayBGM(this AudioClipVariable r,
        AudioOptions options)
        => AudioManager.PlayBackgroundTrack(r.Value, options);
        
    /// <summary>
    /// Play the AudioClip with the attached AudioOptions.
    /// </summary>
    public static AudioID PlaySFX(this AudioClipVariable clip)
        => AudioManager.PlaySFX(clip.Value, clip.Options);

    /// <summary>
    /// Play the AudioClip with the given AudioOptions.
    /// </summary>
    public static AudioID PlaySFX(this AudioClipVariable clip,
        AudioOptions options)
        => AudioManager.PlaySFX(clip.Value, options);
}
