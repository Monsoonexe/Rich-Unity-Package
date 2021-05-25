using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

public class VariableText : RichUIElement<BaseVariable>
{
    [Header("---Scene Refs---")]
    [SerializeField]
    private TextMeshProUGUI tmp;

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
        tmp.text = targetData.ToString();
    }
}
