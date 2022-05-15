using System;
using UnityEngine;
using UnityEngine.Events;

namespace RichPackage.Events
{
    /// <summary>
    /// My collider has collided with another Collider.
    /// Useful for implementing hit boxes.
    /// </summary>
    [Serializable]
    public class TriggerCollisionUnityEvent : UnityEvent<Collider, Collider>
    {
        //exists
    }
}
