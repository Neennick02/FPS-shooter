using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float damageAmount;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damageAmount);

        }
        Destroy(gameObject);
    }

    public void SetDamage(float amount)
    {
        damageAmount = amount;
    }
}
