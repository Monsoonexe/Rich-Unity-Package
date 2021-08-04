using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Extension methods for monobehaviours.
/// </summary>
/// <seealso cref="Component_Extensions"/>
/// <seealso cref="Behaviour_Extensions"/>
public static class MonoBehaviour_Extensions
{
    public static void InvokeAfterDelay(this MonoBehaviour mono,
        Action action, float delay)
        => mono.StartCoroutine(InvokeAfterDelay(action, delay));

    private static IEnumerator InvokeAfterDelay(
        Action action, float delay)
    {
        //TODO - used unmarshalled Time.
        var endTime = Time.time + delay;

        do yield return null;
        while (Time.time < endTime);
        action();
    }
}
