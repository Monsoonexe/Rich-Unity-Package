using UnityEngine;
using System.Runtime.CompilerServices;
using System.Text;

namespace RichPackage
{
    public static class Color_Extensions
    {
        /// <summary>
        /// Prefer WithR(float) instead.
        /// </summary>
        /// <remarks>But seriously, 4 conditionals????</remarks>
        public static Color With(this Color c,
            float? r = null, float? g = null, float? b = null,
            float? a = null)
            => new Color
            (
                r ?? c.r,
                g ?? c.g,
                b ?? c.b,
                a ?? c.a
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithR(this in Color c, float r)
            => new Color(r, c.g, c.b, c.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithG(this in Color c, float g)
            => new Color(c.r, g, c.b, c.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithB(this in Color c, float b)
            => new Color(c.r, c.g, b, c.a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Color WithA(this in Color c, float a)
            => new Color(c.r, c.g, c.b, a);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Opaque(ref this Color c)
            => c.a = 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Transparent(ref this Color c)
            => c.a = 0;

        /// <summary>
        /// Converts <paramref name="c"/> to its hex representation. e.g. '#RRGGBBAA'.
        /// </summary>
        public static string ToHex(this in Color c)
        {
            return StringBuilderCache.Rent(16)
                .Append('#')
                .Append(ToByte(c.r).ToString("X2"))
                .Append(ToByte(c.g).ToString("X2"))
                .Append(ToByte(c.b).ToString("X2"))
                .Append(ToByte(c.a).ToString("X2"))
                .ToStringAndReturn();

            //return $"#{ToByte(c.r):X}{ToByte(c.g):X}{ToByte(c.b):X}{ToByte(c.a):X}";
        }

        /// <summary>
        /// Converts <paramref name="c"/> to its hex representation. e.g. '#RRGGBBAA'.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToHex(this in Color32 c)
        {
            return $"#{c.r:X2}{c.g:X2}{c.b:X2}{c.a:X2}";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static byte ToByte(float f)
        {
            f = Mathf.Clamp01(f);
            return (byte)(f * byte.MaxValue);
        }
    }
}
