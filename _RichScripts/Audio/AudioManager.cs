using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class AudioManager : RichMonoBehaviour
{
    //consts
    private static readonly string AUDIO_MANAGER = "AudioManager";

    //singleton
    private static AudioManager Instance;

    /// <summary>
    /// True if there is an active, initialized AudioManager in the Scene.
    /// If this is false and you aren't okay with that, call AudioManager.Init().
    /// </summary>
    public static bool IsInitialized { get; private set; }

    [Header("---Audio Manager---")]
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField]
    private int sfxAudioSourceCount = 6;

    private AudioSource[] SFXAudioSources;
    private static AudioSource[] SFXSources { get => Instance.SFXAudioSources; }

    [SerializeField] private Vector2 pitchShiftRange = new Vector2(0.8f, 1.2f);
    private static Vector2 PitchShiftRange { get => Instance.pitchShiftRange; }

    //[Header("---Background Music Sources---")]
    private AudioSource backgroundMusicTrackA;
    private static AudioSource BackgroundMusicTrackA
    { get => Instance.backgroundMusicTrackA; }

    private AudioSource backgroundMusicTrackB;
    private static AudioSource BackGroundMusicTrackB
    { get => Instance.backgroundMusicTrackB; }

    private static AudioSource ActiveMusicTrack;

    /// <summary>
    /// This is so the caller can query the sound after the fact, like for interruping a spell.
    /// </summary>
    private static Dictionary<uint, AudioSource> sourceDictionary
        = new Dictionary<uint, AudioSource>(6);

    protected override void Awake()
    {
        base.Awake();
        InitSingleton(this);
        CreateAudioSources();
        ActiveMusicTrack = backgroundMusicTrackB; // start with b to switch to a
    }

    private void OnDestroy()
    {
        if (Instance == this)
            IsInitialized = false;//no more active audio manager in scene.
    }

    private static void InitSingleton(AudioManager current)
    {
        if (!Instance)
        {
            Instance = current;
            current.transform.SetParent(null);//DontDestroyOnLoad only works when transform is at root of Scene
            DontDestroyOnLoad(Instance.gameObject); // immortality!
            IsInitialized = true;
        }
        else
        {
            Debug.LogWarning("[AudioManager] Singleton: Too many AudioManagers in the Scene! Destroying: " +
                current.name, current);
            Destroy(current.gameObject); // there can only be one!
        }
    }

    /// <summary>
    /// Create all the AudioSources needed for the whole game.
    /// </summary>
    /// <remarks>Create all at once on same GameObject to help with cache.</remarks>
    private void CreateAudioSources()
    {
        //create SFX audio sources
        SFXAudioSources = new AudioSource[sfxAudioSourceCount];

        for (var i = 0; i < sfxAudioSourceCount; ++i)
        {
            SFXAudioSources[i] = gameObject.AddComponent<AudioSource>(); // new AudioSource
            SFXAudioSources[i].playOnAwake = false;
        }

        //create background track sources
        backgroundMusicTrackA = gameObject.AddComponent<AudioSource>();
        backgroundMusicTrackA.playOnAwake = false;
        backgroundMusicTrackB = gameObject.AddComponent<AudioSource>();
        backgroundMusicTrackB.playOnAwake = false;

        if (audioMixer)
        {   //this section of code makes a lot of assumptions about the given audio mixer
            var sfxGroup = audioMixer.FindMatchingGroups("SFX")[0];
            for (var i = 0; i < sfxAudioSourceCount; ++i)
            {
                SFXAudioSources[i].outputAudioMixerGroup = sfxGroup;
            }
            var bgGroup = audioMixer.FindMatchingGroups("Background Music")[0];
            backgroundMusicTrackA.outputAudioMixerGroup = bgGroup;
            backgroundMusicTrackB.outputAudioMixerGroup = bgGroup;
        }
    }

    /// <summary>
    /// Track AudioSource with a key so it can be found later.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private static AudioID TrackAudio(AudioSource source)
    {
        //maybe use a List of custom structs
        var key = AudioID.GetNextKey();

        sourceDictionary.Add(key, source);

        return key;
    }

    private static float RandomPitchShift()
        => Random.Range(PitchShiftRange.x, PitchShiftRange.y);

    /// <summary>
    /// Load options and play clip from source.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="clip"></param>
    /// <param name="options"></param>
    private static void ConfigAudioSource(AudioSource source,
        AudioClip clip, in AudioOptions options)
    {
        //load options
        source.loop = options.loop;
        source.priority = options.priority;
        if (options.crossfade > 0)
            source.DOFade(options.volume, options.crossfade); // fade in
        else
            source.volume = options.volume;
        source.pitch = options.pitchShift ? RandomPitchShift() : 1.0f;
        source.clip = clip;
        source.Play();
    }

    /// <summary>
    /// Free clip after time.
    /// </summary>
    /// <param name="clipLength"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static IEnumerator RemoveSourceAfterClip(float clipLength, AudioID key)
    {
        yield return new WaitForSeconds(clipLength); // wait until duration over

        if (sourceDictionary.TryGetValue(key.ID, out AudioSource ass))
        {
            sourceDictionary.Remove(key.ID);
            if (ass.loop == true)//only need to stop if looping.
                ass.Stop();
        }
    }

    #region API

    /// <summary>
    /// Create an AudioManager in the Scene.
    /// </summary>
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Tools/Audio Manager/Create Instance in Scene")]
#endif
    public static void Init()
    {
        if (!Instance)
        {
            var prefab = Resources.Load<AudioManager>(AUDIO_MANAGER);
#if UNITY_EDITOR
            UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
#else
            Instantiate(prefab);
            //new GameObject(AUDIO_MANAGER).AddComponent<AudioManager>();
#endif
        }
    }

    public void PlaySFX(AudioClip clip)
        => AudioManager.PlaySFX(clip);

    public void PlaySFX(AudioClipReference clipRef)
        => AudioManager.PlaySFX(
            clipRef.Value, clipRef.Options);

    public void PlaySFX(AudioClipVariable clipVar)
        => AudioManager.PlaySFX(
            clipVar.Value, clipVar.Options);

    /// <summary>
    /// Different way to play a SFX if you don't want to use AudioOptions.
    /// </summary>
    /// <param name="duration">If 'duration' LT 0, then it will be length of clip.</param>
    /// <returns></returns>
    public static AudioID PlaySFX(AudioClip clip, bool loop = false,
        bool pitchShift = true, float crossfade = 0.0f, int priority = 128,
        float volume = 1.0f, float duration = 0.0f)
    {
        if (clip == null) return AudioID.Invalid;//for safety
        var audioOptions = new AudioOptions(
            loop: loop, pitchShift: pitchShift, crossfade: crossfade,
            priority: priority, volume: volume,
            duration: duration //validate
            );

        return PlaySFX(clip, audioOptions);
    }

    /// <summary>
    /// Play the given clip. If 'duration' LT 0, then it will be length of clip.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="options"></param>
    public static AudioID PlaySFX(AudioClip clip, AudioOptions options)
    {
        if (clip == null) return AudioID.Invalid;//for safety

        Debug.Assert(Instance != null, "[AudioManager] Not initialized. " +
            "Please call AudioManager.Init() or instantiate prefab at root.");

        //default values for options, iff none included.
        if (options.priority <= 0) //clear sign this wasn't init'd
            options = AudioOptions.DefaultSFX;

        //find an audio source with the lowest volume, or that is not playing
        var size = SFXSources.Length;
        var sources = SFXSources;
        var lowestVolume = 1.0f;// note not used at all
        var lowestIndex = 0;
        AudioSource source = null;

        for (var i = 0; i < size; ++i)
        {
            source = sources[i];
            if (!source.isPlaying) // if this one isn't busy
            {
                // use this one
                lowestIndex = i;
                break;
            }

            //try to override quietest volume
            else if (source.volume < lowestVolume)
            {
                lowestVolume = source.volume;
                lowestIndex = i;
            }
        }

        source = sources[lowestIndex];
        //set duration to clip length if <= 0
        var _duration = options.duration <= 0 ? clip.length : options.duration;

        var key = TrackAudio(source); // return a key so audio can be interrupted later
        ConfigAudioSource(source, clip, in options);

        // if looping with duration < 0, it's up to caller to stop the clip.
        if (!options.loop || _duration > 0) //stop clip if not looping, or if looping for a specified duration
            Instance.StartCoroutine(RemoveSourceAfterClip(_duration, key)); // free source after time
        return key;
    }

    public static AudioID PlayBackgroundTrack(AudioClip clip,
        AudioOptions options = default)
    {
        if (!clip) return AudioID.Invalid;//for safety

        Debug.Assert(Instance != null, "[AudioManager] Not initialized. " +
            "Please call AudioManager.Init() or instantiate prefab at root.");

        //default values for options, iff none included.
        if (options.priority <= 0) //clear sign this wasn't init'd
            options = AudioOptions.DefaultBGM;

        //
        ActiveMusicTrack.DOFade(0, options.crossfade); // fade out current track
        ActiveMusicTrack = ActiveMusicTrack == BackgroundMusicTrackA ? // switch active track
            BackGroundMusicTrackB : BackgroundMusicTrackA; // just alternate tracks

        var key = TrackAudio(ActiveMusicTrack);
        if (!options.loop) // if it's looping, it's up to caller to stop the clip.
            Instance.StartCoroutine(RemoveSourceAfterClip(clip.length, key)); // free source after time
        ConfigAudioSource(ActiveMusicTrack, clip, in options);
        return key;
    }

    /// <summary>
    /// Stop clip from playing and free up AudioSource.
    /// </summary>
    /// <param name="key"></param>
    public static void StopSFX(AudioID key, float fadeOutDuration = 0.0f)
    {
        var id = key.ID;
        if (id < 0) return;//check for invalid key

        var found = sourceDictionary.TryGetValue(id, out AudioSource source);

        if (found && source)
        {
            if (fadeOutDuration > 0.01f)
            {
                source.DOFade(0, fadeOutDuration);
            }
            else
            {
                source.Stop();
            }
            sourceDictionary.Remove(id);
        }
    }

    public static void StopAllSFX()
    {
        var sources = SFXSources;
        for (var i = sources.Length - 1; i >= 0; --i)
            sources[i].Stop();
    }

    public static void StopAllSFXFade(float fadeOutDuration)
    {
        var sources = SFXSources;
        for (var i = sources.Length - 1; i >= 0; --i)
        {
            sources[i].DOFade(0, fadeOutDuration)
                .OnComplete(sources[i].Stop);
        }
    }

    public static void StopAllBackground()
    {
        BackgroundMusicTrackA.Stop();
        BackGroundMusicTrackB.Stop();
    }

    public static void StopAllBackgroundFade(float fadeDuration)
    {
        BackgroundMusicTrackA.DOFade(0, fadeDuration)
            .OnComplete(BackgroundMusicTrackA.Stop);
        BackGroundMusicTrackB.DOFade(0, fadeDuration)
            .OnComplete(BackGroundMusicTrackB.Stop);
    }

    #endregion
}
