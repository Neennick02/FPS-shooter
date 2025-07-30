using UnityEngine;

public class Shotgun : GunScript
{
    public int pelletsPerShot = 8;
    public float spreadAngle = 5;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Color trailStartColor;
    [SerializeField] Color trailEndColor;


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
            //fire pallets
            if (Physics.Raycast(ray, out RaycastHit hit, range))
            {
                //draw bullet trails
                lineRenderer.enabled = true;
                lineRenderer.startColor = trailStartColor;
                lineRenderer.endColor = trailEndColor;

                lineRenderer.SetPosition(0, playerCam.transform.position);
                lineRenderer.SetPosition(1, hit.point);


                //damage target
                Health targetHealth = hit.transform.gameObject.GetComponent<Health>();
                Rigidbody targetRigidbody = hit.rigidbody;

                if(targetHealth != null)
                {
                    targetHealth.TakeDamage(damage);
                }

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
