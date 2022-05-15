using UnityEngine;
using ScriptableObjectArchitecture;

namespace RichPackage.Audio
{
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
        private RichAudioClip collisionClip = new RichAudioClip();

        public void OnCollisionEnter(Collision collision)
        {
            var options = (AudioOptions)collisionClip.Options.Clone(); //override option's volume
            options.volume *= (collision.relativeVelocity.magnitude 
                * maxForceFactor); //volume relative to impact force
            AudioManager.PlaySFX(collisionClip, options);
        }
    }
}
