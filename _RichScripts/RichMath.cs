using System;
using UnityEngine;

public static class RichMath
{
    //these functions are faster than Mathf.Clamp because they aren't marshalled.

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

    public static int AbsoluteValue(int i)
        => i >= 0 ? i : -i;

    /// <summary>
    /// Runs ~twice as fast as Mathf.Abs().
    /// </summary>
    /// <param name="f"></param>
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

    public static int Min(int x, int y) => x < y ? x : y;
    public static int Max(int x, int y) => x > y ? x : y;
    public static float Min(float x, float y) => x < y ? x : y;
    public static float Max(float x, float y) => x > y ? x : y;

    /// <summary>
    /// 10.37435 (1) = 10.3
    /// </summary>
    /// <param name="a"></param>
    /// <param name="decimalDigits"></param>
    /// <returns></returns>
    public static float TruncateMantissa(this ref float a, int decimalDigits)
    {
        if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
            return (int)a;

        const int TEN = 10;//base 10
        var truncator = 1.0f;//start at 1 for multiply

        //exponentiate to move desired portion into integer section
        for (var i = 0; i < decimalDigits; ++i)
            truncator *= TEN;

        //move decimal left, truncate mantissa, move decimal back right
        return a = ((int)(a * truncator)) / truncator;
    }

    /// <summary>
    /// 10.37435 (1) = 10.3
    /// </summary>
    /// <param name="a"></param>
    /// <param name="decimalDigits"></param>
    /// <returns></returns>
    public static float TruncateMantissa(float a, int decimalDigits)
    {
        if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
            return (int)a;

        const int TEN = 10;//base 10
        var truncator = 1.0f;//start at 1 for multiply

        //exponentiate to move desired portion into integer section
        for (var i = 0; i < decimalDigits; ++i)
            truncator *= TEN;

        //move decimal left, truncate mantissa, move decimal back right
        return a = ((int)(a * truncator)) / truncator;
    }

    #region Quick Convert

    /// <summary>
    /// Converts radians to degrees
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float ToDeg(this float v)
    {
        return v * 180 / Mathf.PI;
    }
    /// <summary>
    /// Converts radians to degrees
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double ToDeg(this double v)
    {
        return v * 180 / Mathf.PI;
    }
    /// <summary>
    /// Converts degrees to radians
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float ToRad(this float v)
    {
        return v * Mathf.PI / 180;
    }
    /// <summary>
    /// Converts degrees to radians
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static double ToRad(this double v)
    {
        return v * Mathf.PI / 180;
    }

    #endregion
}
