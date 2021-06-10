using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound : MonoBehaviour
{
    public float volume = 1f;
    public AudioSource audioSource;
    public AudioClip audioClip;
    
    public void Play()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }

    public void PlayAdditively()
    {
        Sound addSound = Instantiate(gameObject).GetComponent<Sound>();
        addSound.PlayAndDestroy(1f);
    }

    public void PlayAndDestroy(float duration)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        StartCoroutine(IEDestroy(duration));
    }

    private IEnumerator IEDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);

        yield return null;
    }
}
