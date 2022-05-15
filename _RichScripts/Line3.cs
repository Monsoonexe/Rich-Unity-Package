using System;
using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// A line is between two points in space, A and B. It's not a ray, cuz no direction.
    /// </summary>
    [Serializable]
    public struct Line3
    {
        /// <summary>
        /// Start Point
        /// </summary>
        public Vector3 A;//like start point

        /// <summary>
        /// End Point
        /// </summary>
        public Vector3 B;//and endpoint
        
        #region Constructors
        
        public Line3(Vector3 a, Vector3 b)
        {
            A = a;
            B = b;
        }
        
        #endregion
    }
}
