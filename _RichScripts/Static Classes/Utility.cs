using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

namespace RichPackage
{
    /// <seealso cref="ConstStrings"/>
    public static class Utility
    {
        /// <summary>
        /// 0-based index, similar to how 2-d, Row-Major arrays work.
        /// </summary>
        /// <example>RowColumnToIndex(1, 2, 3) = 5 </example>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int RowColumnToIndex(int row, int column, int columnCount)
            => (row * columnCount) + column;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// Returns an item from <paramref name="objs"/> that is closest to <paramref name="worldPoint"/>.
        /// </summary>
        public static TObject GetClosestObject<TObject>(
            IEnumerable<(TObject obj, Transform transform)> objs, Vector3 worldPoint)
        {
            TObject result = default; // return value
            float minDist = Mathf.Infinity;

            foreach ((TObject obj, Transform transform) in objs)
            {
                // use squared-distance strategy: we care about relative distance, not exact distance.
                Vector3 direction = transform.position - worldPoint;
                float distanceSquared = direction.sqrMagnitude; // no sqrt

                if (distanceSquared < minDist)
                {
                    result = obj;
                    minDist = distanceSquared;
                }
            }

            return result;
        }

        /// <summary>
        /// Finds a path to each item and returns the closest one by path distance.
        /// </summary>
        /// <param name="objs">Things the <paramref name="agent"/> might want to get to.</param>
        /// <param name="agent">The one who wants to get to a thing, but optimally the closest one.</param>
        /// <returns>(The thing, How to get to the thing).</returns>
        public static (TObject Item, NavMeshPath Path) GetClosestObjectByPath<TObject>(
            IEnumerable<(TObject Item, Transform Transform)> objs, NavMeshAgent agent)
        {
            var path = new NavMeshPath();
            var shortestPath = new NavMeshPath();
            TObject result = default; // return value
            float prevDistance = Mathf.Infinity; // cached calculation

            foreach ((TObject item, Transform transform) in objs)
            {
                path.ClearCorners();
                bool canPath = agent.CalculatePath(transform.position, path);

                if (canPath)
                {
                    float distance = path.CalculateRelativeDistance();
                    if (distance < prevDistance)
                    {
                        result = item;

                        // swap 'working' path with the ref to the shortest (avoids 'new')
                        NavMeshPath tmp = path;
                        path = shortestPath;
                        shortestPath = tmp;

                        // cache expensive calc
                        prevDistance = distance;
                    }
                }
            }

            return (result, shortestPath);
        }
    }
}
