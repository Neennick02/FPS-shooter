using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public Image healthBar;
    [SerializeField] Image overlay;

    [SerializeField] int health;
    [SerializeField] public int maxHealth;

    [SerializeField] float healthDelay = 1;
    public float duration;
    public float fadeSpeed;

    float durationTimer;

    Coroutine regenCoroutine;
    void Start()
    {
        health = maxHealth;
        if (overlay != null)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        }
    }

    void Update()
    {
        health = math.clamp(health, 0, maxHealth);

        if (health <= 0)
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
        if(regenCoroutine != null)
        {
            StopCoroutine(regenCoroutine);
            regenCoroutine = null;
        }

        health -= amount;
        UpdateHealthBar();
        if (overlay != null)
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        }
        durationTimer = 0;

        regenCoroutine = StartCoroutine(RegenerateHealth(healthDelay));
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
        if (healthBar == null) return;

            float newWidth = (float)health / maxHealth;
            healthBar.fillAmount = newWidth;
    }



    public virtual void Die()
    {
            Debug.Log("Game over");
    }

    IEnumerator RegenerateHealth(float delay)
    {
        // dont heal if health is over 66 %
        if (health > (maxHealth / 3) * 2)
        {
            regenCoroutine = null;
            yield break;
        }

        float targetHealth = math.min(health + maxHealth / 4, maxHealth);

        yield return new WaitForSeconds(delay); //heal delay


        while(health < targetHealth)
        {
            health++;
            UpdateHealthBar();
            yield return new WaitForSeconds(0.05f);
        }
        regenCoroutine = null;
    }
}

