using UnityEngine;

public class GunScript : MonoBehaviour
{
    public bool fullAutoEnabled;

    [SerializeField] int damage = 10;
    [SerializeField] float range = 100f;


    [SerializeField] float fireRate = .2f;
    float fullAutoTimer = 0;

    [SerializeField] Camera playerCam;
    [SerializeField] Recoil_Sway_ADS recoilScript;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject impactEffect;

    [SerializeField] InputManager inputManager;

    void Start()
    {
        
    }

    void Update()
    {
        //full auto
        if (fullAutoEnabled)
        {
            fullAutoTimer += Time.deltaTime;

            if(inputManager.onFoot.Shoot.IsPressed() && fullAutoTimer > fireRate)
            {
                Shoot();
                fullAutoTimer = 0f;
            }
        }
        else
        {
            if (inputManager.onFoot.Shoot.triggered && Time.time >= fireRate)
            {
                
                Shoot();
            }
        }
        
    }

    void Shoot()
    {
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
}
