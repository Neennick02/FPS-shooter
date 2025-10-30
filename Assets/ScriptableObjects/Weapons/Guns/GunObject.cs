using UnityEngine;

[CreateAssetMenu(fileName = "GunObject", menuName = "GunObject")]
public class GunObject : ScriptableObject
{
    [Header("Config")]
    public float Damage;
    public float Force;
    public float Range;
    public float RangeOffSet;
    public float FireRate;

    [Header("Recoil")]
    public float RecoilUp;
    public float RecoilSide;

    [Header("Aim")]
    public float AimSpeed;

    [Header("Ammo")]
    public int MaxMagSize;
    public int MagAmount;
    public float ReloadTime;
}
