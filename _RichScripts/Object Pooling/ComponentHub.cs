using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.ObjectPooling
{
    /// <summary>
    /// A locator for objects that add themselves to hub.
    /// </summary>
    public abstract class ADictionaryComponent<T> : RichMonoBehaviour
    {
        private readonly Dictionary<string, T>
            table = new Dictionary<string, T>(4); //lol. it's not what you think

        public T Get(string key)
        {
            if (!table.TryGetValue(key, out T value))
            {
                Debug.LogError($"[{nameof(ADictionaryComponent<T>)}] " +
					$"{key} not found.");
            }

            return value;
        }

        public U Get<U>(string key)
            where U : class
            => Get(key) as U;

        /// <summary>
        /// Keyed by name.
        /// </summary>
        public void Add(string key, T value)
            => table.Add(key, value);

        /// <summary>
        /// Keyed by name.
        /// </summary>
        public void Remove(string key)
            => table.Remove(key);
    }

    /// <summary>
    /// A locator for objects that add themselves to hub.
    /// </summary>
    public class ComponentHub : ADictionaryComponent<Component>
    {
        public void Add(Component comp)
            => Add(comp.name, comp);

        public void Remove(Component comp)
            => Remove(comp.name);
    }
}
