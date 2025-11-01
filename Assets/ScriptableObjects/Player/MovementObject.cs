using UnityEngine;

[CreateAssetMenu(fileName = "MovementObject", menuName = "Scriptable Objects/MovementObject")]
public class MovementObject : ScriptableObject
{
    public float JumpHeight;
    public float Gravity;
    [Header("Speed config")]
    public float Speed;
    public float SprintSpeed;
    public float CrouchSpeed;
    public float ClimbSpeed;
    public float SlideSpeed;
    [Header(" ")]

    public float SlideDuration;
    public float CrouchHeight;
    public float DefaultHeight;
}
