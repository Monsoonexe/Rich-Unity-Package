using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

/// <summary>
/// Displays an IntVariable's value with a TMP.
/// </summary>
/// <seealso cref="VariableText"/>
/// <seealso cref="FormattableIntUIElement"/>
public class VariableIntText : RichUIElement<IntVariable>
{
    [Header("---Scene Refs---")]
    [SerializeField]
    [Required]
    [Tooltip("Update this text element with target data.")]
    protected TextMeshProUGUI textElement;
    
    private void Reset()
    {
        textElement = GetComponent<TextMeshProUGUI>();
    }

    protected override void SubscribeToEvents()
    {
        targetData.AddListener(UpdateUI);
    }

    protected override void UnsubscribeFromEvents()
    {
        targetData.RemoveListener(UpdateUI);
    }

    [Button]
    public override void UpdateUI()
    {
        textElement.text = ConstStrings.GetCachedString(targetData);
    }

}
