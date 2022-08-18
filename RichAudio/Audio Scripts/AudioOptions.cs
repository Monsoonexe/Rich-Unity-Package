using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
    /// <summary>
    /// Options for dynamically playing sounds
    /// </summary>
    /// <seealso cref="AudioManager"/>
    [Serializable]
    public sealed class AudioOptions
    {
        #region Constants

        public const byte Priority_UI = 96;
        public const byte Priority_BGM = 32;
        public const byte Priority_Default = 128;
        public const float DefaultBgmCrossFade = 2.5f;
        public const float DefaultVolume = 1.0f;
        public const int UseClipDuration = -1;

        #endregion Constants

        #region Properties

        /// <summary>
        /// Repeat when complete.
        /// </summary>
        [PropertyTooltip("Repeat when complete.")]
        [field: SerializeField, LabelText(nameof(Loop))]
        public bool Loop { get; private set; } = false;

        /// <summary>
        /// Randomize pitch slightly for variation.
        /// </summary>
        [PropertyTooltip("Randomize pitch slightly for variation.")]
        [field: SerializeField, LabelText(nameof(PitchShift))]
        public bool PitchShift { get; private set; } = false;

        /// <summary>
        /// [hi, lo] : [0, 255]
        /// </summary>
        [PropertyTooltip("[hi, lo] : [0, 255]")]
        [PropertyRange(0, 255)]
        [field: SerializeField, LabelText(nameof(Priority))]
        public int Priority { get; private set; } = Priority_Default; // should be byte

        /// <summary>
        /// How much time should this clip should be played for.
        /// Value LTE 0 means "play full length of clip".
        /// </summary>
        [PropertyTooltip("How much time should this clip should be played for. \nValue LT 0 means 'full length of clip'.")]
        [field: SerializeField, LabelText(nameof(Duration))]
        public float Duration { get; private set; } = UseClipDuration;

        /// <summary>
        /// Slowly fade out old track to replace with this one. Useful for background music.
        /// </summary>
        [PropertyTooltip("Slowly fade out old track to replace with this one. Useful for background music.")]
        [field: SerializeField, LabelText(nameof(CrossFade))]
        public float CrossFade { get; private set; } = 0.0f;

        /// <summary>
        /// [0.0, 1.0]
        /// </summary>
        [PropertyTooltip("[0.0, 1.0]")]
        [PropertyRange(0, 1.0f)]
        [field: SerializeField, LabelText(nameof(Volume))]
        public float Volume { get; private set; } = 1.0f;

        #endregion Properties

        #region Defaults Options

        /// <summary>
        /// Default options suitable for a one-shot sound effect.
        /// </summary>
        public static readonly AudioOptions DefaultSfx
            = new AudioOptions(
                loop: false,
                pitchShift: true,
                priority: Priority_Default,
                duration: UseClipDuration,
                crossfade: 0.0f);

        /// <summary>
        /// Default options suitable for persitant background music.
        /// </summary>
        public static readonly AudioOptions DefaultBGM
            = new AudioOptions(
                loop: true,
                pitchShift: false,
                priority: Priority_BGM,
                crossfade: DefaultBgmCrossFade);

        #endregion Default Options

        #region Constructors

        public AudioOptions(bool loop = false, bool pitchShift = true,
            int priority = Priority_Default, float volume = DefaultVolume,
            float duration = UseClipDuration, float crossfade = 0.0f)
        {
            this.Loop = loop;
            this.PitchShift = pitchShift;
            this.Priority = priority;
            this.Volume = volume;
            this.Duration = duration;
            this.CrossFade = crossfade;
        }

        /// <summary>
        /// Copy-constructor.
        /// </summary>
        public AudioOptions(in AudioOptions source)
		{
            this.Loop = source.Loop;
            this.PitchShift = source.PitchShift;
            this.Priority = source.Priority;
            this.Volume = source.Volume;
            this.Duration = source.Duration;
            this.CrossFade = source.CrossFade;
		}

        public static AudioOptions From(in AudioOptions source)
            => new AudioOptions(source);

        #endregion Constructors

        public AudioOptions Clone(in AudioOptions source)
            => new AudioOptions(source);
    }
}
