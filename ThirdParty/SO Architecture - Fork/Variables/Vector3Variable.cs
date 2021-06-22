using UnityEngine;

namespace ScriptableObjectArchitecture
{
    [CreateAssetMenu(
        fileName = "Vector3Variable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "Structs/Vector3",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 11)]
    public sealed class Vector3Variable : BaseVariable<Vector3>
    {
        public override bool IsInitializeable { get => !_readOnly; }
        public void Add(Vector3 x) => Value += x;
        public void Subtract(Vector3 x) => Value -= x;
        public void Multiply(float x) => Value *= x;
        public void Divide(float x) => Value /= x;
    } 
}