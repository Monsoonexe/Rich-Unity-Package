
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on Update(). Rig it in Inspector.
    /// </summary>
    public sealed class UpdateUnityMessage : AUnityLifetimeMessage
    {
        private void Update()
        {
            lifetimeEvent.Invoke();
        }
    }
}
