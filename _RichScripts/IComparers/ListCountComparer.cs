using System.Collections;
using System.Collections.Generic;

namespace RichPackage
{
	public sealed class IListCountComparer : IComparer<IList>
	{
		public int Compare(IList x, IList y) => x.Count.CompareTo(y.Count);

		public static readonly IListCountComparer Default = new IListCountComparer();
    }

    public sealed class IListCountReverseComparer : IComparer<IList>
    {
        public int Compare(IList x, IList y) => y.Count.CompareTo(x.Count);

        public static readonly IListCountComparer Default = new IListCountComparer();
    }
}
