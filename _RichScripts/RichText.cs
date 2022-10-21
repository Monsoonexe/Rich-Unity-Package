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
    /// <param name="obj">Original object</param>
    public static string Bold(this string obj) { return $"<b>{obj}</b>"; }

    /// <summary>
    /// Renders the text in italics.	
    /// </summary>
    /// <param name="obj">Original object</param>
    public static string Italic(this string obj) { return $"<i>{obj}</i>"; }

    #endregion Modifications

    #region Size

    /// <summary>
    /// Sets the size of the text according to the parameter value, given in pixels.	
    /// </summary>
    /// <param name="obj">Original object</param>
    /// <param name="size">Desired text size in pixels</param>
    public static string Size(this string obj, int size) { return $"<size={size}>{obj}</size>"; }

    #endregion Size

    #region Colors

    /// <summary> Paint it Aqua!</summary>
    public static string Aqua(this string obj) { return ColorProcessor(obj, "aqua"); }

    /// <summary> Paint it Black!</summary>
    public static string Black(this string obj) { return ColorProcessor(obj, "black"); }

    /// <summary> Paint it Blue!</summary>
    public static string Blue(this string obj) { return ColorProcessor(obj, "blue"); }

    /// <summary> Paint it Brown!</summary>
    public static string Brown(this string obj) { return ColorProcessor(obj, "brown"); }

    /// <summary> Paint it Cyan!</summary>
    public static string Cyan(this string obj) { return ColorProcessor(obj, "cyan"); }

    /// <summary> Paint it Dark Blue!</summary>
    public static string DarkBlue(this string obj) { return ColorProcessor(obj, "darkblue"); }

    /// <summary> Paint it Fuchsia!</summary>
    public static string Fuchsia(this string obj) { return ColorProcessor(obj, "fuchsia"); }

    /// <summary> Paint it Green!</summary>
    public static string Green(this string obj) { return ColorProcessor(obj, "green"); }

    /// <summary> Paint it Grey!</summary>
    public static string Grey(this string obj) { return ColorProcessor(obj, "grey"); }

    /// <summary> Paint it Light Blue!</summary>
    public static string LightBlue(this string obj) { return ColorProcessor(obj, "lightblue"); }

    /// <summary> Paint it Lime!</summary>
    public static string Lime(this string obj) { return ColorProcessor(obj, "lime"); }

    /// <summary> Paint it Magenta!</summary>
    public static string Magenta(this string obj) { return ColorProcessor(obj, "magenta"); }

    /// <summary> Paint it Maroon!</summary>
    public static string Maroon(this string obj) { return ColorProcessor(obj, "maroon"); }

    /// <summary> Paint it Navy!</summary>
    public static string Navy(this string obj) { return ColorProcessor(obj, "navy"); }

    /// <summary> Paint it Olive!</summary>
    public static string Olive(this string obj) { return ColorProcessor(obj, "olive"); }

    /// <summary> Paint it Orange!</summary>
    public static string Orange(this string obj) { return ColorProcessor(obj, "orange"); }

    /// <summary> Paint it Purple!</summary>
    public static string Purple(this string obj) { return ColorProcessor(obj, "purple"); }

    /// <summary> Paint it Red!</summary>
    public static string Red(this string obj) { return ColorProcessor(obj, "red"); }

    /// <summary> Paint it Silver!</summary>
    public static string Silver(this string obj) { return ColorProcessor(obj, "silver"); }

    /// <summary> Paint it Teal!</summary>
    public static string Teal(this string obj) { return ColorProcessor(obj, "teal"); }

    /// <summary> Paint it White!</summary>
    public static string White(this string obj) { return ColorProcessor(obj, "white"); }

    /// <summary> Paint it Yellow!</summary>
    public static string Yellow(this string obj) { return ColorProcessor(obj, "yellow"); }

    public static string ColorProcessor(string obj, string color)
    {
        // $"<color={color}>{obj}</color>";
        return StringBuilderCache.Rent()
            .Append("<color=")
            .Append(color)
            .Append('>')
            .Append(obj)
            .Append("</color>")
            .GetStringAndReturn();
    }

    #endregion Colors
}
