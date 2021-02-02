using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Particle}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public Skeleton skeleton;
    public AnimationClip clip;
    public SpriteRenderer spriteRenderer;

    /* --- Internal Variables --- */
    public float length = 0f;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        length = clip.length;
    }

    /*--- Methods ---*/
    public void Activate(bool activate)
    {
        //if (transform.parent != null) { spriteRenderer.sortingLayer = 
        gameObject.SetActive(activate);
    }

    public IEnumerator TimedDeactivate(float delay)
    {
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(false);

        yield return null;
    }

    public void ActivateForDuration(float duration)
    {
        gameObject.SetActive(true);
        StartCoroutine(TimedDeactivate(duration));
    }

    public void Fire()
    {
        ActivateForDuration(length);
    }
}
