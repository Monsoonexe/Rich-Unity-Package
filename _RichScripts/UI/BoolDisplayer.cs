using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

[SelectionBase]
public class BoolDisplayer : RichUIElement<BoolVariable>
{
    [Header("---Settings---")]
    public string trueMessage = "On";
    public string falseMessage = "Off";

    [Header("---Prefab Refs---")]
    [SerializeField]
    [Required]
    private TextMeshProUGUI textGUI;

    private void Reset()
    {
        SetDevDescription("I show a string based on a boolean value.");
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
        //determine which message to print.
        var outString = string.Empty;
        if (targetData.Value == true)
            outString = trueMessage;
        else
            outString = falseMessage;
        textGUI.text = outString;
    }
}
