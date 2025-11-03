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
    protected override void Attack()
    {
        //change ammo amount
        ammoInChamber--;
        _audioManager.PlayClip(_gunObject.FireSound, 1f);

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        Physics.SyncTransforms();

        for (int i =0; i < pelletsPerShot; i++)
        {
            Vector3 dir = playerCam.transform.forward;
            dir = AddSpread(dir);

            Ray ray = new Ray(playerCam.transform.position, dir);
            Vector3 targetPos;

            //fire pallets
            if (Physics.Raycast(ray, out RaycastHit hit, _gunObject.Range))
            {
                targetPos = hit.point;

                //damage target
                FindTargetHealth(hit);

                //send object flying
                FindTargetRB(hit, ray);

                //impact effect
                AddImpact(hit);
            }
            else
            {
                //when the raycast hits nothing
                targetPos = firePoint.position + dir.normalized * _gunObject.Range;
            }
            palletManager.DrawPelletTrail(firePoint.position, targetPos, startC, endC);

            //draws rays for trails
            Ray trailRay = new Ray(firePoint.position, firePoint.transform.forward);
            

        }
        recoilScript.RecoilFire(_gunObject.RecoilUp, _gunObject.RecoilSide);

    }

    Vector3 AddSpread(Vector3 direction)
    {
        float spreadX = Random.Range(-spreadAngle, spreadAngle);
        float spreadY = Random.Range(-spreadAngle, spreadAngle);

        Quaternion spreadRotation = Quaternion.Euler(spreadY, spreadX, 0);
        return spreadRotation * direction;
    }
}
