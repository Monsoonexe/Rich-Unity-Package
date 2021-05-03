using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ScriptableObjectArchitecture;

/// <summary>
/// Controls a meter fill amount between min and max
/// </summary>
public class UIMeterInt : RichUIElement
{
    [Header("---Prefab Refs---")]
    [SerializeField]
    private Image myImage;

    /// <summary>
    /// Change sprite used to fill image.
    /// </summary>
    public Sprite FillSprite { get => myImage.sprite; set => myImage.sprite = value; }

    /// <summary>
    /// Change tint of image (Image.color).
    /// </summary>
    public Color FillTint { get => myImage.color; set => myImage.color = value; }

    [Header("---Resources---")]
    [SerializeField]
    private IntVariable sourceValue;

    private void OnEnable()
    {
        SubscribeToEvents();
        UpdateUI();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void Start()
    {
        UpdateUI();
    }

    public void SubscribeToEvents()
    {
        sourceValue.AddListener(UpdateUI);
    }

    public void UnsubscribeFromEvents()
    {
        sourceValue.RemoveListener(UpdateUI);
    }

    /// <summary>
    /// Refresh UI element with current data values.
    /// </summary>
    public override void UpdateUI()
    {
        var min = sourceValue.MinClampValue; //cache
        float range = sourceValue.MaxClampValue - min;
        myImage.fillAmount = (sourceValue  - min) / range;
    }

    public override void ToggleVisuals(bool active)
    {
        myImage.enabled = active;
    }

    public void OnValidate()
    {
        if (sourceValue != null && !sourceValue.Clampable)
            Debug.LogError("[UIMeterInt] sourceValue is not clampable! " + sourceValue.name, this);
    }
}
