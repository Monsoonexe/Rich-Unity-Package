using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.Audio
{
    /// <summary>
    /// Play an audio clip using only a string as the key.
    /// </summary>
    /// <seealso cref="GlobalAudioHub"/>
    public class AudioHub : RichMonoBehaviour
    {
        #region Data Structures

        [Serializable]
        protected struct TableEntry
        {
            [HorizontalGroup("entry"), LabelWidth(24)]
            public string tag;

            [HorizontalGroup("entry"), LabelWidth(100)]
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
        protected Dictionary<string, RichAudioClip> audioClipTable;// = new Dictionary<string, TableEntry>(8);

        [ShowInInspector, ReadOnly]
        public bool IsInitialized { get; private set; }

        #region UnityMessages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Play an audio clip using only a string as the key.");
		}

		protected override void Awake()
        {
            base.Awake();
            IsInitialized = false;

            if (initOnAwake)
                Inititialize();
        }

		#endregion UnityMessages

		private void PlaySFX(AudioClip clip)
            => AudioManager.PlaySFX(clip);

        [Button, DisableInEditorMode]
        public void Inititialize()
        {
            if (IsInitialized)
			{
                Debug.LogWarning($"{nameof(AudioHub)} already initialized.");
			}
			else
            {
                var entries = clipEntries.Length;
                audioClipTable = new Dictionary<string, RichAudioClip>(entries);
                for (var i = 0; i < entries; ++i)
                    audioClipTable.Add(clipEntries[i].tag, clipEntries[i].audioClipRef);
                clipEntries = null; //release memory
                IsInitialized = true;
            }
        }

        [Button, DisableInEditorMode]
        public void PlayAudioClipSFX(string clipTag)
        {
            //validation
            Debug.Assert(AudioManager.IsInitialized,
                "[AudioHub] AudioManager not initialized. Not in scene?");
            Debug.Assert(IsInitialized,
                "[AudioHub] Being used without being Init'd: " + this.name, this);

            if (audioClipTable.TryGetValue(clipTag, out RichAudioClip entry))
                entry.PlaySFX();//actually do the thing
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

            if (audioClipTable.TryGetValue(clipTag, out RichAudioClip entry))
                AudioManager.PlaySFX(entry, options);//actually do the thing
            else
                Debug.LogWarning($"[{nameof(AudioHub)}] Requested clip '{clipTag}' not found on {name}.", this);
        }

        public static AudioHub Construct()
        {
            return new GameObject(nameof(AudioHub)).AddComponent<AudioHub>();
        }
    }
}
