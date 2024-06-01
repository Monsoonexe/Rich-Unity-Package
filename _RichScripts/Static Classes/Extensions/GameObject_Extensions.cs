using RichPackage.GuardClauses;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RichPackage
{
    public static class GameObject_Extensions
    {
        public static bool IsPrefab(this GameObject gameObject)
        {
            // a prefab has 0 roots if in project or 1 if it's in the prefab editor scene
            return gameObject.scene.rootCount <= 1;
        }

        public static GameObject GetChild(this GameObject gameObject, int i)
        {
            // isn't weird that the Transform component is the one in charge 
            // of the object graph? Sure, children are relative to their parents, but
            // is this not mostly a relation of game object less than a relation of orientation?
            return gameObject.transform.GetChild(i).gameObject;
        }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveToggle(this GameObject a)
            => a.SetActive(!a.activeSelf);

        /// <summary>
        /// Shortcut for Destroy(gameObject);
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Destroy(this GameObject a)
            => UnityEngine.Object.Destroy(a);

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/> if not not null,
        /// otherwise <see cref="GameObject.AddComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrAddComponent<T>(this GameObject gameObject,
            T comp = null)
            where T : Component
        {
            return comp != null || gameObject.TryGetComponent(out comp) ? comp : gameObject.AddComponent<T>();
        }

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
        /// a <see cref="Component"/> fetched with 
        /// <see cref="GameObject.GetComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetComponentIfNull<T>(this GameObject gameObject,
            T comp = null)
            where T : Component
        {
            return (comp != null) ? comp : gameObject.GetComponent<T>();
        }

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
        /// a <see cref="Component"/> fetched with <see cref="GameObject.GetComponent"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GetComponentIfNull<T>(this GameObject gameObject,
            ref T comp)
            where T : Component
        {
            if (comp == null)
                comp = gameObject.GetComponent<T>();
        }

        /// <returns><paramref name="comp"/> if <paramref name="comp"/> is not null, or
        /// a <see cref="Component"/> fetched with 
        /// <see cref="GameObject.GetComponent"/>.
        /// </returns>
        public static T GetComponentIfNull<T>(this GameObject gameObject,
            Maybe<T> maybeComponent)
            where T : Component
        {
            T comp;
            return ((comp = maybeComponent.GetValueOrDefault()) != null)
                ? comp : gameObject.GetComponent<T>();
        }

        /// <summary>
        /// Looks for a component of <typeparamref name="T"/> on self and any parent.
        /// </summary>
        public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T comp)
            where T : class
        {
            // linked-list traversal
            Transform current = gameObject.transform;

            do
            {
                if (current.TryGetComponent(out comp))
                {
                    return true;
                }

                current = current.parent;
            } while (current != null);

            return false;
        }

        /// <summary>
        /// Get name of the <see cref="GameObject"/> prefixed 
        /// with the current <see cref="UnityEngine.SceneManagement.Scene"/> name.<br/>
        /// e.g. "MainMenuScene/MainMenu".
        /// </summary>
        public static string GetNameWithScene(this GameObject a)
        {
            return UnityEngine.SceneManagement.SceneManager
                .GetActiveScene().name + "/" + a.name;
        }
        
        /// <summary>
        /// Builds a string that represents the <see cref="GameObject"/>'s hierarchy, like a folder path.
        /// </summary>
        /// <remarks>Not optimized.</remarks>
        public static string GetFullyQualifiedName(this GameObject a)
        {
            string name = a.name; // return value
            Transform parent = a.transform.parent;

            // walk hierarchy upwards towards scene root
            while (parent != null)
            {
                name = string.Concat(parent.name, "/", name);
                parent = parent.parent;
            }

            return name;
        }

        /// <summary>
        /// Perform <paramref name="action"/> on every Transform on each of its children.
        /// </summary>
        public static void ForEachChild(this GameObject obj,
            Action<Transform> action)
        {
            Transform transform = obj.transform;
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
            Transform transform = obj.transform;
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
            Transform transform = obj.transform;
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
            Transform transform = obj.transform;
            int childCount = transform.childCount;
            for (int i = childCount - 1; i >= 0; --i)
            {
                GameObject go = transform.GetChild(i).gameObject;
                ForEachChildRecursive(go, action);
                action(go);
            }
        }

        /// <summary>
        /// Returns the hierarchy path from the root to this object.
        /// </summary>
        public static string GetFullPath(this GameObject gameObject)
        {
            // TODO - optimize
            string path = gameObject.name;
            Transform parent = gameObject.transform.parent;
            while (parent != null)
            {
                path = $"{parent.name}/{path}";
                parent = parent.parent;
            }
            return path;
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
                Transform parent = gameObject.transform.parent;
                if (parent)
                    SetActiveInHierarchy(parent.gameObject, active);
            }

            gameObject.SetActiveChecked(active);
        }

        public static IEnumerable<TComp> EnumerateComponents<TComp>(
            this GameObject obj, bool recursive)
            where TComp : class
        {
            GuardAgainst.ArgumentIsNull(obj, nameof(obj));
            
            using (UnityEngine.Rendering.ListPool<TComp>.Get(out var comps))
            {
                obj.GetComponents(comps);
                foreach (TComp comp in comps)
                    yield return comp;
            }

            if (recursive)
            {
                foreach (Transform child in obj.transform)
                {
                    foreach (TComp comp in EnumerateComponents<TComp>(obj, recursive))
                        yield return comp;
                }
            }
        }
    }
}
