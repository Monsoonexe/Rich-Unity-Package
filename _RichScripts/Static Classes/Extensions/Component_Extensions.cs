using UnityEngine;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace RichPackage
{
    /// <seealso cref="Behaviour_Extensions"/>
    public static class Component_Extensions
    {
        //useful for delegates and events
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyGameObject(this Component c)
            => UnityEngine.Object.Destroy(c.gameObject);

        //useful for delegates and events
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DestroyComponent(this Component c)
            => UnityEngine.Object.Destroy(c);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DontDestroyOnLoad(this Component c)
        {
            c.transform.SetParent(null);
            UnityEngine.Component.DontDestroyOnLoad(c.gameObject);
        }

        //useful for delegates and events
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveTrue(this Component a) 
            => a.gameObject.SetActive(true);

        //useful for delegates and events
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveFalse(this Component a) 
            => a.gameObject.SetActive(false);

        //useful for delegates and events
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActive(this Component a, bool active) 
            => a.gameObject.SetActive(active);

        public static void SetAllEnabled<TBehaviour>(this IList<TBehaviour> behaviours, bool enabled)
            where TBehaviour : Behaviour
        {
            int length = behaviours.Count;
            for (int i = 0; i < length; i++)
            {
                behaviours[i].enabled = enabled;
            }
        }
    }
}
