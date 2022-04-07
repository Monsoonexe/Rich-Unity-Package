﻿using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

/// <summary>
/// Updates a Text element based on value of a Variable.
/// </summary>
[SelectionBase]
public sealed class VariableText : VariableUIElement<BaseVariable>
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
    public override void UpdateUI()
        => tmp.text = targetData.ToString();
}
