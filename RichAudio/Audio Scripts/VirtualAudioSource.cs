﻿using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Audio
{
    /// <summary>
    /// A Mono that will play audio from static AudioManager.
    /// Helpful for rigging events and prefabs to audio that aren't
    /// yet aware of an <see cref="AudioManager"/> in a scene.
    /// </summary>
    /// <seealso cref="AudioManager"/>
    /// <seealso cref="AudioSource"/>
    public class VirtualAudioSource : RichMonoBehaviour
    {
        [Title("Options")]
        public bool playOnEnable = false;

        [InlineProperty, HideLabel, Title(nameof(clip))]
        public RichAudioClipReference clip
            = new RichAudioClipReference(AudioOptions.DefaultSfx);

        // runtime fields
        private AudioID audioID;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I will play audio using the " + nameof(AudioManager) + "system." + 
				" Helpful for rigging events and prefabs to audio" +
				" that aren't yet aware of an AudioManager in a scene..");
        }

        private void OnEnable()
        {
            if (playOnEnable)
                PlayOneShot(clip.Clip, clip.Options);
        }

        #endregion Unity Messages

        [Button, HorizontalGroup("butts"), DisableInEditorMode]
        public void PlayMusic()
            => audioID = UnityServiceLocator.AudioPlayer.PlayMusic(clip);

        public void PlayMusic(AudioClip clip)
            => audioID = UnityServiceLocator.AudioPlayer.PlayMusic(clip);

        public void PlayBGM(AudioClip clip, in AudioOptions options)
            => audioID = UnityServiceLocator.AudioPlayer.PlayMusic(clip, options);

        [Button, HorizontalGroup("butts"), DisableInEditorMode]
        public void PlayOneShot()
            => UnityServiceLocator.AudioPlayer.PlayOneShot(clip.Clip, clip.Options);

        public void PlayOneShot(AudioClip clip)
            => UnityServiceLocator.AudioPlayer.PlayOneShot(clip);

        public void PlayOneShot(AudioClip clip, AudioOptions options)
            => UnityServiceLocator.AudioPlayer.PlayOneShot(clip, options);

        public void PlaySFXAndDestroy()
        {
            PlayOneShot();
            Destroy(this);
            clip = null;
        }

        [Button, HorizontalGroup("butts_off"), DisableInEditorMode]
        public void StopBGM()
            => UnityServiceLocator.AudioPlayer.StopAllMusic();
    }
}
