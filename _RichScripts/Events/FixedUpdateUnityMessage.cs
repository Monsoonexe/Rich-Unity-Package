
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on FixedUpdate(). Rig it in Inspector.
    /// </summary>
    public sealed class FixedUpdateUnityMessage : AUnityLifetimeMessage
    {
        private void FixedUpdate()
        {
            lifetimeEvent.Invoke();
        }
    }
}
