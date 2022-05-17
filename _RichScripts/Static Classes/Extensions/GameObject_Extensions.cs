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
        /// <param name="a"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveTrue(this GameObject a)
            => a.SetActive(true);

        /// <summary>
        /// Shortcut for a.enabled = false;
        /// </summary>
        /// <param name="a"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetActiveFalse(this GameObject a)
            => a.SetActive(false);

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
