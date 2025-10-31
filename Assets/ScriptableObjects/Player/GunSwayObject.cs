using UnityEngine;

[CreateAssetMenu(fileName = "GunSwayObject", menuName = "Scriptable Objects/GunSwayObject")]
public class GunSwayObject : ScriptableObject
{
    [Header("Sway Config")]
    public float SwayAmount;
    public float SwaySmooth;

    [Header("Movement Sway Config")]
    [Header("")]
    public float WalkSwayAmount;
    public float RunSwayAmount;
    [Header("")]
    public float WalkSwaySpeed;
    public float RunSwaySpeed;

    [Header("Camera Bob Config")]
    [Header("")]
    public float WalkBobAmount;
    public float RunBobAmount;

    [Header("")]
    public float WalkBobSpeed;
    public float RunBobSpeed;

    [Header("")]
    public float DefaultBobAmount;
    public float DefaulkBobSpeed;

    [Header("Recoil Config")]
    [Header("")]
    public float RecoilKickback;
    public float ReturnSpeed;
}
