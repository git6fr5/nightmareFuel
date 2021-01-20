using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Particle}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public Skeleton skeleton;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    /*--- Methods ---*/
    public void Create()
    {
        gameObject.SetActive(true);
    }

    public void CreateForDuration(float duration)
    {
    }
}
