﻿using System.Diagnostics;
using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage
{
    /// <summary>
    /// Base class that includes helper methods for MonoBehaviour.
    /// </summary>
    /// <seealso cref="RichScriptableObject"/>
    [SelectionBase]
    public class RichMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, TextArea]
        [PropertyOrder(-5)]
#pragma warning disable IDE0052 // Remove unread private members
		private string developerDescription = "Please enter a description or a note.";
#pragma warning restore IDE0052 // Remove unread private members
#endif

		/// <summary>
		/// This call will be stripped out of Builds. Use anywhere.
		/// </summary>
		/// <param name="newDes"></param>
		[Conditional(ConstStrings.UNITY_EDITOR)]
        public void SetDevDescription(string newDes)
        {
#if UNITY_EDITOR
            developerDescription = newDes;
#endif
        }

        /// <summary>
        /// Cached Transform.
        /// </summary>
        [SerializeField, HideInInspector]
        protected Transform myTransform;

        /// <summary>
        /// Cached Transform  
        /// (because native 'transform' is secretly <see cref="GameObject.GetComponent{Transform}"/>()' which is expensive on repeat).
        /// </summary>
        public new Transform transform { get => myTransform; }

        protected virtual void Reset()
		{
            myTransform = gameObject.GetComponent<Transform>();
		}

        protected virtual void Awake()
            => myTransform = GetComponentIfNull<Transform>(myTransform);

        /// <summary>
        /// Set a reference to a singleton to the given instance if valid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="singletonRef">Reference to the Singleton object, typically a static class variable.</param>
        /// <returns>False if a SingletonError occured.</returns>
        protected static bool InitSingleton<T>(T instance, ref T singletonRef,
            bool dontDestroyOnLoad = true)
            where T : RichMonoBehaviour
        {
            var valid = true; //return value
            if (singletonRef == null)
            {   //we are the singleton
                singletonRef = instance;
                if (dontDestroyOnLoad) instance.DontDestroyOnLoad();
            }
            else if (!instance.Equals(singletonRef))
            {   //there are two Singletons
                //throw new SingletonException(string.Format("[SingletonError] Two instances of a singleton exist: {0} and {1}.",
                //instance.ToString(), singletonRef.ToString()));
                valid = false;
            }
            return valid;
        }

        protected T GetComponentIfNull<T>(Maybe<T> maybeComponent)
            where T : Component
        {
            T comp;
            return ((comp = maybeComponent.GetValueOrDefault()) != null)
                ? comp: gameObject.GetComponent<T>();
        }

        protected T GetComponentInChildrenIfNull<T>(Maybe<T> maybeComponent)
            where T : Component
        {
            T comp;
            return ((comp = maybeComponent.GetValueOrDefault()) != null)
                ? comp : gameObject.GetComponentInChildren<T>();
        }

        /// <summary>
        /// Try to get component on self, in children, and then add it on self if not found.
        /// </summary>
        protected T GetComponentOnPrefabAtAllCosts<T>(Maybe<T> maybeComponent)
            where T : Component
        {
            T comp;
            if ((comp = maybeComponent.GetValueOrDefault()) != null)
                return comp;
            if ((comp = gameObject.GetComponent<T>()) != null)
                return comp;
            if ((comp = GetComponentInChildren<T>()) != null)
                return comp;
            
            return gameObject.AddComponent<T>();
        }

        [Conditional(ConstStrings.UNITY_EDITOR)]
        public void Editor_MarkDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public static T Construct<T>() where T : RichMonoBehaviour
            => new GameObject(typeof(T).Name).AddComponent<T>();

        public static T Construct<T>(string name) where T : RichMonoBehaviour
            => new GameObject(name).AddComponent<T>();
    }
}
