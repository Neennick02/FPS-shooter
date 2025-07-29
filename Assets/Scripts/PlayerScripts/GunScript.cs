using UnityEngine;
using System.Collections;
public abstract class GunScript : MonoBehaviour
{
    [Header("Shooting config")]
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float range = 100f;
    [SerializeField] protected float fireRate = .2f;
    float fullAutoTimer = 0;
    [SerializeField] protected float recoilUp, recoilSide;

    [Header("Ammo config")]
    public bool fullAutoEnabled;
    public bool isReloading;
    public int ammoInChamber;
    public int maxMagSize;
    public int magAmout;
    public float reloadTime;
    float timer = 0;

    [Header("Effects config")]
    [SerializeField] protected Recoil_Sway_ADS recoilScript;
    protected Camera playerCam;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject impactEffect;

    protected InputManager inputManager;
    protected  PlayerUI UI;
    protected virtual void Start()
    {
        ammoInChamber = maxMagSize;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        UI = player.GetComponent<PlayerUI>();
        playerCam = Camera.main;
        inputManager = player.GetComponent<InputManager>();
    }

    protected virtual void Update()
    {
        //full auto
        if (fullAutoEnabled)
        {
            fullAutoTimer += Time.deltaTime;

            if(inputManager.onFoot.Shoot.IsPressed() && fullAutoTimer > fireRate && ammoInChamber > 0 && !isReloading)
            {
                Shoot();
                fullAutoTimer = 0f;
            }
        }
        //semi auto
        else
        {
            if (inputManager.onFoot.Shoot.triggered && Time.time >= fireRate && ammoInChamber > 0 && !isReloading)
            {
                Shoot();
            }
        }

        if (inputManager.onFoot.FullAutoonoff.IsPressed())
        {
            fullAutoEnabled = !fullAutoEnabled;
        }


        Reload();
    }

    protected virtual void Shoot()
    {
        //change ammo amount
        ammoInChamber--;

        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        recoilScript.RecoilFire(recoilUp ,recoilSide);

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


    protected void Reload()
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
