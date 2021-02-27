using UnityEngine;

/// <summary>
/// Holds commonly used string values to help avoid excess garbage generation.
/// </summary>
/// <seealso cref="Utility"/>
public static class ConstStrings
{
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

    //common keys
    public static readonly string INPUT_INTERACT_KEY = "E";

    //common prompts
    public static readonly string INFO = "Info";
    public static readonly string NO = "No";
    public static readonly string OKAY = "Okay";
    public static readonly string YES = "Yes";
    public static readonly string CONTINUE = "Continue";

    //common spacers
    public static readonly string SPACE_SLASH_SPACE = " / ";
    public static readonly string SPACE = " ";

    //punctuation
    public static readonly string EXCLAMATION_POINT = "!";

    //multiplier
    public static readonly string X_UPPER = "X";
    public static readonly string X_LOWER = "x";

    /// <summary>
    /// Cached string versions of commonly-used numbers. 
    /// Reduces garbage GENERATION and fragmentation.
    /// </summary>
    public static readonly string[] NUMBER_STRINGS;

    /// <summary>
    ///  0 - 100 covers all times, dates, and percentages and only costs
    ///  about 204 bytes (I think)
    /// </summary>
    public const int NUMBER_STRINGS_LIMIT = 1000;

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
//        if(num > NUMBER_STRINGS_LIMIT)
//        {
//            Debug.Log("[ConstStrings] "+ num.ToString() + 
//                " not in constant string array, so a new string was " +
//                "generated. If this happens quite often, consider increasing " +
//                "NUMBER_STRINGS_LIMIT: (" + NUMBER_STRINGS_LIMIT.ToString() + 
//                ") or making a dictionary to track new entries.");
//        }
//#endif
        return (num <= NUMBER_STRINGS_LIMIT && num >= 0) ?
            NUMBER_STRINGS[num] : num.ToString();//garbage
    }

    #region Constructor

    static ConstStrings()
    {
        //preload first few ints as strings
        NUMBER_STRINGS = new string[NUMBER_STRINGS_LIMIT + 1];//for 0
        for(var i = 0; i <= NUMBER_STRINGS_LIMIT; ++i)
        {
            NUMBER_STRINGS[i] = i.ToString();//cache string versions of common numbers
        }
    }

    #endregion
}
