using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[CreateAssetMenu(
	    fileName = "AudioClipVariable.asset",
	    menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "AudioClip",
	    order = 120)]
	public class AudioClipVariable : BaseVariable<AudioClip>
	{
        [Header("---Clip Options---")]
        [Tooltip("Note, changes to Options does not Raise events.")]
        [SerializeField]
        private AudioOptions defaultOptions = AudioOptions.Default;

        /// <summary>
        /// AudioClip options that indicate how this clip should be played.
        /// </summary>
        public AudioOptions Options
        {
            get => defaultOptions;
            set
            {
                if (_readOnly)
                    RaiseReadonlyWarning();
                else
                    defaultOptions = value;
            }
        }
    }
}
