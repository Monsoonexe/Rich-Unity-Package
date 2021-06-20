using System;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "FloatVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "float",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 3)]
    public class FloatVariable : BaseVariable<float>
    {
        private const int TEN = 10; //base ten
        public enum EMantissaBehaviour
        {
            /// <summary>
            /// Default mantissa behaviour.
            /// </summary>
            Default = 0,

            /// <summary>
            /// Ceiling at the nth decimal place.
            /// </summary>
            Ceiling,

            /// <summary>
            /// Floor at the nth decimal place.
            /// </summary>
            Floor,

            /// <summary>
            /// Round to nth decimal place.
            /// </summary>
            Round,
        }

        public EMantissaBehaviour mantissaBehaviour 
            = EMantissaBehaviour.Default;

        [Range(0, 8)]
        public int decimalDigits = 2;

        public override bool Clampable { get { return true; } }
        protected override float ClampValue(float value)
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
        public bool IsAtMaxValue { get => IsClamped && Value == MaxClampValue; }
        public bool IsAtMinValue { get => IsClamped && Value == MinClampValue; }

        protected override float SetValue(float value)
        {
            if (_readOnly)
            {
                RaiseReadonlyWarning();
                return _value;
            }
            else if (Clampable && IsClamped)
            {
                value = ClampValue(value);
            }

            switch (mantissaBehaviour)
            {
                case EMantissaBehaviour.Default:
                    break;
                case EMantissaBehaviour.Round:
                    value = (float)Math.Round(
                        value, decimalDigits, MidpointRounding.AwayFromZero);
                    break;
                case EMantissaBehaviour.Ceiling:
                    value = CeilingAtNthDecimal(value, decimalDigits);
                    break;
                case EMantissaBehaviour.Floor:
                    value = FloorAtNthDecimal(value, decimalDigits);
                    break;
            }

            return value;
        }

        public void Add(float x) => Value += x;
        public void Subtract(float x) => Value -= x;
        public void Multiply(float x) => Value *= x;
        public void Divide(float x) => Value /= x;
        public void Halve() => Value /= 2;
        public void Double() => Value *= 2;
        public void Negate() => Value *= -1;

        private float GetPowerOfTen(int power)
        {
            var factor = 1.0f;//start at 1 for multiply

            //exponentiate to move desired portion into integer section
            for (var i = 0; i < decimalDigits; ++i)
                factor *= TEN;

            return factor;
        }

        private float CeilingAtNthDecimal(float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return Mathf.Ceil(a);

            var factor = GetPowerOfTen(decimalDigits);

            a *= factor;//move decimal place right
            a = Mathf.Ceil(a);//ceil
            a /= factor;//move decimal place left

            return a;
        }

        private float FloorAtNthDecimal(float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return Mathf.Floor(a);

            var factor = GetPowerOfTen(decimalDigits);

            a *= factor;//move decimal place right
            a = Mathf.Floor(a);//ceil
            a /= factor;//move decimal place left

            return a;
        }
    }
}
