using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    public sealed class AudioClipReference : BaseReference<AudioClip, AudioClipVariable>
    {
        private AudioOptions options = AudioOptions.DefaultSFX;

        public AudioClipReference() : base() { }
        public AudioClipReference(AudioClip value) : base(value) { }

        public AudioClipReference(AudioClip value, AudioOptions options)
            : base(value)
        {
            this.options = options;
        }

        /// <summary>
        /// AudioClip options that indicate how this clip should be played.
        /// </summary>
        public AudioOptions Options
        {
            get => (_useConstant || _variable == null) 
                ? options : _variable.Options;

            set
            {
                if (!_useConstant && _variable != null)
                {
                    _variable.Options = value;
                }
                else
                {
                    _useConstant = true;
                    options = value;
                }
            }
        }
    }
}
