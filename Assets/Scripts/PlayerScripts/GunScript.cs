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
    [SerializeField] protected GameObject bloodEffect;

    protected InputManager inputManager;
    protected  PlayerUI UI;
    [Header("Aiming config")]
    [SerializeField] protected float aimSpeed = 8f;
    [SerializeField] protected GameObject crossHair;
    [Header("Hip config")]
    [SerializeField] protected Vector3 hipPos;
    [SerializeField] protected Vector3 hipRot;

    [Header("ADS config")]
    [SerializeField] protected Vector3 ADSPos;
    [SerializeField] protected Vector3 ADSRot;

    [Header("Reload config")]
    [SerializeField] protected Vector3 aimPos;
    [SerializeField] protected Vector3 aimRot;

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
            FindTargetHealth(hit);
            FindTargetRB(hit, ray);
            AddImpact(hit);


            //move behint hit point
            Vector3 behindFirstHit = hit.point + ray.direction * .01f;

            //find distance to hit point
            float remainingDistance = range - Vector3.Distance(playerCam.transform.position, behindFirstHit);

            //shoot raycast to check if enemy is behind target
            if(Physics.Raycast(ray, out RaycastHit newHit, remainingDistance))
            {
                //make sure you dont hit the same object
                if(newHit.collider != hit.collider)
                {
                    FindTargetHealth(newHit);
                    FindTargetRB(newHit, ray);
                    AddImpact(newHit);
                }
            }
        }
    }

    void FindTargetHealth(RaycastHit hit)
    {
        //find health component in parent
        Health targetHealth = hit.transform.GetComponentInParent<Health>();
        Debug.Log(hit.collider.name);
        if(targetHealth != null)
        {
            int finalDamage = damage;
            if (hit.collider.CompareTag("Head"))
            {
                finalDamage *= 3;
                Debug.Log("headshot");
            }
                //damage
                targetHealth.TakeDamage(finalDamage);
                //hit marker
                UI.ShowHitMarker(.5f);
        }
    }

    void FindTargetRB(RaycastHit hit, Ray ray)
    {
        Rigidbody targetRigidbody = hit.transform.GetComponent<Rigidbody>();

        if (targetRigidbody != null)
        {
            targetRigidbody.AddForceAtPosition(ray.direction * force, hit.point, ForceMode.Impulse);
        }

    }

    void AddImpact(RaycastHit hit)
    {
        //add impactEffect
        if (impactEffect != null)
        {
            if (hit.transform.CompareTag("Terrain"))
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f);
            }
            else if(hit.transform.CompareTag("Enemy"))
            {
                GameObject impactGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                impactGo.transform.SetParent(hit.transform);
                Destroy(impactGo, 10f);
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
        if (!isReloading)
        {
            Aim();
        }
        else
        {
            MoveToReloadPos();
        }
        
    }

    protected virtual void Aim()
    {
        //target pos / rotations
        Vector3 targetPos = isAiming ? ADSPos : hipPos;
        Quaternion targetRot = Quaternion.Euler(isAiming ? ADSRot : hipRot);

        //move between points
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * aimSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRot, Time.deltaTime * aimSpeed);

        //dis/enable crosshair
        if (Vector3.Distance(transform.localPosition, ADSPos) < 0.05f)
        {
            crossHair.SetActive(false);
        }
        else
        {
            crossHair.SetActive(true);
        }
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

    public void MoveToReloadPos()
    {
        //move between points
        transform.localPosition = Vector3.Lerp(transform.localPosition, aimPos, Time.deltaTime * aimSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(aimRot), Time.deltaTime * aimSpeed);
    }

    public void ResetGunPos()
    {
        transform.localPosition = hipPos;
        transform.localRotation = Quaternion.Euler( hipRot);
    }
}
