using UnityEngine;

public class GunScript : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;
    [SerializeField] float fireRate = 10f;
    [SerializeField] Camera playerCam;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject impactEffect;
    [SerializeField] Recoil_Sway_ADS recoilScript;

    [SerializeField] float fireInterval = .5f;
    void Start()
    {
        
    }

    void Update()
    {
        //fire bullet
        if (Input.GetMouseButtonDown(0) && Time.time >= fireInterval)
        {
            fireInterval = Time.time + 1f / fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if(muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        recoilScript.ApplyRecoil();

        RaycastHit hit;
        //send raycast
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, range))
        {
            //find target health
            Health targetHealth = hit.transform.GetComponent<Health>();

            if(targetHealth != null)
            {
                targetHealth.Sethealth(damage);
            }

            //add impactEffect
            if(impactEffect != null)
            {
                GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGo, 2f);
            }
        }
    }
}
