using UnityEngine;

public class Pistol : GunScript
{
    protected override void Update()
    {
        Reload();

        //disable full auto
        if (inputManager.onFoot.Shoot.triggered && Time.time >= fireRate && ammoInChamber > 0)
        {
            Shoot();
        }
    }
}
