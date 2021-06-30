
namespace RichPackage.Managed
{
    /// <summary>
    /// 
    /// </summary>
    public interface IManagedBehaviour
    {
        //void ManagedPreAwake();
        //void ManagedAwake();
        //void ManagedStart();
        //void ManagedUpdate();
    }

    public interface IManagedPreAwake : IManagedBehaviour
    {
        void ManagedPreAwake();
    }

    public interface IManagedAwake : IManagedBehaviour
    {
        void ManagedAwake();
    }

    public interface IManagedStart : IManagedBehaviour
    {
        void ManagedStart();
    }

    public interface IManagedEarlyUpdate : IManagedBehaviour
    {
        void ManagedEarlyUpdate();
    }

    public interface IManagedUpdate : IManagedBehaviour
    {
        void ManagedUpdate();
    }

    public interface IManagedFixedUpdate : IManagedBehaviour
    {
        void ManagedFixedUpdate();
    }

    public interface IManagedLateUpdate : IManagedBehaviour
    {
        void ManagedLateUpdate();
    }

    public interface IManagedOnApplicationQuit : IManagedBehaviour
    {
        void ManagedOnApplicationQuit();
    }

    public interface IManagedOnApplicationPause : IManagedBehaviour
    {
        void ManagedOnApplicationPause(bool pause);
    }
}
