
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on Start(). Rig it in Inspector. 
    /// </summary>
    /// <remarks>Probably useless. Included for completeness.</remarks>
    public sealed class OnApplicationQuitUnityMessage : AUnityLifetimeMessage
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(OnApplicationQuit)}.");
        }

        private void OnApplicationQuit()
        {
            lifetimeEvent.Invoke();
        }
    }
}
