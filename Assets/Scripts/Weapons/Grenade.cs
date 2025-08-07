using UnityEngine;
using System.Collections;
public class Grenade : ThrowAble
{
    [SerializeField] float explosionForce = 10f;
    [SerializeField] float upwardsModifier = 1.0f;
    protected override void Explode()
    {
        //explosion effect
        explosionPrefab = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        //explosion sound
        ///


        //find objects in radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider hit in colliders)
        {
            //get health component
            Health health = hit.gameObject.GetComponentInParent<Health>();

            if(health != null)
            {
                health.TakeDamage(health.maxHealth);
            }

            //add explosion force to rigidbodies
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, radius, upwardsModifier, ForceMode.Impulse);
            }
        }


    }
}
