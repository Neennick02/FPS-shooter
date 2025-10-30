
public class Rifle : GunScript
{
    protected override void LateUpdate()
    {
        base.LateUpdate();
        if (_inputManager.onFoot.FullAutoonoff.triggered)
        {
            fullAutoEnabled = !fullAutoEnabled;
        }
    }
}
