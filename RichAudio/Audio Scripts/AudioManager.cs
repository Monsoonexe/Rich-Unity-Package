using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

/* 
 * TODO - use less memory for WaitForSeconds in RemoveAfterPlay()
 * TODO - reduce 'static' interface surface
 * TODO - support StopBGM (track BGM AudioIDs)
 */

namespace RichPackage.Audio
{
    /// <summary>
    /// This class handles pooling AudioSources for static audio (audio not 
    /// affected by distance from Listener).
    /// </summary>
    public partial class AudioManager : RichMonoBehaviour, IAudioPlayer
    {
        #region Constants

        public const int MAX_PRIORITY = byte.MaxValue;
        public const int MIN_PRIORITY = byte.MinValue;

        #endregion Constants

        //singleton
        /// <summary>
        /// Backing field for <see cref="Instance"/>.
        /// </summary>
        private static AudioManager _instance;
        public static AudioManager Instance
        {
            get => _instance;
            private set => _instance = value;
        }

        private bool instanceIsInitialized = false;

        /// <summary>
        /// True if there is an active, initialized AudioManager in the Scene.
        /// If this is false and you aren't okay with that, call AudioManager.Init().
        /// </summary>
        public static bool IsInitialized { get => Instance != null && Instance.instanceIsInitialized; }

        [Title("---Settings---")]
        [SerializeField, Min(0)]
        private int sfxAudioSourceCount = 6;

        public Ease FadeEase = Ease.Linear;

        public Vector2 PitchShiftRange = new Vector2(0.8f, 1.2f);

        [Title("---Resources---")]
        [SerializeField]
        private AudioMixer audioMixer;

        private AudioSource[] sfxAudioSources;

        //[Header("---Background Music Sources---")]
        private AudioSource backgroundMusicTrackA;
        private AudioSource backgroundMusicTrackB;

        private AudioSource ActiveMusicTrack;
        private float cachedPausedMusicVolume;

        /// <summary>
        /// This is so the caller can query the sound after the fact, like for interruping a spell.
        /// </summary>
        private Dictionary<AudioID, AudioSource> sourceDictionary
            = new Dictionary<AudioID, AudioSource>(6);

        public bool IsBackgroundTrackPlaying { get => ActiveMusicTrack.isPlaying; }

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            if (InitSingleton(this, ref _instance, dontDestroyOnLoad: true))
            {
                UnityServiceLocator.Instance.RegisterAudioPlayer(this);
                CreateAudioSources();
                ActiveMusicTrack = backgroundMusicTrackB; // start with b to switch to a
                instanceIsInitialized = true;
            }
            else
            {
                Debug.LogWarning($"[{name}] Singleton: " +
                    $"Too many AudioManagers in the Scene! Destroying: {gameObject.name}.", this);
                Destroy(gameObject); // there can only be one!
            }
        }

        private void OnDestroy()
        {
            if (instanceIsInitialized)
            {
                instanceIsInitialized = false;
                StopAllCoroutines();
                foreach (var source in sfxAudioSources)
                    DOTween.Kill(source);
                sourceDictionary.Clear();
                UnityServiceLocator.Instance.DeregisterAudioPlayer(this);
            }
        }

        #endregion Unity Messages

        #region Initialize

        /// <summary>
        /// Create an AudioManager in the Scene.
        /// </summary>
#if UNITY_EDITOR
        [UnityEditor.MenuItem("RichUtilities/Audio/Create Instance in Scene")]
#endif
        public static AudioManager Init()
        {
            if (!Instance)
            {
                var prefab = Resources.Load<AudioManager>(nameof(AudioManager));
#if UNITY_EDITOR
                UnityEditor.PrefabUtility.InstantiatePrefab(prefab);
#else
                Instantiate(prefab);
#endif
            }
            return Instance;
        }

        /// <summary>
        /// Create all the AudioSources needed for the whole game.
        /// </summary>
        /// <remarks>Create all at once on same GameObject to help with cache.</remarks>
        private void CreateAudioSources()
        {
            //create SFX audio sources
            sfxAudioSources = new AudioSource[sfxAudioSourceCount];

            for (int i = 0; i < sfxAudioSourceCount; ++i)
            {
                sfxAudioSources[i] = gameObject.AddComponent<AudioSource>(); // new AudioSource
                sfxAudioSources[i].playOnAwake = false;
            }

            //create background track sources
            backgroundMusicTrackA = gameObject.AddComponent<AudioSource>();
            backgroundMusicTrackA.playOnAwake = false;
            backgroundMusicTrackB = gameObject.AddComponent<AudioSource>();
            backgroundMusicTrackB.playOnAwake = false;

            if (audioMixer)
            {   //this section of code makes a lot of assumptions about the given audio mixer
                var sfxGroup = audioMixer.FindMatchingGroups("SFX").First();
                for (int i = 0; i < sfxAudioSourceCount; ++i)
                {
                    sfxAudioSources[i].outputAudioMixerGroup = sfxGroup;
                }
                var bgGroup = audioMixer.FindMatchingGroups("Music").First();
                backgroundMusicTrackA.outputAudioMixerGroup = bgGroup;
                backgroundMusicTrackB.outputAudioMixerGroup = bgGroup;
            }
        }

        #endregion Initialize

        private static AudioSource DepoolAudioSource(AudioSource[] sources, int clipPriority)
        {
            // priority below this value are safe from overriding
            const int Will_Not_Override = 1;
            int size = sources.Length;
            int lowestPriority = clipPriority.Clamp(Will_Not_Override, byte.MaxValue);
            int lowestIndex = 0;

            for (int i = 0; i < size; ++i)
            {
                AudioSource source = sources[i]; // fetch once

                // prefer source that isn't busy
                if (!source.isPlaying)
                {
                    // use this one
                    lowestIndex = i;
                    break;
                }

                // try to override lowest priority (highest numeric value)
                else if (source.priority >= lowestPriority)
                {
                    lowestPriority = source.priority;
                    lowestIndex = i;
                }
            }

            return sources[lowestIndex];
        }

        /// <summary>
        /// Track AudioSource with a key so it can be found later.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private AudioID TrackAudioSource(AudioSource source)
        {
            //maybe use a List of custom structs
            var key = AudioID.GetNext();

            sourceDictionary.Add(key, source);

            return key;
        }

        /// <summary>
        /// Load options and play clip from source.
        /// </summary>
        public void ConfigAudioSourceAndPlay(AudioSource source,
            AudioClip clip, in AudioOptions options)
        {
            //load options
            source.loop = options.Loop;
            source.priority = options.Priority;
            if (options.CrossFade > 0)
            {
                source.volume = 0;
                source.DOFade(options.Volume, options.CrossFade)
                    .SetEase(FadeEase); // fade in
            }
            else
            {
                source.volume = options.Volume;
            }
            source.pitch = options.PitchShift
                ? PitchShiftRange.RandomRange()
                : 1.0f; // don't shift
            source.clip = clip;
            source.Play();
        }

        /// <summary>
        /// Free clip after time.
        /// </summary>
        private async UniTaskVoid StopSourceAfterTimeAsync(
            AudioClip clip, float clipLength,
            AudioID key, AudioSource source)
        {
            // wait until clip finishes playing
            await UniTask.Delay(TimeSpan.FromSeconds(clipLength),
                delayTiming: PlayerLoopTiming.EarlyUpdate, // so it is ready this frame
                cancellationToken: this.GetCancellationTokenOnDestroy());

            // clean up and ready
            sourceDictionary.Remove(key);
            if (source.clip == clip) // only stop if it wasn't cancelled and reused
                source.Stop();
        }

        #region Play Sound Effects

        /// <summary>
        /// Different way to play a SFX if you don't want to
        /// use <see cref="PlayOneShot(AudioClip, AudioOptions)"/>.
        /// </summary>
        /// <param name="duration">If <paramref name="duration"/> &lt; 0, then it will be length of clip.</param>
        public void PlayOneShot(AudioClip clip,
            bool loop = false,
            bool pitchShift = true,
            int priority = AudioOptions.Priority_Default,
            float volume = AudioOptions.DefaultVolume,
            float duration = AudioOptions.UseClipDuration)
        {
            // flag not an existing clip
            if (clip == null)
                return;

            var audioOptions = new AudioOptions(
                loop: loop,
                pitchShift: pitchShift,
                priority: priority,
                volume: volume,
                duration: duration);

            PlaySfxInternal(clip, audioOptions);
        }

        /// <summary>
        /// Play the given clip. If 'duration' LT 0, then it will be length of clip.
        /// </summary>
        public void PlayOneShot(AudioClip clip, AudioOptions options)
        {
            // flag not an existing clip
            if (clip == null)
                return;

            options = options ?? AudioOptions.DefaultSfx;

            PlaySfxInternal(clip, options);
        }

        private void PlaySfxInternal(AudioClip clip, AudioOptions options)
        {
            ConfigAudioSourceAndPlay(DepoolAudioSource(sfxAudioSources, options.Priority), clip, in options);
        }

        /// <summary>
        /// Restart the clip associated with <paramref name="key"/> if it is playing.
        /// </summary>
        public AudioID RestartSfx(AudioID key)
        {
            AudioID newID = AudioID.Invalid; // return value

            //TODO - take into account Coroutine
            //check key is valid and source is active
            if (key != AudioID.Invalid
                && sourceDictionary.TryGetValue(key,
                    out AudioSource source))
            {
                StopSFX(key);
                newID = TrackAudioSource(source);
            }

            return newID;
        }

        #endregion Play Sound Effects

        #region Stop Sound Effects

        /// <summary>
        /// Stop clip from playing and free up AudioSource.
        /// </summary>
        public void StopSFX(AudioID key, float fadeOutDuration = 0.0f, Ease fadeEase = Ease.Linear)
        {
            // check for invalid key
            if (key.Equals(AudioID.Invalid))
                return;

            // stop the clip if we were tracking it
            if (sourceDictionary.TryGetRemove(key, out AudioSource source))
            {
                if (fadeOutDuration > 0.01f)
                {
                    source.DOFade(0, fadeOutDuration)
                        .SetEase(fadeEase);
                }
                else
                {
                    source.Stop();
                }
            }
        }

        public void StopAllSFX()
        {
            for (int i = sfxAudioSources.Length - 1; i >= 0; --i)
                sfxAudioSources[i].Stop();
        }

        public void StopAllSFX(float fadeOutDuration)
        {
            for (int i = sfxAudioSources.Length - 1; i >= 0; --i)
            {
                sfxAudioSources[i].DOFade(0, fadeOutDuration)
                    .SetEase(FadeEase)
                    .OnComplete(sfxAudioSources[i].Stop);
            }
        }

        #endregion Stop Sound Effects

        #region Play Ambient Sounds

        public AudioID PlayAmbientSfx(AudioClip clip, AudioOptions options)
        {
            if (clip == null)
                return AudioID.Invalid;

            AudioSource source = DepoolAudioSource(sfxAudioSources, options.Priority);
            AudioID key = TrackAudioSource(source);
            ConfigAudioSourceAndPlay(source, clip, in options);

            if (options.Duration > 0 && !options.Loop)
                StopSourceAfterTimeAsync(clip, options.Duration, key, source).Forget();

            return key;
        }

        #endregion Play Ambient Sounds

        #region Play Background Music

        public void PauseBackgroundTrack()
        {
            ActiveMusicTrack.Pause();
        }

        public void PauseBackgroundTrack(float fadeDuration)
        {
            GuardClauses.GuardAgainst.IsZeroOrNegative(fadeDuration, nameof(fadeDuration));

            cachedPausedMusicVolume = ActiveMusicTrack.volume;
            ActiveMusicTrack.DOFade(0, fadeDuration)
                .SetEase(FadeEase)
                .OnComplete(ActiveMusicTrack.Pause);
        }

        /// <summary>
        /// Restart clip from the beginning.
        /// </summary>
        public void RestartMusic()
        {
            ActiveMusicTrack?.Restart();
        }

        public void ResumeMusic()
        {
            ActiveMusicTrack.Play();
            ActiveMusicTrack.volume = cachedPausedMusicVolume;
        }

        public void ResumeMusic(float fadeDuration)
        {
            // validate
            GuardClauses.GuardAgainst.IsZeroOrNegative(fadeDuration, nameof(fadeDuration));

            // fade
            ActiveMusicTrack.volume = 0;
            ActiveMusicTrack.Play();
            ActiveMusicTrack.DOFade(cachedPausedMusicVolume, fadeDuration)
                .SetEase(FadeEase);
        }

        public AudioID PlayMusic(AudioClip clip)
            => PlayMusic(clip, AudioOptions.DefaultBGM);

        public AudioID PlayMusic(AudioClip clip,
            in AudioOptions options)
        {
            if (!clip)
                return AudioID.Invalid;//for safety

            ActiveMusicTrack.DOFade(0, options.CrossFade)
                .SetEase(FadeEase); // fade out current track
            ActiveMusicTrack = ActiveMusicTrack == backgroundMusicTrackA ? // switch active track
                backgroundMusicTrackB : backgroundMusicTrackA; // just alternate tracks

            AudioID key = TrackAudioSource(ActiveMusicTrack);
            ConfigAudioSourceAndPlay(ActiveMusicTrack, clip, in options);
            return key;
        }

        public void StopAllMusic()
        {
            backgroundMusicTrackA.Stop();
            backgroundMusicTrackB.Stop();
        }

        public void StopAllMusic(float fadeDuration, Ease fadeEase = Ease.Linear)
        {
            // validate
            GuardClauses.GuardAgainst.IsZeroOrNegative(fadeDuration, nameof(fadeDuration));

            // animate
            if (backgroundMusicTrackA.isPlaying)
                backgroundMusicTrackA.DOFadeOut(fadeDuration, fadeEase);

            if (backgroundMusicTrackB.isPlaying)
                backgroundMusicTrackB.DOFadeOut(fadeDuration, fadeEase);
        }

        public void MuteBackgroundTrack(bool muted)
            => ActiveMusicTrack.mute = muted;

        public void ToggleMuteBackgroundTrack()
            => ActiveMusicTrack.mute = !ActiveMusicTrack.mute;

        #endregion Play Background Music
    }

}