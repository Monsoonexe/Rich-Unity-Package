using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(RichAudioClip),
        fileName = "AC_")]
    public class RichAudioClip : RichScriptableObject
    {
        [field: SerializeField, Required(InfoMessageType.Warning), LabelText(nameof(AudioClip))]
        public AudioClip AudioClip { get; private set; }

        /// <summary>
        /// AudioClip options that indicate how this clip should be played.
        /// </summary>
        [BoxGroup(nameof(Options))]
        [SerializeField, HideLabel, InlineProperty]
        [CustomContextMenu("SFX Options", nameof(ConfigureOptionsForSFX))]
        [CustomContextMenu("BGM Options", nameof(ConfigureOptionsForBGM))]
        private AudioOptions options = AudioOptions.DefaultSFX;

        public AudioOptions Options { get => options; private set => options = value; }

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
