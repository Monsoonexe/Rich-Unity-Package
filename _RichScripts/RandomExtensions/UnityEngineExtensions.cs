using RichPackage.RNG;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RichPackage.RandomExtensions
{
    /// <summary>
    /// Contains helper methods for dealing with randomness for unity engine objects.
    /// </summary>
    public static class UnityEngineExtensions
    {
        /// <summary>
        /// Returns a random value between x [inclusive] and y [inclusive].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float RandomRange(this Vector2 range)
            => Rng.Current.Range(range.x, range.y);

        /// <summary>
        /// Returns a random value between x [inclusive] and y [inclusive].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RandomRange(this Vector2Int range)
            => Rng.Current.Range(range.x, range.y);

        /// <summary>
        /// Randomly draw a value on <paramref name="animationCurve"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sample(this AnimationCurve animationCurve)
            => Sample(animationCurve, Rng.Current);

        /// <param name="t">T on curve.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(this AnimationCurve animationCurve, float t)
        {
            var y = animationCurve.Evaluate(t);
            var rgn = Rng.Current.Next();
            return rgn < y;
        }

        /// <summary>
        /// Randomly draw a value on <paramref name="animationCurve"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sample(this AnimationCurve animationCurve, IRandomNumberGenerator rng)
        {
            float rgn = rng.Next();
            return animationCurve.Evaluate(rgn);
        }
    }
}
