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

    [Header("Damage overlay, player only")]
    [SerializeField] Image overlay;
    public float duration;
    public float fadeSpeed;

    float durationTimer;
    void Start()
    {
        health = maxHealth;
        if(overlay != null)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        }
    }

    void Update()
    {
        health = math.clamp(health, 0, maxHealth);

        if(health <= 0)
        {
            Die();
        }

        //checks if player overlay needs to be shown
        if (overlay != null)
        {
            if (overlay.color.a > 0)
                //checks if player health is low
                if (health < maxHealth / 3)
                {
                    return;
                }

            //applies fade effect
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                //fade the image
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;

                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

   public void TakeDamage(int amount)
    {
        health -= amount;
        UpdateHealthBar();
        if (overlay != null)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        }
        durationTimer = 0;
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
