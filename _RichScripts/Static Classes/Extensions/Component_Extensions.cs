using UnityEngine;

/// <seealso cref="Behaviour_Extensions"/>
public static class Component_Extensions
{
    //useful for delegates and events
    public static void DestroyGameObject(this Component c)
        => UnityEngine.Object.Destroy(c.gameObject);

    //useful for delegates and events
    public static void DestroyComponent(this Component c)
        => UnityEngine.Object.Destroy(c);

    public static void DontDestroyOnLoad(this Component c)
    {
        c.transform.SetParent(null);
        UnityEngine.Component.DontDestroyOnLoad(c.gameObject);
    }

    //useful for delegates and events
    public static void SetActiveTrue(this Component a) 
        => a.gameObject.SetActive(true);

    //useful for delegates and events
    public static void SetActiveFalse(this Component a) 
        => a.gameObject.SetActive(false);

    //useful for delegates and events
    public static void SetActive(this Component a, bool active) 
        => a.gameObject.SetActive(active);
}
