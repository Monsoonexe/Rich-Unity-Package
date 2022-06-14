
namespace RichPackage
{
    public abstract class AIDFactory<T> : IDFactory<T>
    {
        public T InvalidID = default;

        protected T previousID;

        public abstract T GetNext();

        public abstract void Reset();
    }
}
