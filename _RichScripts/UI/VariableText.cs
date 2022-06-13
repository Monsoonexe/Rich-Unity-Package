using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

namespace RichPackage.UI
{
    /// <summary>
    /// Updates a Text element based on value of a Variable.
    /// </summary>
    [SelectionBase]
    public class VariableText : VariableUIElement<BaseVariable>
    {
        [Title("Scene Refs")]
        [SerializeField, Required]
        protected TextMeshProUGUI tmp;
        
        protected override void Reset()
        {
            base.Reset();
            tmp = GetComponent<TextMeshProUGUI>(); //make a guess for convenience
        }

        /// <summary>
        /// Update UI elements with current values.
        /// </summary>
        [Button]
        public override void UpdateUI()
            => tmp.text = targetData.ToString();
    }
}
