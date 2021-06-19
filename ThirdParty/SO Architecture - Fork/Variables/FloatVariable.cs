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
            /// Raise to nearest integer greater than f.
            /// </summary>
            CeilingToInt,

            /// <summary>
            /// Lower to nearest integer smaller than f.
            /// </summary>
            FloorToInt,

            /// <summary>
            /// Round to nearest integer.
            /// </summary>
            RoundToInt,

            /// <summary>
            /// Ceiling at the nth decimal place.
            /// </summary>
            CeilingAtDecimal,

            /// <summary>
            /// Floor at the nth decimal place.
            /// </summary>
            FloorAtDecimal,

            /// <summary>
            /// Round to nth decimal place.
            /// </summary>
            RoundToDecimal,

            /// <summary>
            /// Cut mantissa off after x places.
            /// </summary>
            Truncate
        }

        public EMantissaBehaviour mantissaBehaviour 
            = EMantissaBehaviour.Default;

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

        public override float SetValue(float value)
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
                case EMantissaBehaviour.Truncate:
                    value = TruncateMantissa(value, decimalDigits);
                    break;
                case EMantissaBehaviour.RoundToDecimal:
                    value = RoundToDecimal(value, decimalDigits);
                    break;
                case EMantissaBehaviour.RoundToInt:
                    value = Mathf.Round(value);
                    break;
                case EMantissaBehaviour.CeilingToInt:
                    value = Mathf.Ceil(value);
                    break;
                case EMantissaBehaviour.FloorToInt:
                    value = Mathf.Floor(value);
                    break;
                case EMantissaBehaviour.CeilingAtDecimal:
                    value = CeilingAtNthDecimal(value, decimalDigits);
                    break;
                case EMantissaBehaviour.FloorAtDecimal:
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

        /// <summary>
        /// 10.37435 (1) = 10.3
        /// </summary>
        /// <param name="a"></param>
        /// <param name="decimalDigits"></param>
        /// <returns></returns>
        private float TruncateMantissa(float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return (int)a;

            var truncator = GetPowerOfTen(decimalDigits);

            //move decimal left, truncate mantissa, move decimal back right
            return a = ((int)(a * truncator)) / truncator;
        }

        /// <summary>
        /// 10.37565 (2) = 10.38
        /// </summary>
        /// <param name="a"></param>
        /// <param name="decimalDigits"></param>
        /// <returns></returns>
        private float RoundToDecimal(float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return Mathf.Round(a);

            var truncator = GetPowerOfTen(decimalDigits);

            a = Mathf.Round(a * truncator);//round off decimals
            
            //move decimal point back to origin
            return a / truncator;
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
