﻿using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "BoolVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "bool",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 5)]
    public sealed class BoolVariable : BaseVariable<bool>
    {
        public bool InvertedValue { get => !Value; }

        public void InvertValue() => Value = !Value;
    } 
}
