using RichPackage;
using RichPackage.Events;
using System;

namespace UnityEngine
{
    /// <summary>
    /// Raises events when the behavior is en/disabled.
    /// </summary>
    public class EnableEventMessages : RichMonoBehaviour
    {
        private EventHandlerList onDisabledList;
        private EventHandlerList onEnabledList;
        private readonly EventHandlerList<bool> eventList = new EventHandlerList<bool>();
        private EventHandlerList<EnableEventMessages, bool> eventList1;

        #region Unity Messages

        private void OnDestroy()
        {
            onDisabledList?.RemoveAll();
            onEnabledList?.RemoveAll();
            eventList.RemoveAll();
            eventList1?.RemoveAll();

            onDisabledList = null;
            onEnabledList = null;
            eventList1 = null;
        }

        private void OnEnable()
        {
            onEnabledList?.Invoke();
            eventList.Invoke(true);
            eventList1?.Invoke(this, true);
        }

        private void OnDisable()
        {
            onDisabledList?.Invoke();
            eventList.Invoke(false);
            eventList1?.Invoke(this, false);
        }

        #endregion Unity Messages

        public void AddListener(Action<bool> action) => eventList.Add(action);
        public void RemoveListener(Action<bool> action) => eventList.Remove(action);

        // TODO - add more support
    }
}
