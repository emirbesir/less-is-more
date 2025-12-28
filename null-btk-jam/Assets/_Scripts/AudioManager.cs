using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> musicsToPlay;
    [SerializeField] private AudioSource musicSource;
    
    private int currentMusicIndex = 0;
    private bool isPlaying = false;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (musicsToPlay.Count > 0 && musicSource != null && !isPlaying)
        {
            isPlaying = true;
            PlayNextMusic();
        }
    }

    private void PlayNextMusic()
    {
        musicSource.clip = musicsToPlay[currentMusicIndex];
        musicSource.Play();
        currentMusicIndex = (currentMusicIndex + 1) % musicsToPlay.Count;
        Invoke(nameof(PlayNextMusic), musicSource.clip.length);
        Debug.Log("Playing music: " + musicSource.clip.name);
    }
}
