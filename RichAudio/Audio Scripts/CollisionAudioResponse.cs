using UnityEngine;

namespace RichPackage.Audio
{
    /// <summary>
    /// Plays a sound when collided against.
    /// </summary>
    public class CollisionAudioResponse : RichMonoBehaviour
    {
        [Header("---Settings---")]
        [SerializeField]
        private float volumeModifyFactor = 0.25f;

        [Header("---Audio---")]
        [SerializeField]
        private RichAudioClipReference collisionClip = new RichAudioClipReference();

        public void OnCollisionEnter(Collision collision)
        {
            PlayCollisionAudio(collision);
        }

        // allows riggin in inspector with CollisionUnityEvent or something
        public void PlayCollisionAudio(Collision collision)
        {
            float modifiedVolume = collision.relativeVelocity.magnitude
                * volumeModifyFactor * collisionClip.Options.Volume;

            var options = collisionClip.Options;

            // prefer library code so it can be more easily changed
            AudioManager.Instance.PlayOneShot(collisionClip,
                options.Loop,
                options.PitchShift,
                options.Priority,
                modifiedVolume, // override volume
                options.Duration);
        }
    }
}
