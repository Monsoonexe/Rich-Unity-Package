using CommonServiceLocator;
using RichPackage.Audio;
using RichPackage.GuardClauses;

namespace RichPackage
{
    public sealed partial class UnityServiceLocator : ServiceLocatorImplBase
    {
        public static IAudioPlayer AudioPlayer { get; private set; } = new DummyAudioPlayer();

        public void RegisterAudioPlayer(IAudioPlayer service)
        {
            GuardAgainst.ArgumentIsNull(service, nameof(service));
            RegisterService(typeof(IAudioPlayer), service);
            AudioPlayer = service;
        }

        public void DeregisterAudioPlayer(IAudioPlayer _)
        {
            DeregisterService(typeof(IAudioPlayer));
            AudioPlayer = null;
        }
    }
}
