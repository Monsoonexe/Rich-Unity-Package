using System;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	/// <seealso cref="BoolVariable"/>
    [AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "bool Event Listener")]
    public sealed class BoolGameEventListener : BaseGameEventListener<bool, BoolGameEvent, BoolUnityEvent>
    {
        [SerializeField]
        private BoolUnityEvent invertedResponse = new BoolUnityEvent();

		protected override void RaiseResponse(bool value)
		{
			base.RaiseResponse(value);
			invertedResponse.Invoke(!value);
		}

        public void AddInvertedListener(Action<bool> action)
        {
            invertedResponse.AddListener(action.Invoke);
        }

        public void RemoveInvertedListener(Action<bool> action)
        {
            invertedResponse.RemoveListener(action.Invoke);
        }
    }
}
