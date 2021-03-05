using System;
using UnityEngine;

/// <summary>
/// Options for dynamically playing sounds
/// </summary>
/// <seealso cref="AudioManager"/>
[Serializable]
public struct AudioOptions
{
    /// <summary>
    /// Repeat when complete.
    /// </summary>
    [Tooltip("Repeat when complete.")]
    public bool loop;

    /// <summary>
    /// Randomize pitch slightly for variation.
    /// </summary>
    [Tooltip("Randomize pitch slightly for variation.")]
    public bool pitchShift;

    /// <summary>
    /// How much time should this clip should be played for.
    /// Value LT 0 means "full length of clip".
    /// </summary>
    [Tooltip("How much time should this clip should be played for. \nValue LT 0 means 'full length of clip'.")]
    public float duration;

    /// <summary>
    /// Slowly fade out old track to replace with this one. Useful for background music.
    /// </summary>
    [Tooltip("Slowly fade out old track to replace with this one. Useful for background music.")]
    public float crossfade;

    /// <summary>
    /// 0.0 - 1.0
    /// </summary>
    [Tooltip("[0.0 - 1.0")]
    [Range(0, 1.0f)]
    public float volume;

    /// <summary>
    /// 1 (high) - 256 (low).
    /// </summary>
    [Tooltip("1 (hi) - 256 (lo).")]
    [Range(1, 256)]
    public int priority;

    #region Constructors

    //nope//no guarantee this gets called. Don't rely on it.
    //public AudioOptions()
    //{
    //    loop = false;
    //    pitchShift = true;
    //    volume = 1;
    //    priority = 128;
    //}

    public AudioOptions(bool loop = false, bool pitchShift = true, 
        float volume = 1.0f, int priority = 128, 
        float duration = 1.0f, float crossfade = 0.0f)
    {
        this.loop = loop;
        this.pitchShift = pitchShift;
        this.priority = priority;
        this.volume = volume;
        this.duration = duration;
        this.crossfade = crossfade;
    }

    #endregion

    public static AudioOptions Default { get => new AudioOptions(
        loop: false, pitchShift: true, priority: 128, duration: -1, 
        crossfade: 0); }
}