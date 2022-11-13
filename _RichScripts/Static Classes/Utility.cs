using System;
using System.Runtime.CompilerServices;

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
            var watch = SimpleTimer.StartNew();
            action();
            return watch.Elapsed;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
