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
        /// Compares references.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is(this object obj, object other)
            => ReferenceEquals(obj, other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNot<T>(this object obj)
            where T : class => !(obj is T);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryCast<T>(this object source, out T result)
        {
            if (source is T cast)
            {
                result = cast;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="obj"/> is null.
        /// </summary>
        /// <param name="paramName">The name of the parameter being checked.</param>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static object ThrowIfNull(this object obj, string paramName)
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
            return obj; // allow chaining
        }
        
        /// <summary>
        /// Create a single-element array containing only the given item.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToSingleElementArray<T>(this T obj)
            => new T[] { obj };
    }
}
