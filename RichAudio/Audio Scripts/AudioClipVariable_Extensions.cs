using ScriptableObjectArchitecture;

/// <summary>
/// 
/// </summary>
public static class AudioClipVariable_Extensions
{
	/// <summary>
	/// Play the AudioClip with the attached AudioOptions. Returns 'void', 
	/// so good to use as Action.
	/// </summary>
	/// <param name="v"></param>
	public static void DoPlayBGM(this AudioClipVariable v)
		=> AudioManager.PlayBackgroundTrack(v.Value, v.Options);

	/// <summary>
	/// Play the AudioClip with the attached AudioOptions.
	/// </summary>
	public static AudioID PlayBGM(this AudioClipVariable v)
        => AudioManager.PlayBackgroundTrack(v.Value, v.Options);

    /// <summary>
    /// Play the AudioClip with the given AudioOptions.
    /// </summary>
    public static AudioID PlayBGM(this AudioClipVariable v,
        AudioOptions options)
        => AudioManager.PlayBackgroundTrack(v.Value, options);

	/// <summary>
	/// Play the AudioClip with the attached AudioOptions. Returns 'void', 
	/// so good to use as Action.
	/// </summary>
	/// <param name="v"></param>
	public static void DoPlaySFX(this AudioClipVariable v)
		=> AudioManager.PlaySFX(v.Value, v.Options);

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
