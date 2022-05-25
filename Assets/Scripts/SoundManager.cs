using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    AudioClip menuMusic;
    [SerializeField]
    AudioClip gameMusic;
    [SerializeField]
    AudioClip bossMusic;

    [SerializeField]
    AudioSource musicSource;
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
}
