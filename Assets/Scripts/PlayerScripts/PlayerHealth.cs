using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Image HealthBar;
    public Image ShieldBar;
    private GameObject _shieldBarParent;

    [SerializeField] private Image _overlay;

    private bool _damageOverlay = false;
    private float _health;
    public float MaxHealth { get; private set; }
    bool _isDead = false;

    private float _shieldAmount;
    bool _hasShield;
    private float _durationTimer;

    private Coroutine _regenCoroutine;

    [SerializeField] HealthObject healthSO;


    private void Awake()
    {
        MaxHealth = healthSO.MaxHealth;
    }
    void Start()
    {
        _health = healthSO.MaxHealth;
        UpdateShieldBar();
        UpdateHealthBar();
        if (_overlay != null) _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, 0);
    }

    void Update()
    {
        _health = math.clamp(_health, 0, MaxHealth);

        if (_shieldAmount <= 0)
        {
            _hasShield = false;
        }

        if (_health <= 0 && !_isDead)
        {
            Die();
            return;
        }
        else
        {
            FadeOutDamageOverlay();
        }

        UpdateShieldBar();
        if (_regenCoroutine == null) _regenCoroutine = StartCoroutine(RegenerateHealth(1.5f));
    }

    public void TakeDamage(int amount)
    {
        if(_regenCoroutine != null)
        {
            //stop regeneration
            StopCoroutine(_regenCoroutine);
            _regenCoroutine = null;
        }


        //check if player has shield
        if (_hasShield)
        {
            //check if damage is bigger than shield
            float rest = amount - _shieldAmount;
            if (rest > 0)
            {
                //take shield damage and health damage
                _shieldAmount = 0;
                _health -= rest;
            }
            //only take shield damage
            _shieldAmount = Mathf.Clamp(_shieldAmount, 0, MaxHealth);
            _shieldAmount -= amount;
        }
        else
        {
            _health -= amount;
        }
            UpdateHealthBar();
            UpdateShieldBar();

        //set damage overlay to true
        if (_overlay != null)
        {
            _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, 1);
            _damageOverlay = true;
        }
        _durationTimer = 0;
    }

    public void AddShield(float percentage)
    {
        _hasShield = true;

       // _shieldBarParent.SetActive(true);
        float size = MaxHealth *(percentage / 100f);
        _shieldAmount =+ size + _shieldAmount;
        UpdateShieldBar();
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
        if (HealthBar == null) return;

            float newWidth = _health / MaxHealth;
            HealthBar.fillAmount = newWidth;
    }

    void UpdateShieldBar()
    {
        if (ShieldBar == null) return;

        float newWidth = _shieldAmount / MaxHealth;
        ShieldBar.fillAmount = newWidth;
    }

    void FadeOutDamageOverlay()
    {
        if (_overlay == null) return;

        //check if overlay is active
        if (_overlay.color.a > 0)
            //checks if player health is low
            if (_health < healthSO.MaxHealth / 3)
            {
                return;
            }
        

   //applies fade effect
   _durationTimer += Time.deltaTime;
        if (_durationTimer > healthSO.FadeDuration)
        {
            //fade the image
            float tempAlpha = _overlay.color.a;
            tempAlpha -= Time.deltaTime * healthSO.OverlayFadeSpeed;

            _overlay.color = new Color(_overlay.color.r, _overlay.color.g, _overlay.color.b, tempAlpha);
            if (_overlay.color.a == 0) _damageOverlay = false;
        }
    }

    public virtual void Die()
    {
        _isDead = true;
            Debug.Log("Game over");
    }

    IEnumerator RegenerateHealth(float delay)
    {
        // dont heal if health is over 66 %
        float sixtyProcent = (healthSO.MaxHealth / 3) * 2;

        if (_health > sixtyProcent)
        {
            _regenCoroutine = null;
            yield break;
        }
        float regenerationSize = (healthSO.RegenrationStep / MaxHealth) * 100;
        float targetHealth = math.min(_health + regenerationSize, healthSO.MaxHealth);

        yield return new WaitForSeconds(delay); //heal delay


        while(_health < targetHealth)
        {
            _health++;
            UpdateHealthBar();
            if (_health >= sixtyProcent)
            {
                _regenCoroutine = null;
                yield break;
            }
            yield return new WaitForSeconds(0.05f);
        }
        _regenCoroutine = null;
    }
}

