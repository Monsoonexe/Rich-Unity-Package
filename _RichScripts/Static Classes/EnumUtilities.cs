using System;
using System.Collections.Generic;

namespace RichPackage
{
    public static class EnumUtilities
    {
        /// <summary>
        /// Creates a dictionary that maps enums to their string representations.
        /// Useful because reflection is expensive and this operation caches the results.
        /// </summary>
        public static Dictionary<int, string> EnumNamedValues<TEnum>()
            where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum));
            var result = new Dictionary<int, string>(values.Length); //return value

            foreach (int item in values)
                result.Add(item, Enum.GetName(typeof(TEnum), item));

            return result;
        }
    }
}
