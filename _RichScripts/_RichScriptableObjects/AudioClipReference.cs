using UnityEngine;

namespace Explore.Audio
{
    [CreateAssetMenu(fileName = "_AudioClipReference",
        menuName = "ScriptableObjects/References/Audio Clip")]
    public class AudioClipReference : ScriptableReference<AudioClip>
    {
        [Header("---Clip Options---")]
        [SerializeField]
        private AudioOptions defaultOptions = AudioOptions.Default;
        public AudioOptions Options { get => defaultOptions; }

        /// <summary>
        /// Play the AudioClip with the attached AudioOptions.
        /// </summary>
        public AudioID PlaySFX()
            => AudioManager.PlaySFX(value, defaultOptions);
    }

}