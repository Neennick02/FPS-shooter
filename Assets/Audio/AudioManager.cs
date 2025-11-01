using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public int poolSize;
    private List<AudioSource> pool = new List<AudioSource>();

    public static AudioManager Instance;
    private void Awake()
    {
        //make a singleton
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        //add audio component
        for(int i = 0; i < poolSize; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            pool.Add(source);
        }
    }

    public void PlayClip(AudioClip clip, float volume = 1f)
    {
        var source = GetAwailableSource();
        if(source == null) return;

        source.clip = clip;
        source.volume = volume;
        source.Play();
        StartCoroutine(ReleaseWhenDone(source));
    }

    private AudioSource GetAwailableSource()
    {
        foreach(AudioSource source in pool)
        {
            if(!source.isPlaying) return source;
        }
        return null;
    }

    IEnumerator ReleaseWhenDone(AudioSource source)
    {
        while(source.isPlaying)
            yield return null;
        source.clip = null;
    }
}
