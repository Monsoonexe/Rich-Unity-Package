﻿//TODO - configure InputField input schema to fit data type.

using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using RichPackage.GuardClauses;

namespace RichPackage.UI
{
    [SelectionBase]
    public sealed class InputFieldToVariable : RichUIElement<BaseVariable>
    {
        // member Components
        [SerializeField, Required]
        private TMP_InputField inputField;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Synchronizes a ScriptableObject with input from InputField.");
            inputField = GetComponent<TMP_InputField>();
        }

        protected override void Awake()
        {
            base.Awake();

            // gather references
            if(inputField == null)
                inputField = GetComponent<TMP_InputField>();

            // validate
            GuardAgainst.ArgumentIsNull(inputField, nameof(inputField));
        }

        #endregion Unity Messages

        protected override void SubscribeToEvents()
        {
            inputField.onEndEdit.AddListener(UpdateData);
        }

        protected override void UnsubscribeFromEvents()
        {
            inputField.onEndEdit.RemoveListener(UpdateData);
        }

        /// <summary>
        /// Updates the input field with the value of the data.
        /// </summary>
        [Button]
        public override void UpdateUI()
        {
            inputField.text = targetData.ToString();
        }

        /// <summary>
        /// Updates the data with the value of the input field.
        /// </summary>
        private void UpdateData(string stringVal)
        {
            switch (targetData)
            {
                case StringVariable stringData:
                    stringData.Value = stringVal;
                    break;

                case BoolVariable boolData:
                    if (stringVal.TryToBool(out bool boolVal)) // if value parsed correctly
                        boolData.Value = boolVal;
                    else
                        Debug.LogWarning($"Could not parse bool value from <{stringVal}>.", this);
                    break;

                case IntVariable intData:
                    if (int.TryParse(stringVal, out int intVal)) //attempt to parse
                        intData.Value = intVal;
                    else
                        Debug.LogWarning($"Could not parse int value from <{stringVal}>.", this);
                    break;

                case FloatVariable floatData:
                    if (float.TryParse(stringVal, out float floatVal)) //attempt to parse
                        floatData.Value = floatVal;//should be valid because of inputfield constraints
                    else
                        Debug.LogWarning($"Could not parse float value from <{stringVal}>.", this);
                    break;

                default:
                    Debug.LogError($"[{nameof(InputFieldToVariable)}] Variable {targetData.Name} " +
                        $"is not a supported type (<{targetData.Type}>). " +
                        "Try bool, int, float, string; otherwise implement it.", this);
                    break;
            }
        }
    }
}
