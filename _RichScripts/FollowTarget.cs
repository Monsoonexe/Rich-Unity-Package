using System;
using UnityEngine;

namespace RichPackage
{
    public class FollowTarget : RichMonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);

        private void LateUpdate()
        {
            myTransform.position = target.position + offset;
        }
    }
}
