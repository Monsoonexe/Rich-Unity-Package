namespace RichPackage.PrimitivePointers
{
    /// <summary>
    /// Pointer-like object for an <see langword="int"/>.
    /// </summary>
    /// <remarks>Maybe 'BoxedInt'?</remarks>
    public sealed class IntObject : APtr<int>
    {
        public IntObject() : this(default) { }

        public IntObject(int value)
        {
            Value = value;
        }
    }

    public abstract class APtr<T> : VoidPtr
    {
        public T Value;

        public APtr() : this(default) { }

        public APtr(T value)
        {
            Value = value;
        }

        public static implicit operator T (APtr<T> ptr) => ptr.Value;
    }

    // basically 'object'
    public abstract class VoidPtr { }
}
