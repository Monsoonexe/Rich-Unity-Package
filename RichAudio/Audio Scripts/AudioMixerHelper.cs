using UnityEngine;
using UnityEngine.Audio;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
    /// <summary>
    /// Exposes AudioMixer properties to FloatVariables.
    /// </summary>
    /// <seealso cref="AudioManager"/>
    public class AudioMixerHelper : RichMonoBehaviour
    {
        public enum InitSettings
        {
            LoadVariablesIntoMixer,
            LoadMixerIntoVariables
        }

        [Header("---Settings---")]
        [Tooltip("Init Variables with values in Mixer.")]
        [EnumToggleButtons]
        public InitSettings initSettings = InitSettings.LoadMixerIntoVariables;

        [Header("---Volume Resources---")]
        [SerializeField, Required]
        private FloatVariable masterVolume = null;

        [Required, SerializeField]
        private FloatVariable musicVolume = null;

        [SerializeField, Required]
        private FloatVariable sfxVolume = null;

        [SerializeField, Required]
        private FloatVariable voiceVolume = null;

        #region Property Names

        [FoldoutGroup("---Property Names---")]
        [Tooltip("Must match property name on AudioMixer exactly.")]
        [SerializeField]
        private string masterVolumeProperty = "MasterVolume";

        [FoldoutGroup("---Property Names---")]
        [Tooltip("Must match property name on AudioMixer exactly.")]
        [SerializeField]
        private string musicVolumeProperty = "MusicVolume";

        [FoldoutGroup("---Property Names---")]
        [Tooltip("Must match property name on AudioMixer exactly.")]
        [SerializeField]
        private string sfxVolumeProperty = "SFXVolume";

        [FoldoutGroup("---Property Names---")]
        [Tooltip("Must match property name on AudioMixer exactly.")]
        [SerializeField]
        private string voiceVolumeProperty = "VoiceVolume";

        #endregion

        [Header("---Resources---")]
        [SerializeField, Required]
        private AudioMixer mainMixer = null;

        protected override void Awake()
        {
            base.Awake();

            //subscribe to events
            masterVolume.AddListener(UpdateMasterVolume);
            musicVolume.AddListener(UpdateMusicVolume);
            sfxVolume.AddListener(UpdateSFXVolume);
            voiceVolume.AddListener(UpdateVoiceVolume);
        }

        private void Start()
        {
            switch (initSettings)
            {
                case InitSettings.LoadMixerIntoVariables:
                    LoadMixerValuesIntoVariables();
                    break;
                case InitSettings.LoadVariablesIntoMixer:
                    mainMixer.SetFloat(masterVolumeProperty, masterVolume);
                    mainMixer.SetFloat(musicVolumeProperty, musicVolume);
                    mainMixer.SetFloat(sfxVolumeProperty, sfxVolume);
                    mainMixer.SetFloat(voiceVolumeProperty, voiceVolume);
                    break;
                default:
                    Debug.LogError("[AudioMixerHelper] Unreachable code detected!!!!", this);
                    break;
            }
        }

        private void OnDestroy()
        {
            //subscribe to events
            masterVolume.RemoveListener(UpdateMasterVolume);
            musicVolume.RemoveListener(UpdateMusicVolume);
            sfxVolume.RemoveListener(UpdateSFXVolume);
            voiceVolume.RemoveListener(UpdateVoiceVolume);
        }

        [Button]
        private void LoadMixerValuesIntoVariables()
        {
            LoadValueFromMixer(masterVolumeProperty, masterVolume);
            LoadValueFromMixer(musicVolumeProperty, musicVolume);
            LoadValueFromMixer(sfxVolumeProperty, sfxVolume);
            LoadValueFromMixer(voiceVolumeProperty, voiceVolume);
        }

        /// <summary>
        /// Read value from Mixer and load it into backing Variable.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="variable"></param>
        private void LoadValueFromMixer(string propertyName,
            FloatVariable variable)
        {
            bool propertyValid;
            float propertyValue;

            propertyValid = mainMixer.GetFloat(propertyName,
                out propertyValue);

            //validate
            Debug.AssertFormat(propertyValid, "[AudioMixerHelper] audioMixer {0}" +
                " doesn't have an exposed property {1}",
                mainMixer, propertyName);

            variable.Value = propertyValue;
        }

        //[Button]
        //private void ValidateMixer()
        //{
        //    //TODO make sure that AudioMixer has all the appropriate settings
        //}

        #region Event Handlers

        private void UpdateMasterVolume(float vol)
            => mainMixer.SetFloat(masterVolumeProperty, masterVolume);

        private void UpdateMusicVolume(float vol)
            => mainMixer.SetFloat(musicVolumeProperty, musicVolume);

        private void UpdateSFXVolume(float vol)
            => mainMixer.SetFloat(sfxVolumeProperty, sfxVolume);

        private void UpdateVoiceVolume(float vol)
            => mainMixer.SetFloat(voiceVolumeProperty, voiceVolume);

        #endregion  
    }
}
