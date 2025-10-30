using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Image HealthBar;
    public Image ShieldBar;

    [SerializeField] private Image _overlay;

    private bool _damageOverlay = false;
    private float _health;
    public float MaxHealth { get; private set; }
    bool _isDead = false;

    private float _shieldAmount;
    bool _hasShield;
    private float _durationTimer;

    private Coroutine _regenCoroutine;
    private Coroutine _healCoroutine;
    [SerializeField] HealthObject healthSO;

    private float rest = 0;
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
        if (_regenCoroutine == null && _health < MaxHealth / 2 && !_isDead)
            _regenCoroutine = StartCoroutine(RegenerateHealthOverTime(1.5f));


    }

    public void TakeDamage(int amount)
    {
        if (_isDead) return;

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
                if (_healCoroutine != null) _healCoroutine = null;
                _healCoroutine = StartCoroutine(SetHealthCoroutine(-rest));
            }
            //only take shield damage
            _shieldAmount = Mathf.Clamp(_shieldAmount, 0, MaxHealth);
            _shieldAmount -= amount;
        }
        else
        {
            if (_healCoroutine != null) _healCoroutine = null;
            _healCoroutine = StartCoroutine(SetHealthCoroutine(-amount));
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
        _shieldAmount += size + _shieldAmount;
        UpdateShieldBar();
    }

    public void Heal(float amount)
    {
        if (_isDead) return;
        if (_healCoroutine != null)
        {
            amount += rest;
            _healCoroutine = null;
        }

       _healCoroutine = StartCoroutine(SetHealthCoroutine(amount));
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

    IEnumerator RegenerateHealthOverTime(float delay)
    {
        float half = healthSO.MaxHealth / 2;
       
        float targetHealth = math.min(_health + healthSO.RegenrationStep, healthSO.MaxHealth);

        yield return new WaitForSeconds(delay); //heal delay


        while(_health < targetHealth)
        {
            //add health
            _health += 0.5f;
            UpdateHealthBar();

            // dont heal if health is over 50 %
            if (_health >= half)
            {
                _regenCoroutine = null;
                yield break;
            }
            yield return new WaitForSeconds(0.03f);
        }
        _regenCoroutine = null;
    }

    IEnumerator SetHealthCoroutine(float amount)
    {
        if (_isDead)
        {
            _healCoroutine = null;
            yield break;
        }

        float targetHealth = math.clamp(_health + amount, 0, MaxHealth);
        bool healthUp = targetHealth > _health ? true : false;


        while (Mathf.Abs(_health - targetHealth) > 0.01f)
        {
            rest = targetHealth - _health;

            if (healthUp) _health += 0.5f;
            else _health -= 0.5f;

                UpdateHealthBar();
            yield return new WaitForSeconds(0.005f);
        }
        _healCoroutine = null;
    }
}

