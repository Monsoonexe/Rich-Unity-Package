using UnityEngine;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 
/// </summary>
[ExecuteInEditMode]
[RequireComponent(typeof(AudioSource))]
public class EditorAudio : RichMonoBehaviour
{
    private static EditorAudio Instance;
    private AudioSource myAudioSource;

    public AudioClipReference testClip = new AudioClipReference();

    private void Reset()
    {
        SetDevDescription("I help play audio in the Editor!" +
            " Make sure I'm tagged 'EditorOnly'.");
        myAudioSource = GetComponent<AudioSource>();
        Instance = this;
    }

    protected override void Awake()
    {
        base.Awake();

#if !UNITY_EDITOR 
        //should be tagged EditorOnly so this doesn't happen.
        Destroy(this.gameObject);
        return;
#endif
        Instance = this;
        myAudioSource = GetComponent<AudioSource>();
    }

    public static void PlayClip(AudioClip clip)
    {
        if(!Instance)
        {
            CreateInstance();
        }
        Instance.myAudioSource.PlayOneShot(clip);
    }
    
#if UNITY_EDITOR
[MenuItem("Tools/Audio/EditorAudio")]
#endif
    public static void CreateInstance()
    {
        Instance = new GameObject(typeof(EditorAudio).Name)
            .AddComponent<EditorAudio>();
        Instance.tag = "EditorOnly";
        Instance.myAudioSource = Instance.gameObject.GetComponent<AudioSource>();
    }

    [Button]
    private void Test()
    {
        PlayClip(testClip);
    }
}
