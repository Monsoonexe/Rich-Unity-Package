using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using ScriptableObjectArchitecture;

namespace RichPackage.Audio
{
    /// <summary>
    /// Play an audio clip using only a string reference to its tag.
    /// </summary>
    /// <seealso cref="GlobalAudioHub"/>
    public class AudioHub : RichMonoBehaviour
    {
        #region Data Structures

        [Serializable]
        protected struct TableEntry
        {
            public string tag;
            public RichAudioClip audioClipRef;
        }

        #endregion

        [Header("---Settings---")]
        [Tooltip("Prefer true unless you really want this " +
            "kind of responsibility on yourself.")]
        [SerializeField] protected bool initOnAwake = true;
        [SerializeField] protected bool isPersistentThroughScenes = true;

        [Header("---Audio---")]
        [SerializeField]
        protected TableEntry[] clipEntries;

        //runtime data
        protected Dictionary<string, TableEntry> audioClipTable;
        public bool IsInitialized { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            IsInitialized = false;

            if (initOnAwake)
                Inititialize();
        }

        private void PlaySFX(AudioClip clip)
            => AudioManager.PlaySFX(clip);

        [Button(null, EButtonEnableMode.Playmode)]
        public void Inititialize()
        {
            var entries = clipEntries.Length;
            audioClipTable = new Dictionary<string, TableEntry>(entries);
            for (var i = 0; i < entries; ++i)
                audioClipTable.Add(clipEntries[i].tag, clipEntries[i]);
            IsInitialized = true;
        }

        public void PlayAudioClipSFX(string clipTag)
        {
            //validation
            Debug.Assert(AudioManager.IsInitialized,
                "[AudioHub] AudioManager not initialized. Not in scene?");
            Debug.Assert(IsInitialized,
                "[AudioHub] Being used without being Init'd: " + this.name, this);

            if (audioClipTable.TryGetValue(clipTag, out TableEntry entry))
                entry.audioClipRef.PlaySFX();//actually do the thing
            else
                Debug.LogWarning($"[{nameof(AudioHub)}] Requested clip '{clipTag}' not found on {name}.", this);
        }

        public void PlayAudioClipSFX(string clipTag, in AudioOptions options)
        {
            //validation
            Debug.Assert(AudioManager.IsInitialized,
                "[AudioHub] AudioManager not initialized. Not in scene?");
            Debug.Assert(IsInitialized,
                "[AudioHub] Being used without being Init'd: " + this.name, this);

            if (audioClipTable.TryGetValue(clipTag, out TableEntry entry))
                entry.audioClipRef.PlaySFX(options);//actually do the thing
            else
                Debug.LogWarning($"[{nameof(AudioHub)}] Requested clip '{clipTag}' not found on {name}.", this);
        }

        public static AudioHub Construct()
        {
            return new GameObject(nameof(AudioHub)).AddComponent<AudioHub>();
        }
    }
}
