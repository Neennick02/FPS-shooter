using UnityEngine;

[CreateAssetMenu(fileName = "EnemyObject", menuName = "Enemies")]
public class EnemyObject : ScriptableObject
{
    [Header("Health config")]
    public float MaxHealth;
    public float HealthRegenSpeed;

    [Header("Sight config")]
    public float SpottingDistance;
    public float HearingDistance;
    public float RotationSpeed;
    public float Fov;
    public float EyeHeight;

    [Header("Weapon config")]
    public float Damage;
    public float FireRate;
    public float BulletSpeed;
    public float ShootingAccuracy;
}
