using System;
using System.Runtime.CompilerServices;

namespace RichPackage.FunctionalProgramming
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Convenient way to cast without needing parentheses. <br/>
        /// Equivalent to `(T)obj`.
        /// </summary>
        /// <returns>The object cast to the given type.</returns>
        /// <exception cref="InvalidCastException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CastTo<T>(this object obj) => (T)obj;

        /// <summary>
        /// Cast <paramref name="a"/> to a base class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TBaseClass CastDown<TSuperClass, TBaseClass>(this TSuperClass a, out TBaseClass b)
            where TSuperClass : TBaseClass
        {
            return b = a;
        }

        /// <summary>
        /// Cast <paramref name="a"/> to a super class.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSuperClass CastTo<TBaseClass, TSuperClass>(this TBaseClass a, out TSuperClass b)
            where TSuperClass : TBaseClass
        {
            return b = (TSuperClass)a;
        }

        /// <summary>
        /// Convenient way to cast without needing parentheses. <br/>
        /// Equivalent to <code>obj <see langword="as"/> T</code>
        /// </summary>
        /// <returns>The object cast to the given type or null if the cast is not valid.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T CastAs<T>(this object obj)
            where T : class => obj as T;

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
