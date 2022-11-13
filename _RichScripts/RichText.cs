// source: https://github.com/skibitsky/unity-rich-text/blob/master/RichText.cs

using System.Text;

/// <summary>
/// Contains helpers for adding Rich Text tags to your strings, like colors, italics, and bold.
/// </summary>
public static class RichText
{
    #region Modifications

    /// <summary>
    /// Renders the text in boldface.	
    /// </summary>
    /// <param name="str">Original strect</param>
    public static string Bold(this string str) { return $"<b>{str}</b>"; }

    /// <summary>
    /// Renders the text in italics.	
    /// </summary>
    /// <param name="str">Original strect</param>
    public static string Italic(this string str) { return $"<i>{str}</i>"; }

    #endregion Modifications

    #region Size

    /// <summary>
    /// Sets the size of the text according to the parameter value, given in pixels.	
    /// </summary>
    /// <param name="str">Original strect</param>
    /// <param name="size">Desired text size in pixels</param>
    public static string Size(this string str, int size) { return $"<size={size}>{str}</size>"; }

    #endregion Size

    #region Colors

    /// <summary> Paint it Aqua!</summary>
    public static string Aqua(this string str) { return ColorProcessor(str, "aqua"); }

    /// <summary> Paint it Black!</summary>
    public static string Black(this string str) { return ColorProcessor(str, "black"); }

    /// <summary> Paint it Blue!</summary>
    public static string Blue(this string str) { return ColorProcessor(str, "blue"); }

    /// <summary> Paint it Brown!</summary>
    public static string Brown(this string str) { return ColorProcessor(str, "brown"); }

    /// <summary> Paint it Cyan!</summary>
    public static string Cyan(this string str) { return ColorProcessor(str, "cyan"); }

    /// <summary> Paint it Dark Blue!</summary>
    public static string DarkBlue(this string str) { return ColorProcessor(str, "darkblue"); }

    /// <summary> Paint it Fuchsia!</summary>
    public static string Fuchsia(this string str) { return ColorProcessor(str, "fuchsia"); }

    /// <summary> Paint it Green!</summary>
    public static string Green(this string str) { return ColorProcessor(str, "green"); }

    /// <summary> Paint it Grey!</summary>
    public static string Grey(this string str) { return ColorProcessor(str, "grey"); }

    /// <summary> Paint it Light Blue!</summary>
    public static string LightBlue(this string str) { return ColorProcessor(str, "lightblue"); }

    /// <summary> Paint it Lime!</summary>
    public static string Lime(this string str) { return ColorProcessor(str, "lime"); }

    /// <summary> Paint it Magenta!</summary>
    public static string Magenta(this string str) { return ColorProcessor(str, "magenta"); }

    /// <summary> Paint it Maroon!</summary>
    public static string Maroon(this string str) { return ColorProcessor(str, "maroon"); }

    /// <summary> Paint it Navy!</summary>
    public static string Navy(this string str) { return ColorProcessor(str, "navy"); }

    /// <summary> Paint it Olive!</summary>
    public static string Olive(this string str) { return ColorProcessor(str, "olive"); }

    /// <summary> Paint it Orange!</summary>
    public static string Orange(this string str) { return ColorProcessor(str, "orange"); }

    /// <summary> Paint it Purple!</summary>
    public static string Purple(this string str) { return ColorProcessor(str, "purple"); }

    /// <summary> Paint it Red!</summary>
    public static string Red(this string str) { return ColorProcessor(str, "red"); }

    /// <summary> Paint it Silver!</summary>
    public static string Silver(this string str) { return ColorProcessor(str, "silver"); }

    /// <summary> Paint it Teal!</summary>
    public static string Teal(this string str) { return ColorProcessor(str, "teal"); }

    /// <summary> Paint it White!</summary>
    public static string White(this string str) { return ColorProcessor(str, "white"); }

    /// <summary> Paint it Yellow!</summary>
    public static string Yellow(this string str) { return ColorProcessor(str, "yellow"); }

    public static string ColorProcessor(string str, string color)
    {
        // $"<color={color}>{str}</color>";
        return StringBuilderCache.Rent()
            .Append("<color=")
            .Append(color)
            .Append('>')
            .Append(str)
            .Append("</color>")
            .ToStringAndReturn();
    }

    #endregion Colors
}
