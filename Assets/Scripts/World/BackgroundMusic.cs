using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    /*private float duration;
    private float elapsedDuration = 0f;*/

    public int tempo = 120;
    [Range(0, 1)] public float maxVolume = 1f;
    private float volume = 0f;
    private float volumeIncrement;

    public bool intro = true;
    public bool intense = false;

    /*--- Audio ---*/

    public AudioClip mainMusic;
    public AudioClip introMusic;
    public AudioClip intenseMusic;

    public AudioSource audioSource;


    /* --- Unity Methods --- */
    void Start()
    {
        audioSource.volume = volume;
        PlaySound();
        intro = false;

        volumeIncrement = maxVolume / GameRules.gameDuration * Time.fixedDeltaTime;

    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlaySound();
        }
    }

    void FixedUpdate()
    {
        IncreaseIntensity();
    }

    /* --- Methods --- */

    void PlaySound()
    {
        bool sounded = false;

        /*--- High Priority ---*/
        if (intense && intenseMusic)
        {
            audioSource.clip = intenseMusic;
            audioSource.Play();
            sounded = true;
        }
        else if (intro && introMusic)
        {
            audioSource.clip = introMusic;
            audioSource.Play();
            sounded = true;
        }

        if (sounded) { return; }

        /*--- Low Priority ---*/

        audioSource.clip = mainMusic;
        audioSource.Play();
        return;
    }

    public void IncreaseIntensity()
    {
        volume = volume + volumeIncrement;
        if (volume > maxVolume)
        {
            volume = maxVolume;
        }
        audioSource.volume = volume;
    }

}
