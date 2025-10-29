using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] public Image healthBar;
    public GameObject canvas;
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

   public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {
        health += amount;
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
    }

    public void DisableHealthBar()
    {
        canvas.SetActive(false);
    }
}
