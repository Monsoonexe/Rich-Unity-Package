using UnityEngine;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

[RequireComponent(typeof(Collider))]
public class Projectile : Poolable
{
    [Header("---Settings---")]
    [SerializeField]
    protected float forwardSpeed = 50;

    [SerializeField]
    protected float explosionLifetime = 1.0f;

    [SerializeField]
    protected int damage = 10;

    [Header("---Impact Stuff---")]

    [SerializeField]
    protected float impactForce = 5;

    [SerializeField]
    protected GameObject explosionEffect;

    [Required]
    [SerializeField]
    protected Transform impactPoint;

    [SerializeField]
    protected AudioClipReference impactSound;

    // Update is called once per frame
    private void Update()
    {
        myTransform.position += myTransform.forward 
            * (Time.deltaTime * forwardSpeed);
    }

    virtual protected void OnImpact()
    {
        impactSound.PlaySFX();
        if (explosionEffect)
        {
            var explosion = Instantiate(explosionEffect, 
                impactPoint.position, Quaternion.identity); // maybe at point of impact
            Destroy(explosion, explosionLifetime); //TODO how can I use a 'static' pool??
        }
        else
        {
            var impactMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            impactMarker.transform.position = impactPoint.position;
            impactMarker.name = "Impact Marker";
            Destroy(impactMarker, explosionLifetime); //TODO how can I use a 'static' pool??
        }

        Destroy(this.gameObject);
    }

    virtual protected void OnTriggerEnter(Collider col)
    {
        OnImpact();

        col.GetComponent<IDamageable>()
            ?.TakeDamage(damage);

        if (col.gameObject.CompareTag("Player"))
        {
            
            //col.attachedRigidbody.AddForce(myTransform.forward * impactForce, ForceMode.Impulse);
            //col.attachedRigidbody.AddTorque(myTransform.forward * impactForce, ForceMode.Impulse);
            //deal damage to player

            //apply impact force to player to knock them off course
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnImpact();
        collision.gameObject.GetComponent<IDamageable>()
            ?.TakeDamage(damage);
    }

}
