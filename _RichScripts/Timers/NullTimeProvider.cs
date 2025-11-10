namespace RichPackage.Timers
{
    public class NullTimeProvider : ITimeProvider
    {
        public static readonly NullTimeProvider Shared = new NullTimeProvider();

        public float DeltaTime { get; }
        public float UnscaledDeltaTime { get; }
        public float FixedDeltaTime { get; set; }
        public float Time { get; }
        public float RealTime { get; }
        public float TimeScale { get; set; }
        public float UnscaledTime { get; }
    }
}

