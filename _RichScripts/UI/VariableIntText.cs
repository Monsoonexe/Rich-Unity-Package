using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

/// <summary>
/// Displays an IntVariable's value with a TMP.
/// </summary>
/// <seealso cref="VariableTextUIController"/>
public class VariableIntText : RichUIElement<IntVariable>
{
    /*TODO 
     * Configuration: just show number
     * Config: cur / max
     * Config: min / cur / max
     */

    [Header("---Scene Refs---")]
    [SerializeField]
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

    public override void UpdateUI()
    {
        textElement.text = ConstStrings.GetCachedString(targetData);
    }

}
