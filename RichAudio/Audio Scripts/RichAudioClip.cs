using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
    public class RichAudioClip : RichScriptableObject
    {
        [field: SerializeField, Required(InfoMessageType.Warning), LabelText(nameof(AudioClip))]
        public AudioClip AudioClip { get; private set; }

        /// <summary>
        /// AudioClip options that indicate how this clip should be played.
        /// </summary>
        [PropertyTooltip("Note, changes to Options does not Raise events.")]
        [field: SerializeField, LabelText(nameof(Options))]
        [CustomContextMenu("SFX Options", "ConfigureOptionsForSFX")]
        [CustomContextMenu("BGM Options", "ConfigureOptionsForBGM")]
        public AudioOptions Options { get; private set; } = AudioOptions.DefaultSFX;

        private void Reset()
		{
            SetDevDescription("Audio with information on how to play it.");
		}

        public void ConfigureOptionsForBGM()
            => Options = AudioOptions.DefaultBGM;

        public void ConfigureOptionsForSFX()
            => Options = AudioOptions.DefaultSFX;

        public AudioID DoPlayBGM()
        {
            return AudioManager.PlayBackgroundTrack(AudioClip, Options);
        }

        public void DoPlaySFX()
        {
            AudioManager.PlaySFX(AudioClip, Options);
        }

        public void PlayBGM()
        {
            AudioManager.PlayBackgroundTrack(AudioClip, Options);
        }

        public AudioID PlaySFX()
        {
            return AudioManager.PlaySFX(AudioClip, Options);
        }

        public static implicit operator AudioClip (RichAudioClip a)
        {
            return a.AudioClip;
        }
        
        public static implicit operator AudioOptions (RichAudioClip a)
        {
            return a.Options;
        }
    }
}
