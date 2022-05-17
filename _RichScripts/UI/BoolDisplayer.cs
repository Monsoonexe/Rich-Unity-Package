using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

namespace RichPackage.UI
{
    [SelectionBase]
    public class BoolDisplayer : VariableUIElement<BoolVariable>
    {
        [Title("Settings")]
        public string trueMessage = "On";
        public string falseMessage = "Off";

        [Title("Prefab Refs")]
        [SerializeField, Required]
        private TextMeshProUGUI textGUI;

        public override void UpdateUI()
        {
        //determine which message to print.
            string outString;
            if (targetData.Value == true)
                outString = trueMessage;
            else
                outString = falseMessage;
            textGUI.text = outString;
        }
    }
}
