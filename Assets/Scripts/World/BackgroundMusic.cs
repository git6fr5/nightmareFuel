using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {BackgroundMusic}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public AudioClip mainMusic;
    public AudioClip introMusic;
    public AudioClip intenseMusic;

    public AudioSource audioSource;

    /* --- Internal Variables --- */
    public int tempo = 120;
    [Range(0, 1)] public float maxVolume = 1f;
    private float volume = 0.5f;
    private float volumeIncrement;

    public bool intro = true;
    public bool intense = false;


    /* --- Unity Methods --- */
    void Start()
    {

        if (DEBUG_init) { print(DebugTag + "Activated"); }

        audioSource.volume = volume;
        PlaySound();
        intro = false;

        volumeIncrement = (maxVolume - volume) / GameRules.gameDuration * Time.fixedDeltaTime;

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
