using UnityEngine;

/// <summary>
/// Do something when something enters this volume.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class TriggerVolume2D : ATriggerVolume
{
    protected void OnTriggerEnter2D(Collider2D collision)
        => HandleEnterCollision(collision.gameObject);

    protected void OnTriggerExit2D(Collider2D collision)
        => HandleExitCollision(collision.gameObject);
}
