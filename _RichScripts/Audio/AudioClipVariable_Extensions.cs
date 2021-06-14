using ScriptableObjectArchitecture;

/// <summary>
/// 
/// </summary>
public static class AudioClipVariable_Extensions
{
    /// <summary>
    /// Play the AudioClip with the attached AudioOptions.
    /// </summary>
    public static AudioID PlaySFX(this AudioClipVariable r)
        => AudioManager.PlaySFX(r.Value, r.Options);
}
