using UnityEngine;

/// <seealso cref="Behaviour_Extensions"/>
public static class Component_Extensions
{
    public static void DestroyGameObject(this Component c)
        => UnityEngine.Object.Destroy(c.gameObject);

    public static void DestroyComponent(this Component c)
        => UnityEngine.Object.Destroy(c);
}
