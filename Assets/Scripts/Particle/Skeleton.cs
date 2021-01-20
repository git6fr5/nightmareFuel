using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Skeleton}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public SkeletonBone root;
    public SkeletonBone head;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    /*--- Methods ---*/
    public static void Attach(SkeletonBone firstBone, SkeletonBone secondBone)
    {
        print("attaching");
        secondBone.transform.parent.parent.position = firstBone.transform.parent.parent.position;
    }
}
