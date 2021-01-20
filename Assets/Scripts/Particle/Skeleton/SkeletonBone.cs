using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBone : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {SkeletonBone}: ";
    private bool DEBUG_init = false;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }
}
