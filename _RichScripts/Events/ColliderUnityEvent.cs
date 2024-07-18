using System;
using UnityEngine;
using UnityEngine.Events;

namespace RichPackage.Events
{
    [Serializable]
    public class ColliderUnityEvent : UnityEvent<Collider>, IUnityEvent<Collider> { }

    [Serializable]
    public class Collider2DUnityEvent : UnityEvent<Collider2D>, IUnityEvent<Collider2D> { }

    [Serializable]
    public class Collision2DUnityEvent : UnityEvent<Collision2D>, IUnityEvent<Collision2D> { }
}
