using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using ScriptableObjectArchitecture;

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
        public AudioClipReference audioClipRef;
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

    public bool IsInitialized { get; private set; }

    //runtime data
    protected Dictionary<string, TableEntry> audioClipTable;

    protected override void Awake()
    {
        base.Awake();
        IsInitialized = false;

        if (initOnAwake)
            Inititialize();
    }

    [Button("Inititialize", EButtonEnableMode.Playmode)]
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
            "[GlobalAudioRefs] AudioManager not initialized. Not in scene?");
        Debug.Assert(IsInitialized,
            "[AudioHub] Being used without being Init'd: " + this.name, this);

        if (audioClipTable.TryGetValue(clipTag, out TableEntry entry))
            entry.audioClipRef.PlaySFX();//actually do the thing
        else
            Debug.LogWarningFormat("[GlobalAudioRefs] Requested clip '{0}' not found on {1}.",
                clipTag, name);
    }

    public static AudioHub Construct()
    {
        //set name
        string newName = null;

#if UNITY_EDITOR
        newName = "AudioHub";
#endif
        return new GameObject(newName).AddComponent<AudioHub>();
    }
}

