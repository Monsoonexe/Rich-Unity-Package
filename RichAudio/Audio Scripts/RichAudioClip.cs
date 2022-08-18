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
        private AudioOptions options = AudioOptions.DefaultSfx;

        public AudioOptions Options
        {
            get => options;
            private set => options = value;
        }

        private void Reset()
		{
            SetDevDescription("Audio with information on how to play it.");
		}

        public AudioID PlayBGM()
        {
            return AudioClip.PlayMusic(Options);
        }

        public void PlayOneShot()
        {
            AudioClip.PlayOneShot(Options);
        }

        #region Operators

        public static implicit operator AudioClip (RichAudioClip a)
        {
            return a.AudioClip;
        }
        
        public static implicit operator AudioOptions (RichAudioClip a)
        {
            return a.Options;
        }

        #endregion Operators
    }
}
