
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

    public int floor;
    public int ceiling;
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

    public LoopingInt(int floor, int ceiling)
    {
        _index = 0; //have to do this before Index can be used.
        this.floor = floor;
        this.ceiling = ceiling;
        step = DEFAULT_STEP;

        Index = floor;
    }

    public LoopingInt(int ceiling)
    {
        _index = 0;
        this.floor = 0;
        this.ceiling = ceiling;
        step = DEFAULT_STEP;
    }

    #endregion

    public LoopingInt ResetIndex()
    {
        Index = floor;
        return this;
    }

    #region Operators

    public static implicit operator int (LoopingInt a) 
        => a.Index;

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
