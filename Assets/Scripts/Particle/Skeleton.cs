using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Skeleton}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public Bone root;
    public Bone head;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }
}
