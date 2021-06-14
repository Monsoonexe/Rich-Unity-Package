using UnityEngine;

/// <summary>
/// A Mono that will play audio from static AudioManager.
/// Helpful for rigging events and prefabs to audio that aren't
/// yet aware of an AudioManager in a scene.
/// </summary>
/// <seealso cref="AudioManager"/>
public class VirtualAudioSource : RichMonoBehaviour
{
    public AudioClipVariable clip;
    public bool playOnAwake = false;
    public bool stopOnDestroy = false;

    private AudioID audioID;

    private void Start()
    {
        if (playOnAwake && clip)
            PlaySFX(clip.Value, clip.Options);
    }

    private void OnDestroy()
    {
        if (stopOnDestroy)
            StopSFX();
    }

    public AudioID PlayBackgroundTrack(AudioClip clip)
        => audioID = AudioManager.PlayBackgroundTrack(clip, default);

    public AudioID PlayBackgroundTrack(AudioClip clip,
        AudioOptions options)
        => audioID = AudioManager.PlayBackgroundTrack(clip, options);

    public void PlaySFX()
        => audioID = AudioManager.PlaySFX(clip.Value, clip.Options);

    public void PlaySFX(AudioClip clip)
        => audioID = AudioManager.PlaySFX(clip);

    public void PlaySFX(AudioClipVariable clipRef)
        => audioID = AudioManager.PlaySFX(
            clipRef.Value, clipRef.Options);

    /// <summary>
    /// Play the given clip. If 'duration' LT 0, then it will be length of clip.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="options"></param>
    public AudioID PlaySFX(AudioClip clip, AudioOptions options)
        => audioID = AudioManager.PlaySFX(clip, options);

    public void StopSFX()
        => AudioManager.StopSFX(audioID, 0);

}
