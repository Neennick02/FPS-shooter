using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Scriptable Objects/Settings")]
public class Settings : ScriptableObject
{
    [Header("Gameplay")]
    public float Fov;
    public float Sensitivity;
    public bool Paused;
    [Header("Audio")]
    public float MainVolume;
    public float MusicVolume;
    public float SfxVolume;

    [Header("Display")]
    public int FrameRate;
    public enum Quality
    {
        Low,
        Mid,
        High
    }
    public Quality QualityState;
    public bool FullScreen;
    public bool Vsync;
    public float Brightness;
}
