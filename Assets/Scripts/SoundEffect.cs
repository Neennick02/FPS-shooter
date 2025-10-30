using UnityEngine;

[CreateAssetMenu(fileName = "SoundEffect", menuName = "Audio Manager/Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public string Label;
    public AudioClip Effect;

}
