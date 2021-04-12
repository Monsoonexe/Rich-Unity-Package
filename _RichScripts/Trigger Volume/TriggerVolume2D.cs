using UnityEngine;

/// <summary>
/// Do something when something enters this volume.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class TriggerVolume2D : ATriggerVolume
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(reactToTag))
        {
            enterEvent.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(reactToTag))
        {
            exitEvent.Invoke();
        }
    }

}
