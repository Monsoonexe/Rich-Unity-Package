using RichPackage.GuardClauses;
using UnityEngine.Events;

public static class UnityEventExtensions
{
    /// <summary>
    /// Subscribes <paramref name="action"/> as a listener to <paramref name="uEvent"/>
    /// that is automatically removed after is is invoked.
    /// </summary>
    public static void AddListenerOneShot(this UnityEvent uEvent, UnityAction action)
    {
        // validate
        GuardAgainst.ArgumentIsNull(uEvent, nameof(uEvent));
        GuardAgainst.ArgumentIsNull(action, nameof(action));

        UnityAction listener = null; // for closure
        listener = Remove;
        uEvent.AddListener(listener);

        void Remove()
        {
            action();
            uEvent.RemoveListener(listener);
        }
    }
}
