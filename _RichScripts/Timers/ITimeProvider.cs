namespace RichPackage.Timers
{
    public interface ITimeProvider
    {
        float DeltaTime { get; }
        float UnscaledDeltaTime { get; }
        float FixedDeltaTime { get; set; }
        float Time { get; }
        float RealTime { get; }
        float TimeScale { get; set; }
        float UnscaledTime { get; }
    }
}
