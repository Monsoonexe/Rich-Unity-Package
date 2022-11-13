
namespace RichPackage
{
    /// <summary>
    /// Default timer that calculates the elapsed time based on <see cref="UnityEngine.Time.time"/>.
    /// </summary>
    public struct SimpleTimer
    {
        public float startTime;

        public float Elapsed => RichAppController.Time - startTime;

        public static SimpleTimer StartNew()
        {
            var t = new SimpleTimer();
            t.Reset();
            return t;
        }

        public void Reset()
        {
            startTime = RichAppController.Time;
        }

        public static bool operator >(SimpleTimer timer, float duration)
            => timer.Elapsed > duration;

        public static bool operator <(SimpleTimer timer, float duration)
            => timer.Elapsed < duration;

        public static bool operator >=(SimpleTimer timer, float duration)
            => timer.Elapsed >= duration;

        public static bool operator <=(SimpleTimer timer, float duration)
            => timer.Elapsed <= duration;
    }
}
