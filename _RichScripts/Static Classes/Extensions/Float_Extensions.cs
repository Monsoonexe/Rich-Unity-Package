using System.Runtime.CompilerServices;

namespace RichPackage
{
    /// <summary>
    /// Extensions and helpers for <see langword="float"/>s.
    /// </summary>
    public static class Float_Extensions
	{

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(this float n)
        {
            return n % 2 == 0;
        }

        /// <summary>
        /// 10.37435 (1) = 10.3
        /// </summary>
        /// <param name="a"></param>
        /// <param name="decimalDigits"></param>
        /// <returns></returns>
        public static float TruncateMantissa(this ref float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return (int)a;

            const int TEN = 10;//base 10
            var truncator = 1.0f;//start at 1 for multiply

            //exponentiate to move desired portion into integer section
            for (var i = 0; i < decimalDigits; ++i)
                truncator *= TEN;

            //move decimal left, truncate mantissa, move decimal back right
            return a = ((int)(a * truncator)) / truncator;
        }

        /// <summary>
        /// 10.37435 (1) = 10.3
        /// </summary>
        /// <param name="a"></param>
        /// <param name="decimalDigits"></param>
        /// <returns></returns>
        public static float TruncateMantissa(float a, int decimalDigits)
        {
            if (decimalDigits <= 0) //cast it to and from an int to clear mantissa
                return (int)a;

            const int TEN = 10;//base 10
            var truncator = 1.0f;//start at 1 for multiply

            //exponentiate to move desired portion into integer section
            for (var i = 0; i < decimalDigits; ++i)
                truncator *= TEN;

            //move decimal left, truncate mantissa, move decimal back right
            return a = ((int)(a * truncator)) / truncator;
        }

    }
}
