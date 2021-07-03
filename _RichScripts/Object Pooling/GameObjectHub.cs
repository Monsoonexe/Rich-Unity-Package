using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class GameObjectHub : RichMonoBehaviour
{
    private readonly Dictionary<string, GameObject>
        poolTable = new Dictionary<string, GameObject>(); //lol. it's not what you think

    public GameObject Get(string key)
    {
        GameObject obj = null;

        if (!poolTable.TryGetValue(key, out obj))
        {
            Debug.LogError("[PoolHub] Pool <" + key + "> not found." +
                " Add this to the pool prior to access.");
        }

        return obj;
    }

    public U Get<U>(string key) 
        where U : Component
    {
        Component comp = null;

        var obj = Get(key);
        if (obj != null)
            comp = obj.GetComponent<U>();

        return comp as U;
    }

    public void Add(string key, GameObject newObj)
        => poolTable.Add(key, newObj);

    /// <summary>
    /// Keyed by name.
    /// </summary>
    public void Add(GameObject newObj)
        => poolTable.Add(newObj.name, newObj);

    /// <summary>
    /// Keyed by name.
    /// </summary>
    public void Remove(GameObject newObj)
        => poolTable.Remove(newObj.name);

    public void Remove(string key)
        => poolTable.Remove(key);
}
