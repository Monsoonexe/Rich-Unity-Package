using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "FloatVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "float",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 3)]
    public class FloatVariable : BaseVariable<float>
    {
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

        public void Add(float x) => Value += x;
        public void Subtract(float x) => Value -= x;
        public void Multiply(float x) => Value *= x;
        public void Divide(float x) => Value /= x;
        public void Halve() => Value /= 2;
        public void Double() => Value *= 2;
        public void Negate() => Value *= -1;
    } 
}