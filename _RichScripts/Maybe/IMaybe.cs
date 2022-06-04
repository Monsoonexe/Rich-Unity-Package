namespace RichPackage
{
    /// <summary>
    /// Useful in scenarios where you need to determine if a value is Maybe or not
    /// </summary>
    public interface IMaybe<T>
    {
        T Value { get; }
        bool HasValue { get; }
        bool HasNoValue { get; }
    }
}
