﻿// credit to Jason Weimann and Unity College
using UnityEngine;

namespace RichPackage
{
    public static class Vector3_Extensions
    {
        /// <summary>
        /// Handy function to make assigning/copying vectors easier
        /// myPostion = somePosition.With(z = 1);//same, but with different z-value
        /// </summary>
        /// <param name="a">Vector3 source to copy</param>
        /// <param name="x">x value, if you want</param>
        /// <param name="y">y value, if you want</param>
        /// <param name="z">z value, if you want</param>
        /// <returns></returns>
        public static Vector3 With(this Vector3 a,
            float? x = null, float? y = null, float? z = null)
            => new Vector3(x ?? a.x, y ?? a.y, z ?? a.z);

        public static Vector3 WithX(this Vector3 a, float x)
            => new Vector3(x, a.y, a.z);

        public static Vector3 WithY(this Vector3 a, float y)
            => new Vector3(a.x, y, a.z);

        public static Vector3 WithZ(this Vector3 a, float z)
            => new Vector3(a.x, a.y, z);

        public static Vector3 PlusX(this Vector3 a, float x)
            => new Vector3(a.x + x, a.y, a.z);

        public static Vector3 PlusY(this Vector3 a, float y)
            => new Vector3(a.x, a.y + y, a.z);

        public static Vector3 PlusZ(this Vector3 a, float z)
            => new Vector3(a.x, a.y, a.z + z);

        public static Vector3 MinusX(this Vector3 a, float x)
            => new Vector3(a.x - x, a.y, a.z);

        public static Vector3 MinusY(this Vector3 a, float y)
            => new Vector3(a.x, a.y - y, a.z);

        public static Vector3 MinusZ(this Vector3 a, float z)
            => new Vector3(a.x, a.y, a.z - z);

        /// <summary>
        /// Handy function to make assigning/copying vectors easier
        /// myPostion = somePosition.With(z = 1);//same, but with different z-value
        /// </summary>
        /// <param name="a">Vector3 source to copy</param>
        /// <param name="x">x value, if you want</param>
        /// <param name="y">y value, if you want</param>
        /// <param name="z">z value, if you want</param>
        /// <returns></returns>
        public static Vector3Int With(this Vector3Int a,
            int? x = null, int? y = null, int? z = null)
            => new Vector3Int(x ?? a.x, y ?? a.y,z ?? a.z);
        
        public static Vector3Int WithX(this Vector3Int a, int x)
            => new Vector3Int(x, a.y, a.z);

        public static Vector3Int WithY(this Vector3Int a, int y)
            => new Vector3Int(a.x, y, a.z);

        public static Vector3Int WithZ(this Vector3Int a, int z)
            => new Vector3Int(a.x, a.y, z);

        //can be called as Vector3_Extensions.With(a, ....), but that's ugly, so the 'this'
        //keyword allows you to call the method from an instance, like a.With(...);

        //following two lines are equivalent.
        //Vector3 myVec = new Vector3(myTransform.position.x, 150, myTransform.position.z);

        //Vector3 otherVec = myTransform.position.With(y: 150);
        public static Vector3 CircleWrap(this Vector3 a)
        {
            if (a.y > 180)
                a.y -= 360;

            if (a.x > 180)
                a.x -= 360;

            if (a.z > 180)
                a.z -= 360;

            if (a.y < -180)
                a.y += 360;

            if (a.x < -180)
                a.x += 360;

            if (a.z < -180)
                a.z += 360;
                
            return a;
        }
    }
}
