
/// <summary>
/// Integer that is always in range ['floor', 'ceiling'].
/// ++ -- will modify integer by 'step'.
/// </summary>
public struct LoopingInt
{
    public const int DEFAULT_STEP = 1;

    private int _index;

    public int Index
    {
        get => _index;
        private set
        {
            //clamp
            if (value < floor)
                value = floor;
            else if (value > ceiling)
                value = ceiling;
            //set
            _index = value;
        }
    }

    /// <summary>
    /// Lowest possible value.
    /// </summary>
    public int floor;

    /// <summary>
    /// Highest possible value.
    /// </summary>
    public int ceiling;

    /// <summary>
    /// Affects step value of -- and ++ operations.
    /// </summary>
    public int step;

    #region Constructors

    public LoopingInt(int startingIndex, int floor, int ceiling, 
        int incrementStep = DEFAULT_STEP)
    {
        _index = 0; //have to do this before Index can be used.
        this.floor = floor;
        this.ceiling = ceiling;
        this.step = incrementStep;

        Index = startingIndex;
    }

    /// <summary>
    /// Index from [floor, ceiling], step = 1.
    /// </summary>
    public LoopingInt(int floor, int ceiling)
    {
        _index = 0; //have to do this before Index can be used.
        this.floor = floor;
        this.ceiling = ceiling;
        step = DEFAULT_STEP;

        Index = floor;
    }

    /// <summary>
    /// Index from [0, ceiling], step = 1. Useful for array indexers
    /// </summary>
    /// <param name="ceiling"></param>
    public LoopingInt(int ceiling)
    {
        _index = 0;
        floor = 0;
        this.ceiling = ceiling;
        step = DEFAULT_STEP;
    }

    #endregion

    /// <summary>
    /// Index = floor;
    /// </summary>
    /// <returns></returns>
    public LoopingInt ResetIndex()
    {
        Index = floor;
        return this;
    }

    #region Operators

    /// <summary>
    /// Index.
    /// </summary>
    /// <param name="a"></param>
    public static implicit operator int (LoopingInt a) => a.Index;

    /// <summary>
    /// Add step to index.
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static LoopingInt operator ++(LoopingInt a)
    {
        a.Index += a.step;
        return a;
    }

    /// <summary>
    /// Subtract step from index
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static LoopingInt operator --(LoopingInt a)
    {
        a.Index -= a.step;
        return a;
    }

    /// <summary>
    /// Straight add with disregard to step.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static LoopingInt operator +(LoopingInt a, int b)
    {
        a.Index += b;
        return a;
    }

    /// <summary>
    /// Straight subtract with disregard to step.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static LoopingInt operator -(LoopingInt a, int b)
    {
        a.Index -= b;
        return a;
    }

    /// <summary>
    /// Inc index by b times step
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static LoopingInt operator *(LoopingInt a, int b)
    {
        a.Index += b * a.step;
        return a;
    }

    /// <summary>
    /// Dec index by b times step
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static LoopingInt operator /(LoopingInt a, int b)
    {
        a.Index -= b * a.step;
        return a;
    }

    #endregion
}
