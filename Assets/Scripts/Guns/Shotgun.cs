using UnityEngine;
using System.Collections;

public class Shotgun : GunScript
{
    public int pelletsPerShot = 8;
    public float spreadAngle = 5;
    [SerializeField] Transform firePoint;
    [SerializeField] Color startC;
    [SerializeField] Color endC;
    [SerializeField] PoolManager palletManager;
    protected override void Shoot()
    {
        //change ammo amount
        ammoInChamber--;

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        recoilScript.RecoilFire(recoilUp, recoilSide);

        for(int i =0; i < pelletsPerShot; i++)
        {
            Vector3 dir = playerCam.transform.forward;
            dir = AddSpread(dir);

            Ray ray = new Ray(playerCam.transform.position, dir);
            Vector3 targetPos;

            //fire pallets
            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                targetPos = hit.point;






                //damage target
                Health targetHealth = hit.transform.gameObject.GetComponent<Health>();
                Rigidbody targetRigidbody = hit.rigidbody;
                
                if(targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }

                //send object flying
                if (targetRigidbody != null)
                {
                    targetRigidbody.AddForceAtPosition(ray.direction * force, hit.point, ForceMode.Impulse);
                }

                //impact effect
                if (impactEffect != null)
                {
                    GameObject impactGo = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(impactGo, 2);
                }
            }
            else
            {
                Debug.Log("out of range");
                //when the raycast hits nothing
                targetPos = firePoint.position + dir.normalized * range;
            }
            palletManager.DrawPelletTrail(firePoint.position, targetPos, startC, endC);


           



            //draws rays for trails
            Ray trailRay = new Ray(firePoint.position, firePoint.transform.forward);
            

        }
    }

    Vector3 AddSpread(Vector3 direction)
    {
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);

        Quaternion spreadRotation = Quaternion.Euler(spreadY, spreadX, 0);
        return spreadRotation * direction;
    }
}
