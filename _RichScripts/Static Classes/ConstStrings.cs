﻿using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds commonly used string values to help avoid excess garbage generation.
/// </summary>
/// <seealso cref="Utility"/>
public static class ConstStrings
{
    public const string UNITY_EDITOR = "UNITY_EDITOR"; //used in [Conditional]
    
    //common tags
    public static readonly string TAG_PLAYER = "Player";
    public static readonly string TAG_MAIN_CAMERA = "MainCamera";

    //common inputs
    public static readonly string INPUT_AXIS_HOR = "Horizontal";
    public static readonly string INPUT_AXIS_VERT = "Vertical";
    public static readonly string INPUT_JUMP = "Jump";
    public static readonly string INPUT_PAUSE_MENU = "Cancel";
    public static readonly string INPUT_INTERACT = "Interact";
    public static readonly string INPUT_MOUSE_X = "Mouse X";
    public static readonly string INPUT_MOUSE_Y = "Mouse Y";
    public static readonly string INPUT_USE_TOOL = "Fire1";
    public static readonly string INPUT_BUILD_ITEM = "Fire1";//same as use tool

    /// <summary>
    /// Like Enter or Return.
    /// </summary>
    public static readonly string INPUT_SUBMIT = "Submit";

    //common prompts
    public static readonly string INFO = "Info";
    public static readonly string NO = "No";
    public static readonly string OKAY = "Okay";
    public static readonly string YES = "Yes";
    public static readonly string CONTINUE = "Continue";
    public static readonly string FALSE_STRING_LOWER = bool.FalseString.ToLower();
    public static readonly string TRUE_STRING_LOWER = bool.TrueString.ToLower();

    //common spacers
    public static readonly string SPACE_SLASH_SPACE = " / ";
    public static readonly string SPACE = " ";

    public const string EMPTY = "";

    //const chars
    public const char COLON = ':';
    public const char DOLLAR_SIGN = '$';
    public const char EXCLAMATION = '!';
    public const char MINUS = '-';
    public const char NEW_LINE = '\n';
    public const char NULL = '\0';
    public const char PERIOD = '.';
    public const char PLUS = '+';
    public const char QUESTION_MARK = '?';

    //multiplier
    public static readonly string X_UPPER = "X";
    public static readonly string X_LOWER = "x";

    /// <summary>
    /// Cached string versions of commonly-used numbers. 
    /// Reduces garbage GENERATION and fragmentation.
    /// </summary>
    public static readonly string[] INTEGER_STRINGS;

    public static readonly Dictionary<int, string>
        cachedStringDictionary;

    /// <summary>
    ///  0 - 100 covers all times, dates, and percentages and only costs
    ///  about 204 bytes (I think)
    /// </summary>
    public const int INTEGER_STRINGS_LIMIT = 100;//salt to needs of project.

    /// <summary>
    /// Always returns a string, either a cached one if it's range or a 
    /// garbage one.
    /// if out of bounds.
    /// </summary>
    /// <param name="num"></param>
    /// <returns>TODO cache new strings.</returns>
    public static string GetCachedString(int num)
    {
        //#if UNITY_EDITOR
        //        if(num > INTEGER_STRINGS_LIMIT)
        //        {
        //            Debug.Log("[ConstStrings] "+ num.ToString() + 
        //                " not in constant string array, so a new string was " +
        //                "generated. If this happens quite often, consider increasing " +
        //                "INTEGER_STRINGS_LIMIT: (" + INTEGER_STRINGS_LIMIT.ToString() + 
        //                ") or making a dictionary to track new entries.");
        //        }
        //#endif
        string str = null;
        if (num <= INTEGER_STRINGS_LIMIT && num >= 0) //O(1)
            str = INTEGER_STRINGS[num];
        else if(cachedStringDictionary.TryGetValue(num, out str)) //O(1)
        {
            //if it worked, str contains the requested string.
        }
        else
            str = num.ToString();//garbage

        return str;
    }
    
    #region Extensions

    public static string ToStringCached(this int num) => GetCachedString(num);

    public static string ToStringCached(this byte num)
    {
        int intNum = num; //upcast
        return intNum.ToStringCached();
    }

    public static string ToStringCached(this short num)
    {
        int intNum = num; //upcast
        return intNum.ToStringCached();
    }

    public static string ToStringCached(this ushort num)
    {
        int intNum = num; //upcast
        return intNum.ToStringCached();
    }

    public static string ToStringCached(this uint num)
    {
        if (num >= 0 && num <= INTEGER_STRINGS_LIMIT)
            return ToStringCached((int)num);
        return num.ToString();
    }

    public static string ToStringCached(this long num)
    {
        if (num >= int.MinValue && num <= INTEGER_STRINGS_LIMIT)
            return ToStringCached((int)num);
        return num.ToString();
    }

    #endregion

    #region Constructor

    static ConstStrings()
    {
        //preload first few ints as strings
        INTEGER_STRINGS = new string[INTEGER_STRINGS_LIMIT + 1];//for 0
        for(var i = 0; i <= INTEGER_STRINGS_LIMIT; ++i)
        {
            INTEGER_STRINGS[i] = i.ToString();//cache string versions of common numbers
        }

        //thousands and other int-keyed strings.
        cachedStringDictionary = new Dictionary<int, string>(10);

        for (var i = 2000; i < 10001; i += 1000)
            cachedStringDictionary.Add(i, i.ToString());
    }

    #endregion
}
