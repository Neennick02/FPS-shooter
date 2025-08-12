using UnityEngine;

public class Rifle : GunScript
{
    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (inputManager.onFoot.FullAutoonoff.triggered)
        {
            fullAutoEnabled = !fullAutoEnabled;
        }
    }
}
