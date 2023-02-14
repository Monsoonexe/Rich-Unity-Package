using UnityEngine;

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
        public static bool QuickEquals(this Object a, Object b)
        {
            return a.GetInstanceID() == b.GetInstanceID();
        }

    }
}
