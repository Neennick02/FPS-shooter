using System.Collections.Generic;
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

    [Header("Positions")]
    public Vector3 HipPos;
    public Vector3 HipRot;
    [Header("")]
    public Vector3 ADSpos;
    public Vector3 ADSrot;
    [Header("")]
    public Vector3 ReloadPos;
    public Vector3 ReloadRot;

    [Header("Sounds")]
    public List<AudioClip> FireSound = new List<AudioClip>();
    public List<AudioClip> ReloadSound = new List<AudioClip>();

}
