using System;
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveTrue(this GameObject a)
            => SetActiveChecked(a, true);

        /// <summary>
        /// Shortcut for a.enabled = false;
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveFalse(this GameObject a)
            => SetActiveChecked(a, false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveChecked(this GameObject a, bool active)
        {
            // it's been tested that checking before calling to native code
            // is more performant if the change isn't needed
            if (a.activeSelf != active)
                a.SetActive(active);
        }

        /// <summary>
        /// Shortcut for `gameObject.SetActive(!gameObject.activeSelf);`
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

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/> if not not null,
        /// otherwise <see cref="GameObject.AddComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this GameObject gameObject, T comp = null)
            where T : Component
        {
            if (comp != null || (comp = gameObject.GetComponent<T>()) != null)
                return comp;

            return gameObject.AddComponent<T>();
        }

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetComponentIfNull<T>(this GameObject gameObject, T comp = null)
            where T : Component
        {
            return (comp != null) ? comp : gameObject.GetComponent<T>();
        }

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetComponentIfNull<T>(this GameObject gameObject, ref T comp)
            where T : Component
		{
            if (comp == null)
                comp = gameObject.GetComponent<T>();
		}

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
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
        /// Perform <paramref name="action"/> on every Transform on each of its children.
        /// </summary>
        public static void ForEachChild(this GameObject obj,
            Action<Transform> action)
        {
            var transform = obj.GetComponent<Transform>();
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                Transform child = transform.GetChild(i);
                action(child);
            }
        }

        /// <summary>
        /// Perform <paramref name="action"/> on every Transform on each of its children, recursively.
        /// </summary>
        public static void ForEachChildRecursive(this GameObject obj,
            Action<Transform> action)
        {
            var transform = obj.GetComponent<Transform>();
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                Transform child = transform.GetChild(i);
                child.ForEachTransformRecursive(action);
                action(child);
            }
        }

        /// <summary>
        /// Perform  <paramref name="action"/> on every GameObject on each of its children, recursively.
        /// </summary>
        public static void ForEachChild(this GameObject obj,
            Action<GameObject> action)
        {
            var transform = obj.GetComponent<Transform>();
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                GameObject go = transform.GetChild(i).gameObject;
                action(go);
            }
        }

        /// <summary>
        /// Perform  <paramref name="action"/> on every GameObject on each of its children, recursively.
        /// </summary>
        public static void ForEachChildRecursive(this GameObject obj,
            Action<GameObject> action)
        {
            var transform = obj.GetComponent<Transform>();
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                GameObject go = transform.GetChild(i).gameObject;
                ForEachChildRecursive(go, action);
                action(go);
            }
        }


        /// <summary>
        /// Returns true if TComponent was found and not null.
        /// </summary>
        /// <returns>True if Component was found and is not null.</returns>
        public static bool TryGetComponent<TComponent>(this GameObject gameObject,
            out TComponent component)
        {
            component = gameObject.GetComponent<TComponent>();
            return component != null;
        }

        /// <summary>
        /// Get a new List each TComponent that is on each the root of each GameObject.
        /// </summary>
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

        /// <summary>
        /// Set this and all children (and on) to given layer.
        /// </summary>
        public static void SetLayerRecursively(this GameObject gameObj, int newLayer)
            => gameObj.transform.SetLayerRecursively(newLayer);

        /// <summary>
        /// If false, set self inactive. If true, walk hierarchy upwards,
        /// setting all parents active.
        /// </summary>
        public static void SetActiveInHierarchy(this GameObject gameObject, bool active)
        {
            if (active)
            {
                // walk parent upwards
                var parent = gameObject.GetComponent<Transform>().parent;
                if (parent)
                    SetActiveInHierarchy(parent.gameObject, active);
            }
            gameObject.SetActiveChecked(active);
        }
    }
}
