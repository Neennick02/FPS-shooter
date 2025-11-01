using UnityEngine;

[CreateAssetMenu(fileName = "MovementObject", menuName = "Scriptable Objects/MovementObject")]
public class MovementObject : ScriptableObject
{
    public float JumpHeight;
    public float Speed;
    public float SprintSpeed; 
    public float Gravity;
    public float ClimbSpeed;
    public float CrouchHeight;
    public float DefaultHeight;
}
