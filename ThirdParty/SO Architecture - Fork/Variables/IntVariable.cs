﻿using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "IntVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "int",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 4)]
    public class IntVariable : BaseVariable<int>
    {
        public override bool Clampable { get { return true; } }
        protected override int ClampValue(int value)
        {
            if (value.CompareTo(MinClampValue) < 0)
            {
                return MinClampValue;
            }
            else if (value.CompareTo(MaxClampValue) > 0)
            {
                return MaxClampValue;
            }
            else
            {
                return value;
            }
        }
        public override bool IsInitializeable { get => !_readOnly; }
        public bool IsAtMaxValue { get => IsClamped && Value == MaxClampValue; }
        public bool IsAtMinValue { get => IsClamped && Value == MinClampValue; }

        /// <summary>
        /// Change the max clamp value. Does not raise events.
        /// </summary>
        public void SetMaxClampValue(int value)
		{
            if (ReadOnly)
			{
                RaiseReadonlyWarning();
                return;
			}
            _maxClampedValue = value;
        }

        /// <summary>
        /// Change the min clamp value. Does not raise events.
        /// </summary>
        public void SetMinClampValue(int value)
        {
            if (ReadOnly)
            {
                RaiseReadonlyWarning();
                return;
            }
            _minClampedValue = value;
        }

		#region Operators

		public static int operator +(IntVariable x, int y)
            => x.Value + y;

        public static int operator -(IntVariable x, int y)
            => x.Value - y;

		#endregion Operators

		public void Add(int x) => Value += x;
        public void Add(IntVariable x) => Value += x;
        public void Add(FloatVariable x) => Value += (int)x;

        public void Subtract(int x) => Value -= x;
        public void Subtract(IntVariable x) => Value -= x;
        public void Subtract(FloatVariable x) => Value -= (int)x;

        public void Multiply(int x) => Value *= x;
        public void Divide(int x) => Value /= x;
        public void Halve() => Value /= 2;
        public void Double() => Value *= 2;
        public void Negate() => Value *= -1;

        #region Static Constructors

        public static IntVariable Create(int value)
		{
            var newVariable = Create(value, int.MinValue, int.MaxValue);

            newVariable._isClamped = false;

            return newVariable;
		}

        public static IntVariable Create(int initial, int min, int max)
        {
            var newVariable = CreateInstance<IntVariable>();

            newVariable.SetMaxClampValue(max);
            newVariable.SetMinClampValue(min);
            newVariable.Value = initial;
            newVariable._isClamped = true;
            newVariable._readOnly = false;

            return newVariable;
        }

		#endregion Static Constructors
	}
}
