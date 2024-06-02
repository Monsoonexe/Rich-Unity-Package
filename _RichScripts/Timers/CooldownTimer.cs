namespace RichPackage
{
    public struct CooldownTimer
    {
        private SimpleTimer timer;
        public float Duration;
        public bool IsExpired => timer.Elapsed > Duration;
        public float Elapsed => timer.Elapsed;
        public CooldownTimer(float duration)
        {
            timer = default;
            this.Duration = duration;
        }
        public void Reset() => timer.Reset();
        public void Reset(float duration)
        {
            Duration = duration;
            Reset();
        }
    }
}
