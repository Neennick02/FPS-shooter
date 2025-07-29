using UnityEngine;

public class GunScript : MonoBehaviour
{
    public bool fullAutoEnabled;
    public bool isReloading;

    [Header("Ammo config")]
    public int ammoInChamber;
    public int maxMagSize =5;
    public int magAmout = 5;
    public float reloadTime = 3f;
    float timer = 0;

    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;


    [SerializeField] float fireRate = .2f;
    float fullAutoTimer = 0;

    [SerializeField] Camera playerCam;
    [SerializeField] Recoil_Sway_ADS recoilScript;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject impactEffect;

    InputManager inputManager;
    PlayerUI UI;
    void Start()
    {
        ammoInChamber = maxMagSize;
        UI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        //full auto
        if (fullAutoEnabled)
        {
            fullAutoTimer += Time.deltaTime;

            if(inputManager.onFoot.Shoot.IsPressed() && fullAutoTimer > fireRate && ammoInChamber > 0)
            {
                Shoot();
                fullAutoTimer = 0f;
            }
        }
        //semi auto
        else
        {
            if (inputManager.onFoot.Shoot.triggered && Time.time >= fireRate && ammoInChamber > 0)
            {
                
                Shoot();
            }
        }


        Reload();
    }

    void Shoot()
    {
        //change ammo amount
        ammoInChamber--;

        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        recoilScript.RecoilFire();

        RaycastHit hit;
        //send raycast
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range))
        {
            //find target health
            Health targetHealth = hit.transform.GetComponent<Health>();

            if(targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }

            //add impactEffect
            if(impactEffect != null)
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f);
            }
        }
    }

    public void Reload()
    {
        //reload when mag is empty
        if (ammoInChamber == 0 && magAmout > 0)
        {
            isReloading = true;
        }
        //you can only reload when mag is not full
        if (inputManager.onFoot.Reload.IsPressed() && ammoInChamber < maxMagSize && magAmout > 0)
        {
            isReloading = true;
        }


        //add time to timer
        if (isReloading)
        {
            timer += Time.deltaTime;
        }

        //fill mag when reloading is done
        if (timer > reloadTime)
        {
            magAmout--;
            ammoInChamber = maxMagSize;
            isReloading = false;
            timer = 0;
        }

        UI.UpdateAmmoCounter(ammoInChamber, magAmout);
        UI.ReloadBar(timer, reloadTime);


    }
}
