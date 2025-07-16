using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] int health;
    [SerializeField] public int maxHealth;
    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        health = math.clamp(health, 0, maxHealth);

        if(health <= 0)
        {
            Die();
        }
    }

   public void Sethealth(int amount)
    {
        health -= amount;
        UpdateHealthBar();

    }

    public int GetHealth()
    {
        return health;
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float newWidth = (float)health / maxHealth;
            healthBar.fillAmount = newWidth;
        }
    }

    public virtual void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Game over");
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
