using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RichPackage
{
    public static class Transform_Extensions
    {
        ///<summary>
        ///
        ///</summary>
        public static float Angle(this Transform a, Transform b)
            => Vector3.Angle(a.position, b.position);

        ///<summary>
        ///
        ///</summary>
        public static float AngleForward(this Transform a, Transform b)
            => Vector3.Angle(a.forward, b.position);

        ///<summary>
        /// 0 <= Y <= 180
        ///</summary>
        public static float AngleForwardDOT(this Transform a, Transform b)
        {
            var worldSpaceForwardVector = a.TransformDirection(a.forward.normalized);//what is my forward vector in world space (normalized)
            var worldSpaceTargetDirection = (b.position - a.position).normalized;//direct from me to target (normalized)

            return Vector3.Dot(worldSpaceForwardVector, worldSpaceTargetDirection);//Ax * Bx + Ay * By + Az * Bz
        }

        ///<summary>
        /// -180 <= Y <= 180
        ///</summary>
        public static float AngleForwardSigned(this Transform a, Transform b)
        {
            
            var targetDir = b.position - a.position;
            var forward = a.forward;
            return Vector3.SignedAngle(targetDir, forward, Vector3.up);
        }

        ///<summary>
        ///
        ///</summary>
        public static float AngleArcTan(this Transform a, Transform b)
        {
            var targetsRelativePosition = a.InverseTransformPoint(b.position);//what is the target's position if it were in MY local space
            
            return Mathf.Atan2(targetsRelativePosition.x, targetsRelativePosition.y) * Mathf.Rad2Deg;//get arc tan, then convert to degrees
        }
        
        /// <param name="predicate">The condition you are looking for.</param>
        /// <param name="recursive">Should children, grandchildren, etc be searched.</param>
        /// <returns>The <see cref="Transform"/> that matched <paramref name="predicate"/>,
        /// otherwise <see langword="null"/> if not found.</returns>
        public static Transform Find(this Transform parent, Predicate<Transform> predicate, bool recursive)
        {
            foreach (Transform child in parent)
            {
                if (predicate(child))
                    return child;

                if (recursive)
                {
                    Transform found = Find(child, predicate, recursive);
                    if (found)
                        return found;
                }
            }
            return null;
        }

        /// <summary>
        /// Perform an action on every Transform on each of its children, etc, recursively.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="newLayer"></param>
        public static void ForEachChildRecursive(this Transform obj, Action<Transform> action)
        {
            var childCount = obj.childCount;
            for (var i = childCount - 1; i >= 0; --i)
            {
                var child = obj.GetChild(i);
                ForEachTransformRecursive(child, action);
            }
        }

        /// <summary>
        /// Perform an action on this Transform and every Transform on each of its children, etc, recursively.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="newLayer"></param>
        public static void ForEachTransformRecursive(this Transform obj, Action<Transform> action)
        {
            action(obj); //perform 
            var childCount = obj.childCount;
            for (var i = childCount - 1; i >= 0; --i)
            {
                var child = obj.GetChild(i);
                ForEachTransformRecursive(child, action);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform GetFirstChild(this Transform transform)
            => transform.GetChild(0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Transform GetLastChild(this Transform transform)
            => transform.GetChild(transform.childCount - 1);

        /// <summary>
        /// Set this and all children (and on) to given layer.
        /// </summary>
        public static void SetLayerRecursively(this Transform obj, int newLayer)
        {
            obj.gameObject.layer = newLayer;
            var childCount = obj.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = obj.GetChild(i);
                SetLayerRecursively(child, newLayer);
            }
        }
        
        public static Transform[] GetChildren(this Transform parent)
        {
            int count = parent.childCount;
            Transform[] result = new Transform[count];
            for (int i = 0; i < count; i++)
                result[i] = parent.GetChild(i);
            return result;
        }

        public static List<Transform> GetChildren(this Transform parent, List<Transform> list)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
                list.Add(parent.GetChild(i));
            return list;
        }
        
        /// <summary>
        /// Zeroes the postion and rotation of <paramref name="transform"/>.
        /// </summary>
        /// <returns><paramref name="transform"/> to allow chaining.</returns>
        public static Transform Reset(this Transform transform, Space space = Space.World)
        {
            if (space == Space.Self)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
            else // world
            {
                transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            return transform; // allow chaining
        }

        /// <summary>
        /// Sorts ascending by default, unless <paramref name="inverse"/>, then descending.
        /// </summary>
        /// credit: https://github.com/JimmyCushnie/JimmysUnityUtilities/blob/master/Scripts/Extensions/base%20Unity%20types/TransformExtensions.cs
        public static void SortChildrenAlphabetically(this Transform t, bool inverse = false)
        {
            var children = new List<Transform>(t.childCount);

            for (int i = 0; i < t.childCount; i++)
                children.Add(t.GetChild(i));


            if (inverse)
                children.Sort((t1, t2) => t2.name.CompareTo(t1.name));
            else
                children.Sort((t1, t2) => t1.name.CompareTo(t2.name));


            for (int i = 0; i < children.Count; ++i)
                children[i].SetSiblingIndex(i);
        }
    }
}
