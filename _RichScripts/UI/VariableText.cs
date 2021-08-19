using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

/// <summary>
/// Updates a Text element based on value of a Variable.
/// </summary>
[SelectionBase]
public class VariableText : VariableUIElement<BaseVariable>
{
    [Header("---Scene Refs---")]
    [SerializeField]
    [Required]
    protected TextMeshProUGUI tmp;
    
    private void Reset()
    {
        SetDevDescription("Updates a Text element based on value of a Variable.");
        tmp = GetComponent<TextMeshProUGUI>();
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
