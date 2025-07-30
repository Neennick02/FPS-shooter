using UnityEngine;

public class Rifle : GunScript
{
    protected override void Update()
    {
        base.Update();
        if (inputManager.onFoot.FullAutoonoff.triggered)
        {
            fullAutoEnabled = !fullAutoEnabled;
        }
    }
}
