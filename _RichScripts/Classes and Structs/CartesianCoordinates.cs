
[System.Serializable]
public struct CartesianCoordinates
{
    public int x;
    public int y;

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="coords"></param>
    public CartesianCoordinates(CartesianCoordinates coords)
    {
        this.x = coords.x;
        this.y = coords.y;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public CartesianCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return "(" + x.ToString() + ", " + y.ToString() + ")";
    }

    #region Static Properties

    /// <summary>
    /// Sort of works like a default Constructor.
    /// </summary>
    public static CartesianCoordinates Zero
        => new CartesianCoordinates(0, 0);

    public static CartesianCoordinates Up
        => new CartesianCoordinates(0, 1);

    public static CartesianCoordinates UpRight
        => new CartesianCoordinates(1, 1);

    public static CartesianCoordinates Right
        => new CartesianCoordinates(1, 0);

    public static CartesianCoordinates DownRight
        => new CartesianCoordinates(1, -1);

    public static CartesianCoordinates Down
        => new CartesianCoordinates(0, -1);

    public static CartesianCoordinates DownLeft
        => new CartesianCoordinates(-1, -1);

    public static CartesianCoordinates Left
        => new CartesianCoordinates(-1, 0);

    public static CartesianCoordinates UpLeft
        => new CartesianCoordinates(-1, 1);

    #endregion

    #region Operators

    public static CartesianCoordinates operator +(CartesianCoordinates a, CartesianCoordinates b)
        => new CartesianCoordinates(a.x + b.x, a.y + b.y);

    public static CartesianCoordinates operator -(CartesianCoordinates a)
        => new CartesianCoordinates(-a.x, -a.y);

    public static CartesianCoordinates operator -(CartesianCoordinates a, CartesianCoordinates b)
        => new CartesianCoordinates(a + (-b));

    #endregion
}
