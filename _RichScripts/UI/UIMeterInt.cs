using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
//using NaughtyAttributes;
using Sirenix.OdinInspector;

/// <summary>
/// Controls a meter fill amount between min and max
/// </summary>
[SelectionBase]
public class UIMeterInt : VariableUIElement<IntVariable>
{
    [Header("---Prefab Refs---")]
    [SerializeField]
    [Required]
    private Image myImage;

    /// <summary>
    /// Change sprite used to fill image.
    /// </summary>
    public Sprite FillSprite { get => myImage.sprite; set => myImage.sprite = value; }

    /// <summary>
    /// Change tint of image (Image.color).
    /// </summary>
    public Color FillTint { get => myImage.color; set => myImage.color = value; }

    public void OnValidate()
    {
        if (targetData != null && !targetData.Clampable)
            Debug.LogError("[UIMeterInt] sourceValue is not clampable! " + targetData.name, this);
    }

    /// <summary>
    /// Refresh UI element with current data values.
    /// </summary>
    [Button]
    public override void UpdateUI()
    {
        var min = targetData.MinClampValue; //cache
        float range = targetData.MaxClampValue - min;
        myImage.fillAmount = (targetData - min) / range;
    }

    public override void ToggleVisuals(bool active)
        => myImage.enabled = active;

    public override void ToggleVisuals()
        => myImage.enabled = !myImage.enabled;
}
