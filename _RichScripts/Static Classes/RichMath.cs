using System;
using UnityEngine;

public static class RichMath
{
    //these functions are faster than Mathf.Clamp because they aren't marshalled.
    const int TEN = 10;//base 10

    public static readonly float RAD_2_DEG = 180 / Mathf.PI;
    public static readonly float DEG_2_RAD = Mathf.PI / 180;

    //Returns a position between 4 Vector3 with Catmull-Rom spline algorithm
    //http://www.iquilezles.org/www/articles/minispline/minispline.htm
    /// <summary>
    /// 
    /// </summary>
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

    #region Clamp

    /// <summary>
    /// Clamps IComparable value
    /// </summary> 
    public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        => value.CompareTo(max) > 0 ?
            max : value.CompareTo(min) < 0 ?
            min : value;

    public static int Clamp(int value, int min, int max)
        => value.CompareTo(max) > 0 ?
            max : value.CompareTo(min) < 0 ?
            min : value;

    public static float Clamp(float value, float min, float max)
        => value.CompareTo(max) > 0 ?
            max : value.CompareTo(min) < 0 ?
            min : value;

    public static double Clamp(double value, double min, double max)
        => value.CompareTo(max) > 0 ?
            max : value.CompareTo(min) < 0 ?
            min : value;

    #endregion

    #region Absolute Value

    /// <summary>
    /// Runs ~twice as fast as Mathf.Abs().
    /// </summary>
    /// <returns>Because Mathf.Abs() is managed code.</returns>
    public static int AbsoluteValue(int i)
        => i >= 0 ? i : -i;

    /// <summary>
    /// Runs ~twice as fast as Mathf.Abs().
    /// </summary>
    /// <returns>Because Mathf.Abs() is managed code.</returns>
    public static float AbsoluteValue(float f)
        => f >= 0 ? f : -f;

    public static Vector2 AbsoluteValue(this Vector2 v)
        => new Vector2(AbsoluteValue(v.x), AbsoluteValue(v.y));

    public static Vector3 AbsoluteValue(Vector3 v)
        => new Vector3(AbsoluteValue(v.x), AbsoluteValue(v.y), AbsoluteValue(v.z));

    /// <summary>
    /// f = |f|
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    public static void SetAbsoluteValue(this ref float f)
        => f = f >= 0 ? f : -f;

    #endregion

    #region Min/Max

    public static int Min(int x, int y) => x < y ? x : y;
    public static int Max(int x, int y) => x > y ? x : y;
    public static float Min(float x, float y) => x < y ? x : y;
    public static float Max(float x, float y) => x > y ? x : y;

    #endregion

    #region Rounding

    /// <summary>
    /// 10.37565 (2) = 10.38
    /// </summary>
    /// <param name="a"></param>
    /// <param name="decimalDigits"></param>
    /// <returns></returns>
    public static float Round(float a, int decimalDigits)
        => (float)Math.Round(
            a, decimalDigits, 
            MidpointRounding.AwayFromZero);//Like elementary school

    public static float MoveDecimalRight(float a, int places)
    {
        var truncator = 1.0f;//start at 1 for multiply

        //exponentiate to move desired portion into integer section
        for (var i = 0; i < places; ++i)
            truncator *= TEN;

        return a * truncator;
    }

    public static float MoveDecimalLeft(float a, int places)
    {
        var truncator = 1.0f;//start at 1 for multiply

        //exponentiate to move desired portion into integer section
        for (var i = 0; i < places; ++i)
            truncator *= TEN;

        return a / truncator;
    }

    private static float GetPowerOfTen(int power)
    {
        var factor = 1.0f;//start at 1 for multiply

        //exponentiate to move desired portion into integer section
        for (var i = 0; i < power; ++i)
            factor *= TEN;

        return factor;
    }

    public static float CeilingAtNthDecimal(float a, int decimalDigits)
    {
        if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
            return Mathf.Ceil(a);

        var factor = GetPowerOfTen(decimalDigits);

        a *= factor;//move decimal place right
        a = Mathf.Ceil(a);//ceil
        a /= factor;//move decimal place left

        return a;
    }

    public static float FloorAtNthDecimal(float a, int decimalDigits)
    {
        if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
            return Mathf.Floor(a);

        var factor = GetPowerOfTen(decimalDigits);

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
    public static float ToDeg(this float v)
    {
        return v * RAD_2_DEG;
    }
    /// <summary>
    /// Converts radians to degrees
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double ToDeg(this double v)
    {
        return v * RAD_2_DEG;
    }
    /// <summary>
    /// Converts degrees to radians
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float ToRad(this float v)
    {
        return v * DEG_2_RAD;
    }
    /// <summary>
    /// Converts degrees to radians
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double ToRad(this double v)
    {
        return v * DEG_2_RAD;
    }
    
    #endregion

    public static int GreatestCommonDenominator(int a, int b)
    {
        //base case
        if(a == 0) return b;
        else if (b == 0) return a;
        return GreatestCommonDenominator(b, a % b);
    }
}
