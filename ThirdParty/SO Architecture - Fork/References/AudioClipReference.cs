using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[System.Serializable]
	public sealed class AudioClipReference : BaseReference<AudioClip, AudioClipVariable>
	{
	    public AudioClipReference() : base() { }
	    public AudioClipReference(AudioClip value) : base(value) { }

        /// <summary>
        /// AudioClip options that indicate how this clip should be played.
        /// </summary>
        public AudioOptions Options
        {
            get => (_useConstant || _variable == null) 
                ? AudioOptions.DefaultSFX : _variable.Options;

            set
            {
                if (!_useConstant && _variable != null)
                {
                    _variable.Options = value;
                }
                else
                {
                    _useConstant = true;
                    //myOptions = value; can't set default
                }
            }
        }
	}
}