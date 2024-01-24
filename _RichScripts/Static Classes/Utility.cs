using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RichPackage.FunctionalProgramming;
using UnityEngine;

namespace RichPackage
{
    /// <seealso cref="ConstStrings"/>
    public static class Utility
    {
        /// <summary>
        /// 0-based index, similar to how 2-d, Row-Major arrays work.
        /// </summary>
        /// <example>RowColumnToIndex(1, 2, 3) = 5 </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RowColumnToIndex(int row, int column, int columnCount)
            => row * columnCount + column;

        #region Functional Iterating

        /// <summary>
        /// Simply loops a given number of times
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Repeat(Action action, int cycles)
        {
            for (int i = 0; i < cycles; ++i)
                action();
        }

        #endregion

        /// <summary>
        /// Times how long the action took to complete and returns that time in seconds.
        /// </summary>
        public static float Time(this Action action)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            action();
            return watch.Elapsed.TotalSeconds.ToFloat();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Returns an item from <paramref name="objs"/> that is closest to <paramref name="worldPoint"/>.
        /// </summary>
        public static TObject GetClosestObject<TObject>(
            IEnumerable<(TObject obj, Transform transform)> objs, Vector3 worldPoint)
            where TObject : class
        {
            TObject result = null; // return value
            float minDist = Mathf.Infinity;

            foreach ((TObject obj, Transform transform) in objs)
            {
                // use squared-distance strategy: we care about relative distance, not exact distance.
                Vector3 direction = transform.position - worldPoint;
                float distanceSquared = direction.sqrMagnitude; // no sqrt

                if (distanceSquared < minDist)
                {
                    result = obj;
                    minDist = distanceSquared;
                }
            }

            return result;
        }
    }
}
