using System;
using System.Runtime.CompilerServices;

namespace RichPackage
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convenient way to cast without needing parentheses. <br/>
        /// Equivalent to `(T)obj`.
        /// </summary>
        /// <returns>The object cast to the given type.</returns>
        /// <exception cref="System.InvalidCastException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CastTo<T>(this object obj) => (T)obj;

        /// <summary>
        /// Convenient way to cast without needing parentheses. <br/>
        /// Equivalent to `obj as T`.
        /// </summary>
        /// <returns>The object cast to the given type or null if the cast is not valid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CastAs<T>(this object obj)
            where T : class  => obj as T;
           
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="obj"/> is null.
        /// </summary>
        /// <param name="paramName">The name of the parameter being checked.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(this object obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }
    }
}
