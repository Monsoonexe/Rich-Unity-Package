using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Do something when something enters this volume.
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerVolume : RichMonoBehaviour
{
    [SerializeField]
    protected string reactToTag = "Player";

    [SerializeField]
    protected UnityEvent enterEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitEvent = new UnityEvent();

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
