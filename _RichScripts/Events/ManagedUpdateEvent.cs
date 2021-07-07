
namespace RichPackage.Managed
{
    /// <summary>
    /// Raises UnityEvent on ManagedUpdate(). Rig it in Inspector.
    /// </summary>
    /// <seealso cref="UpdateUnityEvent"/>
    public sealed class ManagedUpdateEvent : AManagedEvent,
        IManagedUpdate
    {
        //override because specific is better than vague for this system.
        private void OnEnable()
        {   //cast because specific is faster
            ManagedBehaviourEngine.AddManagedListener((IManagedUpdate)this);
        }

        //override because specific is better than vague for this system.
        private void OnDisable()
        {   //cast because specific is faster
            ManagedBehaviourEngine.RemoveManagedListener((IManagedUpdate)this);
        }

        public void ManagedUpdate()
        {
            lifetimeEvent.Invoke();
        }
    }

}
