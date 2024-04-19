
namespace RichPackage
{
    /// <summary>
    /// Default timer that calculates the elapsed time based on <see cref="UnityEngine.Time.time"/>.
    /// </summary>
    public struct SimpleTimer
    {
        public float startTime;

        public float Elapsed => UnityEngine.Time.time - startTime;

        public static SimpleTimer StartNew()
        {
            var t = new SimpleTimer();
            t.Reset();
            return t;
        }

        public void Reset()
        {
            startTime = UnityEngine.Time.time;
        }

        public override string ToString() => Elapsed.ToString();

        public static bool operator >(SimpleTimer timer, float duration)
            => timer.Elapsed > duration;

        public static bool operator <(SimpleTimer timer, float duration)
            => timer.Elapsed < duration;

        public static bool operator >=(SimpleTimer timer, float duration)
            => timer.Elapsed >= duration;

        public static bool operator <=(SimpleTimer timer, float duration)
            => timer.Elapsed <= duration;

        public static bool operator >(float duration, SimpleTimer timer)
            => duration > timer.Elapsed;

        public static bool operator <(float duration, SimpleTimer timer)
            => duration < timer.Elapsed;

        public static bool operator >=(float duration, SimpleTimer timer)
            => duration >= timer.Elapsed;

        public static bool operator <=(float duration, SimpleTimer timer)
            => duration <= timer.Elapsed;

    }
}
