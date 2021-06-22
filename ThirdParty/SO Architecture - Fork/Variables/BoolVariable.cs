using System;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "BoolVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "bool",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 5)]
    public sealed class BoolVariable : BaseVariable<bool>
    {
        public override bool IsInitializeable { get => !_readOnly; }
        private readonly List<Action<bool>> _invertedActions 
            = new List<Action<bool>>();

        public void AddInvertedListener(Action<bool> action)
        {
            if (!_invertedActions.Contains(action))
                _invertedActions.Add(action);
        }
        public void RemoveAllInvertedListeners()
        {
            _invertedActions.Clear();
        }
        public void RemoveInvertedListener(Action<bool> action)
        {
            _invertedActions.Remove(action);
        }
        public override void Raise()
        {
            base.Raise();
            RaiseInverted(!_value);
        }
        public void RaiseInverted(bool invertedValue)
        {
            for (var i = _invertedActions.Count - 1; i >= 0; --i)
                _invertedActions[i](invertedValue);
        }

        public bool InvertedValue { get => !Value; }
        public void InvertValue() => Value = !Value;
    } 
}
