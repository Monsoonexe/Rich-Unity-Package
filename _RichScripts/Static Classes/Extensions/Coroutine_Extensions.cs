using UnityEngine;

public static class Coroutine_Extensions 
{
    /// <summary>
    /// Does null check as well.
    /// </summary>
    /// <param name="routine"></param>
    public static void StopCoroutine(this Coroutine routine, MonoBehaviour owner)
    {
        if (routine != null)
            owner.StopCoroutine(routine);
    }
}
