using UnityEngine;
using ScriptableObjectArchitecture;

/// <summary>
/// Plays a sound when collided against.
/// </summary>
public class CollisionAudioResponse : RichMonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    private float maxForceFactor = 0.25f;

    [Header("---Audio---")]
    [SerializeField]
    private AudioClipReference collisionClip = new AudioClipReference();

    public void OnCollisionEnter(Collision collision)
    {
        var options = collisionClip.Options; //override option's volume
        options.volume *= (collision.relativeVelocity.magnitude 
            * maxForceFactor); //volume relative to impact force
        collisionClip.PlaySFX(options);
    }
}
