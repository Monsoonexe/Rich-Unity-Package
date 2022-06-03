
namespace RichPackage.Managed
{
    /// <summary>
    /// Raises UnityEvent on ManagedUpdate(). Rig it in Inspector.
    /// </summary>
    /// <seealso cref="UpdateUnityEvent"/>
    public sealed class ManagedUpdateEvent : AManagedEvent,
        IManagedUpdate
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(ManagedUpdate)}.");
        }

        //override because specific is better than vague for this system.
        private void OnEnable()
        {   //cast because specific is faster
            ManagedBehaviourEngine.AddManagedListener(this);
        }

        //override because specific is better than vague for this system.
        private void OnDisable()
        {   //cast because specific is faster
            ManagedBehaviourEngine.RemoveManagedListener(this);
        }

        public void ManagedUpdate()
        {
            lifetimeEvent.Invoke();
        }
    }
}
