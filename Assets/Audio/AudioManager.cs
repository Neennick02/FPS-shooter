using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public List<SoundEffect> Sfxs = new List<SoundEffect>();
    public AudioSource AudioSource;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Play(string label)
    {
        //zoek naar soundeffect met label
        SoundEffect effect = Sfxs.Find(s => s.Label == label);
        //check of audioclip 
        if(effect != null)
        {
            if(effect.Effect != null)
            {
                AudioSource.PlayOneShot(effect.Effect);
            }
        }
    }
}
