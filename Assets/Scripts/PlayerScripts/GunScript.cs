using UnityEngine;
using System.Collections;
public abstract class GunScript : MonoBehaviour
{
    [Header("Shooting config")]
    [SerializeField] protected int damage = 10;
    [SerializeField] protected float force = 50;
    [SerializeField] protected float range = 100f;
    [SerializeField] protected float rangeOffSet = 5;
    [SerializeField] protected float fireRate = .2f;
    float fireRateTimer = 0;

    [SerializeField] protected float recoilUp, recoilSide;

    [Header("Ammo config")]
    [SerializeField] protected bool fullAutoEnabled;
    public bool isReloading;
    public bool isAiming;
    [SerializeField] protected int ammoInChamber;
    [SerializeField] protected int maxMagSize= 10;
    [SerializeField] protected int magAmount = 5;
    [SerializeField] protected float reloadTime;
    float timer = 0;

    [Header("Effects config")]
    [SerializeField] protected Recoil_Sway_ADS recoilScript;
    protected Camera playerCam;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject impactEffect;

    protected InputManager inputManager;
    protected  PlayerUI UI;
    [Header("Aiming config")]
    [SerializeField] protected float aimSpeed = 8f;
    [Header("Hip config")]
    [SerializeField] protected Vector3 hipPos;
    [SerializeField] protected Vector3 hipRot;

    [Header("ADS config")]
    [SerializeField] protected Vector3 ADSPos;
    [SerializeField] protected Vector3 ADSRot;

    protected float zoomFOV = 40;
    protected float normalFOV = 60;
    Crosshair crossHairScript;
    protected virtual void Start()
    {
        crossHairScript = FindFirstObjectByType<Crosshair>();
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
            fireRateTimer += Time.deltaTime;

            if(inputManager.onFoot.Shoot.IsPressed() && fireRateTimer > fireRate && ammoInChamber > 0 && !isReloading)
            {
                Attack();
                fireRateTimer = 0f;
            }
        }
        //semi auto
        else
        {
            fireRateTimer += Time.deltaTime;
            if (inputManager.onFoot.Shoot.triggered && fireRateTimer >= fireRate && ammoInChamber > 0 && !isReloading)
            {
                Attack();
                fireRateTimer = 0;
            }
        }

        if (inputManager.onFoot.FullAutoonoff.IsPressed())
        {
            fullAutoEnabled = !fullAutoEnabled;
        }


        Reload();
        ChangeGrip();
    }

    protected virtual void Attack()
    {
        //change ammo amount
        ammoInChamber--;
        range = Random.Range(range - rangeOffSet, range + rangeOffSet);

        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        recoilScript.RecoilFire(recoilUp ,recoilSide);

        RaycastHit hit;
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        //send raycast
        if(Physics.Raycast(ray, out hit, range))
        {
            //find target health
            Health targetHealth = hit.transform.GetComponent<Health>();
            Rigidbody targetRigidbody = hit.transform.GetComponent<Rigidbody>();

            //check if hitpoint has health
            if(targetHealth != null)
            {
                //damage
                targetHealth.TakeDamage(damage);
                //hit marker
                UI.ShowHitMarker(.5f);
            }

            if(targetRigidbody != null)
            {
                targetRigidbody.AddForceAtPosition(ray.direction * force, hit.point, ForceMode.Impulse);
            }

            //add impactEffect
            if(impactEffect != null)
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f);
            }
        }
    }

    void ChangeGrip()
    {
        //can only ADS when not reloading
        if (inputManager.onFoot.Aim.IsPressed() && !isReloading)
        {
            isAiming = true;
            crossHairScript.SetCrossHairSize(50);
        }
        else
        {
            isAiming = false;
            crossHairScript.SetCrossHairSize(100);
        }
        Aim();
    }

    protected virtual void Aim()
    {
        //target pos / rotations
        Vector3 targetPos = isAiming ? ADSPos : hipPos;
        Quaternion targetRot = Quaternion.Euler(isAiming ? ADSRot : hipRot);

        //move between points
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * aimSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * aimSpeed);

        //change FOV
        float targetFOV = isAiming ? zoomFOV : normalFOV;
        playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, targetFOV, Time.deltaTime * aimSpeed);
    }


    protected void Reload()
    {
        //reload when mag is empty
        if (ammoInChamber == 0 && magAmount > 0)
        {
            isReloading = true;
        }
        //you can only reload when mag is not full
        if (inputManager.onFoot.Reload.IsPressed() && ammoInChamber < maxMagSize && magAmount > 0)
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
            magAmount--;
            ammoInChamber = maxMagSize;
            isReloading = false;
            timer = 0;
        }

        UI.UpdateAmmoCounter(ammoInChamber, magAmount);
        UI.ReloadBar(timer, reloadTime);
    }

    public void ResetGunPos()
    {
        transform.localPosition = hipPos;
        transform.localRotation = Quaternion.Euler( hipRot);
    }
}
