using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Do something when something enters this volume.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerVolume : ATriggerVolume
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(reactToTag))
        {
            enterEvent.Invoke();
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(reactToTag))
        {
            exitEvent.Invoke();
        }
    }

}
