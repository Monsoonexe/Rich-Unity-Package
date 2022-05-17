using UnityEngine;

namespace RichPackage.TriggerVolumes
{
    /// <summary>
    /// Do something when something enters this volume.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerVolume : ATriggerVolume
    {
        protected void OnTriggerEnter(Collider other)
            => HandleEnterCollision(other.gameObject);

        protected void OnTriggerExit(Collider other)
            => HandleExitCollision(other.gameObject);

        protected void OnCollisionEnter(Collision collision)
            => HandleEnterCollision(collision.gameObject);

        protected void OnCollisionExit(Collision collision)
            => HandleExitCollision(collision.gameObject);
    }
}
