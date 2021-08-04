
/// <summary>
/// 
/// </summary>
public static class Bool_Extensions
{
    //for situations where bools just don't cut it
    public const int TRUE_int = 1;
    public const int FALSE_int = 0;

    /// <summary>
    /// A && B in function form.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool And(this bool a, bool b) => a && b;

    public static bool Nand(this bool a, bool b) => !(a && b);

    public static bool Nor(this bool a, bool b) => !(a || b);

    public static bool Not(this bool a) => !a;

    public static bool Or(this bool a, bool b) => a || b;

    public static bool Xnor(this bool a, bool b) => a == b;
        //!((a && !b) || (!a && b));

    /// <summary>
    /// One or the other, but not both. Returns true if a and b are different truth values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static bool Xor(this bool a, bool b) => a != b;
        //(a && !b) || (!a && b)

    /// <summary>
    /// Shortcut for !a;
    /// </summary>
    /// <param name="a"></param>
    public static bool Negate(this bool a) => !a;

    /// <summary>
    /// Shortcut for a = !a;
    /// </summary>
    /// <param name="a"></param>
    public static bool Toggle(this ref bool a) => a = !a;

}
