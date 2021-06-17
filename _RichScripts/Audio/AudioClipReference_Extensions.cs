using ScriptableObjectArchitecture;

public static class AudioClipReference_Extensions
{
    /// <summary>
    /// Play the AudioClip with the attached AudioOptions.
    /// </summary>
    public static AudioID PlaySFX(this AudioClipReference r)
        => AudioManager.PlaySFX(r.Value, r.Options);
}
