using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDPanel : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Status}: ";
    private bool DEBUG_init = true;

    /*--- Components ---*/
    public HUDHealthbar hudHealtbar;
    public HUDPortrait hudPortrait;
    public HUDToolslot hudToolslot;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }
}
