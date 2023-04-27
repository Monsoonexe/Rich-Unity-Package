
namespace RichPackage
{
    /// <summary>
    /// Default timer that calculates the elapsed time based on <see cref="UnityEngine.Time.time"/>.
    /// </summary>
    public struct SimpleTimer
    {
        public float startTime;

        public float Elapsed => App.Time - startTime;

        public static SimpleTimer StartNew()
        {
            var t = new SimpleTimer();
            t.Reset();
            return t;
        }

        public void Reset()
        {
            startTime = App.Time;
        }

        #region Object

        public override string ToString()
        {
            return Elapsed.ToString();
        }

        public string ToString(string formatting)
        {
            var time = TimeSpan.FromSeconds(Elapsed);
            return time.ToString(formatting);
        }

        #endregion Object

        #region Operator Overloads

        public static bool operator > (SimpleTimer timer, float duration)
            => timer.Elapsed > duration;

        public static bool operator < (SimpleTimer timer, float duration)
            => timer.Elapsed < duration;

        public static bool operator >= (SimpleTimer timer, float duration)
            => timer.Elapsed >= duration;

        public static bool operator <= (SimpleTimer timer, float duration)
            => timer.Elapsed <= duration;

        public static float operator + (SimpleTimer timer, float value)
            => timer.Elapsed + value;

        public static float operator - (SimpleTimer timer, float value)
            => timer.Elapsed - value;

        public static float operator * (SimpleTimer timer, float value)
            => timer.Elapsed * value;

        public static float operator / (SimpleTimer timer, float value)
            => timer.Elapsed / value;

        #endregion Operator Overloads
    }
}
