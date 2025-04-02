using UnityEngine;
using ScriptableObjectArchitecture;

namespace RichPackage.UI
{
    /// <summary>
    /// Class that shows data based on a BaseVariable.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="BaseVariable"/> to observe.</typeparam>
    [SelectionBase]
    public abstract class VariableUIElement<T> : RichUIElement<T>
        where T : BaseVariable
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Displays a {typeof(T)} and updates when its events are called.");
        }

        protected override void SubscribeToEvents()
            => targetData.AddListener(UpdateUI);

        protected override void UnsubscribeFromEvents()
            => targetData.RemoveListener(UpdateUI);
    }
}
