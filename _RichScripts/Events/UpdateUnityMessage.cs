
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on Update(). Rig it in Inspector.
    /// </summary>
    public sealed class UpdateUnityMessage : AUnityLifetimeMessage
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(Update)}.");
        }

        private void Update()
        {
            lifetimeEvent.Invoke();
        }
    }
}
