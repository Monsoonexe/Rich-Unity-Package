using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A locator for objects that add themselves to hub.
/// </summary>
public class ComponentHub<T> : RichMonoBehaviour
    where T : Component
{
    private readonly Dictionary<string, T>
        componentTable = new Dictionary<string, T>(); //lol. it's not what you think

    public T Get(string key)
    {
        T component = null;

        if (!componentTable.TryGetValue(key, out component))
        {
            Debug.LogError("[ComponentHub] Component <" + key + "> not found." +
                " Add this to the hub prior to access.");
        }

        return component;
    }

    public U Get<U>(string key) 
        where U : Component
        => Get(key) as U;

    /// <param name="newComponent"></param>
    public void Add(string key, T newComponent)
        => componentTable.Add(key, newComponent);

    /// <summary>
    /// Keyed by name.
    /// </summary>
    public void Add(T newComponent)
        => componentTable.Add(newComponent.name, newComponent);

    /// <summary>
    /// Keyed by name.
    /// </summary>
    public void Remove(T newComponent)
        => componentTable.Remove(newComponent.name);

    public void Remove(string key)
        => componentTable.Remove(key);
}

/// <summary>
/// A locator for objects that add themselves to hub.
/// </summary>
public class ComponentHub : ComponentHub<Component>
{

}
