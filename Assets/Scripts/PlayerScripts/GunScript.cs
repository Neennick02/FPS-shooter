using UnityEngine;
using System.Collections;
public abstract class GunScript : MonoBehaviour
{
    [SerializeField] protected GunObject _gunObject;
    [SerializeField] protected Settings _playerSettings;

    private float _fireRateTimer = 0;

    [Header("Ammo config")]
    [SerializeField] protected bool fullAutoEnabled = false;
    public bool isReloading;
    public bool isAiming;
    protected int ammoInChamber;
 
    float timer = 0;

    [Header("Effects config")]
    [SerializeField] protected Recoil_Sway_ADS recoilScript;
    protected Camera playerCam;
    [SerializeField] protected ParticleSystem muzzleFlash;
    [SerializeField] protected GameObject impactEffect;
    [SerializeField] protected GameObject bloodEffect;

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

    protected float zoomFOV;
    protected float normalFOV;
    Crosshair crossHairScript;
    protected InputManager _inputManager;
    protected virtual void Start()
    {
        crossHairScript = FindFirstObjectByType<Crosshair>();
        ammoInChamber = _gunObject.MaxMagSize;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        UI = player.GetComponent<PlayerUI>();
        playerCam = Camera.main;
        _inputManager = InputManager.Instance;

        normalFOV = _playerSettings.Fov;
    }

    protected virtual void LateUpdate()
    {
        SetFov();
        EnableDisableFullAuto();
        Reload();
        ChangeGrip();
    }

    protected virtual void SetFov()
    {
        normalFOV = _playerSettings.Fov;
        zoomFOV = _playerSettings.Fov / 1.5f;
    }

    void EnableDisableFullAuto()
    {
        if (fullAutoEnabled)
        {
            _fireRateTimer += Time.deltaTime;

            if (_inputManager.onFoot.Shoot.IsPressed() && _fireRateTimer > _gunObject.FireRate && ammoInChamber > 0 && !isReloading)
            {
                Attack();
                _fireRateTimer = 0f;
            }
        }
        //semi auto
        else
        {
            _fireRateTimer += Time.deltaTime;
            if (_inputManager.onFoot.Shoot.triggered && _fireRateTimer >= _gunObject.FireRate && ammoInChamber > 0 && !isReloading)
            {
                Attack();
                _fireRateTimer = 0;
            }
        }

        if (_inputManager.onFoot.FullAutoonoff.IsPressed())
        {
            fullAutoEnabled = !fullAutoEnabled;
        }
    }

    protected virtual void Attack()
    {
        //change ammo amount
        ammoInChamber--;
        _gunObject.Range = Random.Range(_gunObject.Range - _gunObject.RangeOffSet, _gunObject.Range + _gunObject.RangeOffSet);

        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        Physics.SyncTransforms();

        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        //make array of hits
        RaycastHit[] hits = Physics.RaycastAll(ray, _gunObject.Range);

        //sort array
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        //send raycast
        foreach (RaycastHit hit in hits)
        {
            
            if (hit.collider.CompareTag("Enemy"))
            {
                FindTargetHealth(hit);
            }
            
            FindTargetRB(hit, ray);
            AddImpact(hit);
        }

        //add recoil
        recoilScript.RecoilFire(_gunObject.RecoilUp, _gunObject.RecoilSide / 2);
    }

    protected void FindTargetHealth(RaycastHit hit)
    {
        //find health component in parent
        Health targetHealth = hit.transform.GetComponentInParent<Health>();
        Debug.Log(hit.collider.name);
        if(targetHealth != null)
        {
            float finalDamage = _gunObject.Damage;
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

    protected void FindTargetRB(RaycastHit hit, Ray ray)
    {
        Rigidbody targetRigidbody = hit.transform.GetComponent<Rigidbody>();

        if (targetRigidbody != null)
        {
            targetRigidbody.AddForceAtPosition(ray.direction * _gunObject.Force, hit.point, ForceMode.Impulse);
        }

    }

    protected void AddImpact(RaycastHit hit)
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
        if (_inputManager.onFoot.Aim.IsPressed() && !isReloading)
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
        if (ammoInChamber == 0 && _gunObject.MagAmount > 0)
        {
            isReloading = true;
        }
        //you can only reload when mag is not full
        if (_inputManager.onFoot.Reload.IsPressed() && ammoInChamber < _gunObject.MaxMagSize && _gunObject.MagAmount > 0)
        {
            isReloading = true;
        }


        //add time to timer
        if (isReloading)
        {
            timer += Time.deltaTime;
            playerCam.fieldOfView = Mathf.Lerp(playerCam.fieldOfView, normalFOV, Time.deltaTime * aimSpeed);

        }

        //fill mag when reloading is done
        if (timer > _gunObject.ReloadTime)
        {
            _gunObject.MagAmount--;
            ammoInChamber = _gunObject.MaxMagSize;
            isReloading = false;
            timer = 0;
        }

        UI.UpdateAmmoCounter(ammoInChamber, _gunObject.MagAmount);
        UI.ReloadBar(timer, _gunObject.ReloadTime);
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

    public void UpdateAmmo(int amount)
    {
        _gunObject.MagAmount = amount;
    }

/*    public virtual void SetFov(float fov)
    {
        normalFOV = fov;
        zoomFOV = normalFOV / 1.5f;
    }

    public void SetZoomFov(float division)
    {
        zoomFOV = normalFOV / division;
    }*/
}
