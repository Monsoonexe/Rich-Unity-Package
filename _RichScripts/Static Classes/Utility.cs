﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DG.Tweening;

//clarifications between UnityEngine and System classes
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
/// <seealso cref="ConstStrings"/>
public static class Utility
{
    //stuff we can all use!
    #region Communal Properties

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    #endregion

    /// <summary>
    /// 0-based index, similar to how 2-d, Row-Major arrays work.
    /// </summary>
    /// <example>RowColumnToIndex(1, 2, 3) = 5 </example>
    public static int RowColumnToIndex(int row, int column, int columnCount)
        => row * columnCount + column;

    #region Random 

    /// <summary>
    /// Has an n probability of returning 'true'.
    /// </summary>
    /// <returns></returns>
    public static bool Chance(double n) => Random.Range(0, 1) <= n;
    
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
    

    public static int RollDie(int sides) => Random.Range(1, sides + 1);

    #endregion

    #region DelayInvokation

    /// <summary>
    /// Must be called from StartCoroutine() on a Mono.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEnumerator InvokeAfterDelay(this Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    public static IEnumerator InvokeAfterDelay(this Action callback,
        YieldInstruction yieldInstruction)
    {
        yield return yieldInstruction;
        callback();
    }

    /// <summary>
    /// Version of InvokeAfterDelay that uses DOTween instead of coroutines
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public static Sequence InvokeAfterDelaySequence(this Action callback, float delay)
    {
        return DOTween.Sequence() //new Sequence()
            .AppendInterval(delay)
            .AppendCallback(callback.Invoke);
    }

    public static IEnumerator InvokeAtEndOfFrame(this Action callback)
    {
        yield return WaitForEndOfFrame;
        callback();
    }

    public static IEnumerator InvokeNextFrame(this Action callback)
    {
        yield return null;
        callback();
    }

    /// <summary>
    /// Postpone execution until after so many frames have passed.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="frames"></param>
    /// <returns></returns>
    public static IEnumerator InvokeAfterFrames(this Action callback, int frames)
    {
        for(var i = 0; i < frames; ++i)
        {
            yield return null;
        }
        callback();
    }

    #endregion

    /// <summary>
    /// Set this and all children (and on) to given layer.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    public static void SetLayerRecursively(this Transform obj, int newLayer)
    {
        obj.gameObject.layer = newLayer;
        var childCount = obj.childCount;
        for (var i = 0; i < childCount; ++i)
        {
            var child = obj.GetChild(i);
            SetLayerRecursively(child, newLayer);
        }
    }

    /// <summary>
    /// Set this and all children (and on) to given layer.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    public static void SetLayerRecursively(this GameObject gameObj, int newLayer)
        => SetLayerRecursively(gameObj.transform, newLayer);

    /// <summary>
    /// Case insensitive check. True iff source == "true".
    /// </summary>
    /// <param name="source">Source string.</param>
    /// <param name="result">Result is invalid if 'false' is returned.</param>
    /// <returns>True if parse was successful. Result is invalid if 'false' is returned.</returns>
    public static bool TryStringToBool(this string source, out bool result)
    {
        var lowerSource = source.ToLower();
        var isTrue = lowerSource == ConstStrings.TRUE_STRING_LOWER;
        var isFalse = lowerSource == ConstStrings.FALSE_STRING_LOWER;
        var success = isTrue || isFalse;

        result = isTrue;

        return success;
    }

    #region Functional Iterating

    /// <summary>
    /// Simply loops a given number of times
    /// </summary>
    /// <param name="cycles"></param>
    /// <param name="action"></param>
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
}
