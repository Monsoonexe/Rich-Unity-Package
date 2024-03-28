using System.Collections;
using System.Collections.Generic;

namespace RichPackage.Collections
{
    public readonly struct DualEnumerableEnumerator<T1, T2> : IEnumerator<(T1, T2)>
    {
        private readonly IEnumerator<T1> enumerator1;
        private readonly IEnumerator<T2> enumerator2;

        public DualEnumerableEnumerator(IEnumerable<T1> collection1, IEnumerable<T2> collection2)
        {
            enumerator1 = collection1.GetEnumerator();
            enumerator2 = collection2.GetEnumerator();
        }

        public readonly (T1, T2) Current => (enumerator1.Current, enumerator2.Current);

        readonly object IEnumerator.Current => Current;

        public readonly bool MoveNext() => enumerator1.MoveNext() && enumerator2.MoveNext();

        public readonly void Reset()
        {
            enumerator1.Reset();
            enumerator2.Reset();
        }

        public readonly void Dispose()
        {
            enumerator1.Dispose();
            enumerator2.Dispose();
        }
    }
}