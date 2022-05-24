using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
    /// <summary>
    /// A Mono that will play audio from static AudioManager.
    /// Helpful for rigging events and prefabs to audio that aren't
    /// yet aware of an <see cref="AudioManager"/> in a scene.
    /// </summary>
    /// <seealso cref="AudioManager"/>
    public class VirtualAudioSource : RichMonoBehaviour
    {
        [Title("Options")]
        public bool playOnEnable = false;
        public bool stopOnDisable = false;

        [Title(nameof(clip))]
        [InlineProperty, HideLabel]
        public RichAudioClipReference clip
            = new RichAudioClipReference(AudioOptions.DefaultSFX);

        //runtime fields
        private AudioID audioID;

        #region Unity Messages

        private void Reset()
        {
            SetDevDescription(" A Mono that will play audio from static " +
				"AudioManager Helpful for rigging events and prefabs to audio" +
				" that aren't yet aware of an AudioManager in a scene..");
        }

        private void OnEnable()
        {
            if (playOnEnable)
                PlaySFX(clip.Clip, clip.Options);
        }

        private void OnDisable()
        {
            if (stopOnDisable)
                StopSFX();
        }

        #endregion Unity Messages

        [Button, HorizontalGroup("butts"), DisableInEditorMode]
        public void PlayBGM()
            => audioID = AudioManager.PlayBackgroundTrack(clip);

        public void PlayBGM(AudioClip clip)
            => audioID = AudioManager.PlayBackgroundTrack(clip);

        public void PlayBGM(AudioClip clip, in AudioOptions options)
            => audioID = AudioManager.PlayBackgroundTrack(clip, options);

        [Button, HorizontalGroup("butts"), DisableInEditorMode]
        public void PlaySFX()
            => audioID = AudioManager.PlaySFX(clip.Clip, clip.Options);

        public void PlaySFX(AudioClip clip)
            => audioID = AudioManager.PlaySFX(clip);

        public void PlaySFX(AudioClip clip, AudioOptions options)
            => audioID = AudioManager.PlaySFX(clip, options);

        public void PlaySFXAndDestroy()
        {
            stopOnDisable = false;
            PlaySFX();
            Destroy(this);
        }

        [Button, HorizontalGroup("butts_off"), DisableInEditorMode]
        public void StopBGM()
            => AudioManager.StopAllBackground();

        [Button, HorizontalGroup("butts_off"), DisableInEditorMode]
        public void StopSFX()
            => AudioManager.StopSFX(audioID, fadeOutDuration: 0);
    }
}
