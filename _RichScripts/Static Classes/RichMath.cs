using System;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace RichPackage
{
    public static class RichMath
    {
        //these functions are faster than Mathf.Clamp because they aren't marshalled.
        const int TEN = 10;//base 10

        public const float RAD_2_DEG = 180 / Mathf.PI;
        public const float DEG_2_RAD = Mathf.PI / 180;

        //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
        //http://www.iquilezles.org/www/articles/minispline/minispline.htm
        
        public static Vector3 CatmullRomPosition(float t, in Vector3 p0, in Vector3 p1, in Vector3 p2, in Vector3 p3)
        {
            //The coefficients of the cubic polynomial (except the 0.5f * which I added later for performance)
            Vector3 a = 2f * p1;
            Vector3 b = p2 - p0;
            Vector3 c = 2f * p0 - 5f * p1 + 4f * p2 - p3;
            Vector3 d = -p0 + 3f * p1 - 3f * p2 + p3;

            //The cubic polynomial: (a + b * t + c * t^2 + d * t^3) / 2
            Vector3 pos = 0.5f * (a + (b * t) + (c * (t * t)) + (d * (t * t * t)));

            return pos;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Wrap(int value, int min, int max)
        {
            int range = max - min + 1;  // Calculate the range of valid values
            int result = (value - min) % range;  // Calculate the wrapped value
            if (result < 0)  // Handle negative wrapped values
            {
                result += range;
            }
            return result + min;  // Adjust for the minimum value
        }

        #region Clamp

        /// <summary>
        /// Returns a value within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
            => value.CompareTo(max) > 0 ?
                max : value.CompareTo(min) < 0 ?
                min : value;

        /// <summary>
        /// Returns a value within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(int value, int min, int max)
            => value > max ? max
                : value < min ? min
                : value;

        /// <summary>
        /// Returns a value within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Clamp(uint value, uint min, uint max)
            => value > max ? max
                : value < min ? min
                : value;

        /// <summary>
        /// Returns a value within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max)
            => value > max ? max
                : value < min ? min
                : value;

        /// <summary>
        /// Returns a value within [<paramref name="min"/>, <paramref name="max"/>].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(double value, double min,
            double max)
            => value > max ? max
                : value < min ? min
                : value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ClampMin(float value, float min)
        {
            return value < min ? min : value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ClampMax(float value, float max)
        {
            return value > max ? max : value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampMin(int value, int min)
        {
            return value < min ? min : value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ClampMax(int value, int max)
        {
            return value > max ? max : value;
        }

        #endregion Clamp

        #region Absolute Value

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int AbsoluteValue(this int i) => Math.Abs(i);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float AbsoluteValue(this float f) => Math.Abs(f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 AbsoluteValue(in Vector2 v)
            => new Vector2(AbsoluteValue(v.x),
                AbsoluteValue(v.y));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 AbsoluteValue(in Vector3 v)
            => new Vector3(AbsoluteValue(v.x),
                AbsoluteValue(v.y), AbsoluteValue(v.z));

        /// <summary>
        /// f = |f|
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetAbsoluteValue(this ref float f)
            => f = f.AbsoluteValue();

        #endregion

        #region Min/Max

        /// <summary>
        /// Returns lesser of the two inputs.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Min<T>(this T x, T y)
            where T : IComparable<T>
            => x.CompareTo(y) < 0 ? x : y;

        /// <summary>
        /// Returns greater of the two inputs.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Max<T>(this T x, T y)
            where T : IComparable<T>
            => x.CompareTo(y) > 0 ? x : y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Min(int x, int y) => x < y ? x : y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Max(int x, int y) => x > y ? x : y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float x, float y) => x < y ? x : y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float x, float y) => x > y ? x : y;

        /// <summary>
        /// Returns true if x is in inverval [min, max].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWithin(int x, int min, int max)
            => (x >= min && x <= max);

        /// <summary>
        /// Returns true if x is in inverval [min, max].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWithin(float x, float min, float max)
            => (x >= min && x <= max);

        #endregion

        #region Rounding

        /// <summary>
        /// 10.37565 (2) = 10.38
        /// </summary>
        /// <param name="a"></param>
        /// <param name="decimalDigits"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Round(float a, int decimalDigits)
            => (float)Math.Round(
                a, decimalDigits,
                MidpointRounding.AwayFromZero);//Like elementary school

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MoveDecimalRight(float a, int places)
        {
            float truncator = 1.0f;//start at 1 for multiply

            //exponentiate to move desired portion into integer section
            for (int i = 0; i < places; ++i)
                truncator *= TEN;

            return a * truncator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float MoveDecimalLeft(float a, int places)
        {
            float truncator = 1.0f;//start at 1 for multiply

            //exponentiate to move desired portion into integer section
            for (int i = 0; i < places; ++i)
                truncator *= TEN;

            return a / truncator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetPowerOfTen(int power)
        {
            float factor = 1.0f;//start at 1 for multiply

            //exponentiate to move desired portion into integer section
            for (int i = 0; i < power; ++i)
                factor *= TEN;

            return factor;
        }

        public static float CeilingAtNthDecimal(float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return Mathf.Ceil(a);

            float factor = GetPowerOfTen(decimalDigits);

            a *= factor;//move decimal place right
            a = Mathf.Ceil(a);//ceil
            a /= factor;//move decimal place left

            return a;
        }

        public static float FloorAtNthDecimal(float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return Mathf.Floor(a);

            float factor = GetPowerOfTen(decimalDigits);

            a *= factor;//move decimal place right
            a = Mathf.Floor(a);//ceil
            a /= factor;//move decimal place left

            return a;
        }

        #endregion

        #region Quick Convert

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToDeg(this float v)
            => v * RAD_2_DEG;

        /// <summary>
        /// Converts radians to degrees
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDeg(this double v)
            => v * RAD_2_DEG;

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToRad(this float v)
            => v * DEG_2_RAD;

        /// <summary>
        /// Converts degrees to radians
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToRad(this double v)
            => v * DEG_2_RAD;

        #endregion

        public static int GreatestCommonDenominator(int a, int b)
            => a == 0 ? b : b == 0 ? a
                : GreatestCommonDenominator(b, a % b);

        public static bool IsPrime(this long n)
        {
            //Primality test using 6k+-1 optimization.
            //early exits
            if (n <= 3)
                return n > 1; //everything <= 1 is non-prime
            else if (n % 2 == 0 || n % 3 == 0)
                return false; //no even number is prime, nor multiple of 3

            //test all others
            long i = 5;
            while (i * i <= n)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
                i += 6;
            }
            return true;
        }
    }
}
