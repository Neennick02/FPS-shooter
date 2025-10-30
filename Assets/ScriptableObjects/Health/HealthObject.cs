using UnityEngine;

[CreateAssetMenu(fileName = "HealthObject", menuName = "Health System")]
public class HealthObject : ScriptableObject
{
    public int MaxHealth;

    public float HealthRegenInterval;
    public float RegenrationStep;
    public float OverlayFadeSpeed;
    public float FadeDuration;
}
