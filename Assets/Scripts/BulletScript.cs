using UnityEngine;

public class BulletScript : MonoBehaviour
{
    /*Rigidbody rb;
    [SerializeField] float speed = 50f;
    public int damageAmount = 10;
    void Start()
    {
       rb = GetComponent<Rigidbody>(); 
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    void Update()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            Health otherHealh = other.GetComponent<Health>();
            if(otherHealh != null)
            {
                otherHealh.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }

    }*/

    private void OnCollisionEnter(Collision collision)
    {
        Transform hitTransform = collision.transform;

        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            hitTransform.GetComponent<Health>().TakeDamage(10);

        }
        Destroy(gameObject);
    }
}
