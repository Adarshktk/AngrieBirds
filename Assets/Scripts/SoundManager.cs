using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance == null)
        { 
            Instance = this;
        }
    }

    public void PlayClip(AudioClip clip, AudioSource source)
    {

        source.clip = clip;
        source.Play();
    }

    
}
