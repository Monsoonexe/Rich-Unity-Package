/* 
 * motivation: c-style casting is a pain in the ass. it breaks up the flow of typing to
 * go backwards and put parentheses in (nevermind getting them correct the first time)
 *  
 *  // standard c-style flow
 *  => Math.Clamp(((int)Math.Log(workUnits)) + 1, MIN_TASK_COUNT, MAX_TASK_COUNT);
 *  
 *  // what is the first expression to be evaluated?
 *  // when 1 is added, what is the resulting type? is it an int before or after the addition?
 *  // how many arguments are provided to Clamp?
 *  // pop quiz! which has a higher priority in order of operations? casting or adding: (int)1.1f + 1
 *  
 *  // functional (fluent) paradigm:
    => Math.Log(workUnits).ToInt().Add(1).Clamp(MIN_TASK_COUNT, MAX_TASK_COUNT);    

 *  // I argue that if you can answer the above questions faster for the 2nd option, then it is 
 *  // the more maintainable, readable, and testable paradigm.
 *  // this is very easy to type and very easy to read. your eyes go left-right and never backtrack.
 *  // no deciphering or order of operations required!
 */

using System;
using System.Runtime.CompilerServices;

namespace RichPackage.FunctionalProgramming
{
    public static class ArithmeticFunctions
    {
        #region Integers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Add(this int a, int b) => a + b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Increment(this int a) => a + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Decrement(this int a) => a - 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Subtract(this int a, int b) => a - b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Multiply(this int a, int b) => a * b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Divide(this int a, int b) => a / b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Negate(this int a) => -a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Squared(this int a) => a * a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Cubed(this int a) => a * a * a;

        #endregion Integers

        #region Floats

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Ceiling(this float f)
        {
            return (float)Math.Ceiling(f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Divide(this float f, int x)
        {
            return f / x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Divide(this float f, float x)
        {
            return f / x;
        }

        #endregion Floats
    }
}
