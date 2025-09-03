namespace RichPackage.Timers
{
    /// <summary>
    /// Time provider that wraps <see cref="UnityEngine.Time"/>.
    /// </summary>
    public class UnityTimeProvider : ITimeProvider
    {
        public float DeltaTime { get => UnityEngine.Time.deltaTime; }
        public float UnscaledDeltaTime { get => UnityEngine.Time.unscaledDeltaTime; }
        public float UnscaledTime { get => UnityEngine.Time.unscaledTime; }
        public float FixedDeltaTime
        {
            get => UnityEngine.Time.fixedDeltaTime;
            set => UnityEngine.Time.fixedDeltaTime = value;
        }
        public float FixedTime { get => UnityEngine.Time.fixedTime; }
        public float Time { get => UnityEngine.Time.time; }
        public float RealTime { get => UnityEngine.Time.realtimeSinceStartup; }
        public float TimeScale
        {
            get => UnityEngine.Time.timeScale;
            set => UnityEngine.Time.timeScale = value;
        }

        // TODO ....
    }
}
