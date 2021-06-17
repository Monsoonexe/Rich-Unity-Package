using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GlobalUIFrame : RichMonoBehaviour
{
    [SerializeField]
    protected UISettings uiSettings;

    public static UIFrame UIFrame { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        InitializeUI();
    }

    protected void InitializeUI()
    {
        UIFrame = uiSettings.CreateUIInstance();
    }
}
