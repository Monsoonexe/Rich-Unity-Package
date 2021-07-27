using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

public class VariableText : RichUIElement<BaseVariable>
{
    [Header("---Scene Refs---")]
    [SerializeField]
    [Required]
    protected TextMeshProUGUI tmp;
    
    private void Reset()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    protected override void SubscribeToEvents()
    {
        targetData.AddListener(UpdateUI);
    }

    protected override void UnsubscribeFromEvents()
    {
        targetData.RemoveListener(UpdateUI);
    }

    /// <summary>
    /// Update UI elements with current values.
    /// </summary>
    public override void UpdateUI()
    {
        var outputString = string.Empty;

        if(targetData is IntVariable intData)
        {
            outputString = ConstStrings.GetCachedString(intData);
        }
        else
        {
            outputString = targetData.ToString();
        }

        tmp.text = outputString; 
    }
}
