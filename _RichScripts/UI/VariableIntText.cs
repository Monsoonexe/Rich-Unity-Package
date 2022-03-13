using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

/// <summary>
/// Displays an IntVariable's value with a TMP.
/// </summary>
/// <seealso cref="VariableText"/>
/// <seealso cref="FormattableIntUIElement"/>
[SelectionBase]
public class VariableIntText : VariableUIElement<IntVariable>
{
    [Header("---Scene Refs---")]
    [SerializeField, Required]
    [Tooltip("Update this text element with target data.")]
    protected TextMeshProUGUI textElement;
    
    private void Reset()
    {
        SetDevDescription("Displays an IntVariable's value with a TMP.");
        textElement = GetComponent<TextMeshProUGUI>();
    }

    [Button]
    public override void UpdateUI()
        => textElement.text = targetData.Value.ToStringCached();
}
