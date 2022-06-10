using UnityEngine;
using RichPackage;
using Sirenix.OdinInspector;
using QFSW.QC;

/// <summary>
/// 
/// </summary>
public class GlobalUIFrame : RichMonoBehaviour
{
    [SerializeField]
    protected UISettings uiSettings;

    [ShowInInspector, ReadOnly]
    public static UIFrame UIFrame { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        InitializeUI();
    }

    protected void InitializeUI()
    {
        UIFrame = uiSettings.CreateUIInstance();
        QuantumRegistry.RegisterObject(UIFrame);
    }
}
