using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldToScriptable : RichUIElement<BaseVariable>
{
    //member Components
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    protected override void SubscribeToEvents()
    {
        inputField.onEndEdit.AddListener(UpdateData);
    }

    protected override void UnsubscribeFromEvents()
    {
        inputField.onEndEdit.RemoveListener(UpdateData);
    }

    public override void UpdateUI()
    {
        inputField.text = targetData.ToString();
    }

    private void UpdateData(string stringVal)
    {
        if (targetData is StringVariable stringData) //string
        {
            stringData.Value = stringVal;
        }
        else if (targetData is BoolVariable boolData) //bool
        {
            if (stringVal.TryStringToBool(out bool boolVal))//if value parsed correctly
                boolData.Value = boolVal;
        }
        else if (targetData is IntVariable intData) //int
        {
            if (int.TryParse(stringVal, out int intVal)) //attempt to parse
                intData.Value = intVal;
        }
        else if (targetData is FloatVariable floatData) //float
        {
            if (float.TryParse(stringVal, out float floatVal)) //attempt to parse
                floatData.Value = floatVal;//should be valid because of inputfield constraints
        }
        else
        {
            Debug.LogWarningFormat("[InputFieldToScriptable] Scriptable {0} " +
                "does not have a supported type {1}. Try bool, int, float, string.",
                targetData.name, targetData.Type);
        }
    }
}
