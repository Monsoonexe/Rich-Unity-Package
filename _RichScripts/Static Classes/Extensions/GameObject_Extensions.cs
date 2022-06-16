﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace RichPackage
{
    public static class GameObject_Extensions
    {
        /// <summary>
        /// Shortcut for a.enabled = true;
        /// </summary>
        /// <param name="a"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveTrue(this GameObject a)
            => SetActiveInternal(a, true);

        /// <summary>
        /// Shortcut for a.enabled = false;
        /// </summary>
        /// <param name="a"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveFalse(this GameObject a)
            => SetActiveInternal(a, false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetActiveInternal(GameObject a, bool enabled)
        {
            //it's been tested that checking before calling to native code
            //is more performant if the change isn't needed
            if (a.activeSelf != enabled)
                a.SetActive(enabled);
        }

        /// <summary>
        /// Shortcut for a.enabled = false;
        /// </summary>
        /// <param name="a"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveToggle(this GameObject a)
            => a.SetActive(!a.activeSelf);

        /// <summary>
        /// Shortcut for Destroy(gameObject);
        /// </summary>
        /// <param name="a"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this GameObject a)
            => UnityEngine.Object.Destroy(a);

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, 
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/> if not not null,
        /// otherwise <see cref="GameObject.AddComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this GameObject gameObject, T comp = null)
            where T : Component
        {
            if (comp != null)
                return comp;

            if ((comp = gameObject.GetComponent<T>()) != null)
                return comp;

            return gameObject.AddComponent<T>();
        }

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, 
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetComponentIfNull<T>(this GameObject gameObject, T comp = null)
            where T : Component
        {
            return (comp == null) ? gameObject.GetComponent<T>() : comp;
        }

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, 
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/>.
        /// </returns>
        public static T GetComponentIfNull<T>(this GameObject gameObject, Maybe<T> maybeComponent)
            where T : Component
        {
            T comp;
            return ((comp = maybeComponent.GetValueOrDefault()) != null)
                ? comp : gameObject.GetComponent<T>();
        }

        /// <summary>
        /// Get name of the <see cref="GameObject"/> prefixed 
        /// with the current <see cref="UnityEngine.SceneManagement.Scene"/> name. <br/>
        /// e.g. "MainMenuScene/MainMenu".
        /// </summary>
        public static string GetNameWithScene(this GameObject a)
            => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "/" + a.name;

        /// <summary>
        /// Perform an action on every Transform on each of its children, etc, recursively.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="newLayer"></param>
        public static void ForEachChildRecursive(this GameObject obj, 
            Action<Transform> action)
        {
            var transform = obj.GetComponent<Transform>();
            var childCount = transform.childCount;
            for (var i = childCount - 1; i >= 0; --i)
            {
                var child = transform.GetChild(i);
                child.ForEachTransformRecursive(action);
            }
        }

        /// <summary>
        /// Get a new List each TComponent that is on each the root of each GameObject.
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <param name="gameObjects"></param>
        /// <returns>A list of non-null component references.</returns>
        public static List<TComponent> GetComponents<TComponent>
            (this IList<GameObject> gameObjects)
            where TComponent : Component
        {
            int count = gameObjects.Count;
            List<TComponent> components = new List<TComponent>(count);
            for(int i = 0; i < count; ++i)
                if(gameObjects[i].TryGetComponent(out TComponent comp))
                    components.Add(comp);
            return components;
        }
    }
}
