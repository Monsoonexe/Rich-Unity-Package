using System.Text;
using System.Runtime.CompilerServices;

namespace RichPackage
{
    internal static class StringBuilderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string OutputAndClear(this StringBuilder sb)
        {
            string output = sb.ToString();
            sb.Clear();
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBuilder ClearOrNew(this StringBuilder sb)
        {
            if (sb == null)
                sb = new StringBuilder(128);
            else
                sb.Clear();

            return sb;
        }
    }
}
