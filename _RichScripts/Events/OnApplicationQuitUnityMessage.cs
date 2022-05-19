
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on Start(). Rig it in Inspector. 
    /// </summary>
    /// <remarks>Probably useless. Included for completeness.</remarks>
    public sealed class OnApplicationQuitUnityMessage : AUnityLifetimeMessage
    {
        private void OnApplicationQuit()
        {
            lifetimeEvent.Invoke();
        }
    }
}
