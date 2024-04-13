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
            => Random.Range(range.x, range.y);

        /// <summary>
        /// Returns a random value between x [inclusive] and y [inclusive].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RandomRange(this Vector2Int range)
            => Random.Range(range.x, range.y);
    }
}
