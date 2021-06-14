using UnityEngine;

public class GlobalAudioHub : AudioHub
{
    private static GlobalAudioHub instance;

    protected override void Awake()
    {
        if (!InitSingleton(this, ref instance, dontDestroyOnLoad: isPersistentThroughScenes))
        {
            Destroy(this);
            return;
        }

        base.Awake();
    }

    public static void PlayGlobalSFX(string clipTag)
    {
        Debug.Assert(instance != null,
            "[GlobalAudioHub] No instance in Scene.");
        instance.PlayAudioClipSFX(clipTag);
    }

    [UnityEditor.MenuItem("Tools/Audio Manager/GlobalAudioHub")]
    public static void ConstructGlobal()
    {
        if(instance != null)
        {
            Debug.Log("[GlobalAudioHub] GlobalAudioHub already Scene.", instance);
            return;
        }
        //set name
        string newName = null;

#if UNITY_EDITOR
        newName = "Global AudioHub";
#endif
        instance = Construct<GlobalAudioHub>(newName);
    }

}