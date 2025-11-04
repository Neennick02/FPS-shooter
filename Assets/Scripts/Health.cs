using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    [SerializeField] EnemyObject _enemyObject;
    [SerializeField] public Image healthBar;
    public GameObject canvas;
    private float _health;

    void Start()
    {
        _health = _enemyObject.MaxHealth;
    }

    void Update()
    {
        _health = math.clamp(_health, 0, _enemyObject.MaxHealth);

        if(_health <= 0)
        {
            Die();
        }
    }

   public void TakeDamage(float amount)
    {
        _health -= amount;
        UpdateHealthBar();
    }

    public void Heal(int amount)
    {
        _health += amount;
        UpdateHealthBar();
    }

    public float GetHealth()
    {
        return _health;
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float newWidth = (float)_health / _enemyObject.MaxHealth;
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
