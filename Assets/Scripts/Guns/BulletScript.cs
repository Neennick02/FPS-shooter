using UnityEngine;

public class BulletScript : MonoBehaviour
{
    int damageAmount;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            collision.gameObject.GetComponent<Health>().TakeDamage(damageAmount);

        }
        Destroy(gameObject);
    }

    public void SetDamage(int amount)
    {
        damageAmount = amount;
    }
}
