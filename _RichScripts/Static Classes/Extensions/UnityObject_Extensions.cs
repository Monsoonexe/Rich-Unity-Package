using RichPackage.GuardClauses;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	/// Extensions for <see cref="Object"/>.
	/// </summary>
	public static class UnityObject_Extensions
	{
        /// <summary>
        /// Returns true if the two objects are the same instance. Faster than the equality
        /// operator if both objects are known to be non-null and alive.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool QuickEquals(this Object a, Object b)
        {
            // return a.GetInstanceID() == b.GetInstanceID(); // results in 2 check for main thread identity
            return ReferenceEquals(a, b); // doesn't throw null-references
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAlive(this Object a)
        {
            GuardAgainst.ArgumentIsNull(a, nameof(a));
            return a; // implicit bool cast lol
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Exists(this Object a) => a; // implicit bool cast lol

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsDestroyed(this Object obj)
        {
            GuardAgainst.ArgumentIsNull(obj, nameof(obj));
            return !obj;
        }
    }
}
