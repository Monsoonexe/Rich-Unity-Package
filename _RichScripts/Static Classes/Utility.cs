using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

//clarifications between UnityEngine and System classes
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace RichPackage
{
    /// <summary>
    /// 
    /// </summary>
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

        #region Random 

        /// <summary>
        /// Has an n probability of returning 'true'.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Chance(float n) => Random.Range(0f, 1f) <= n;
        
        /// <param name="dice">[0, inf)</param>
        /// <param name="sides">[1, inf)</param>
        /// <param name="mod">(whatever, whatever)</param>
        /// <returns>e.g. 2d6 + mod</returns>
        public static int RollDice(int dice, int sides, int mod = 0)
        {   //validate
            Debug.Assert(dice >= 0 && sides > 0, "[Utility] Invalid RollDice input: " 
                + dice.ToString() + " " + sides.ToString());

            var result = mod;
            var upperRandomLimit = sides + 1;//because upper bound of random is exclusive
            for (; dice > 0; --dice)
                result += Random.Range(1, upperRandomLimit);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RollDie(int sides) => Random.Range(1, sides + 1);

        #endregion

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
        /// Times how long the action took to complete and returns that time in milliseconds.
        /// </summary>
        public static long SpeedTest(Action action)
        {
            Stopwatch watch = Stopwatch.StartNew();
            action();
            watch.Stop();
            return watch.ElapsedMilliseconds;
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
