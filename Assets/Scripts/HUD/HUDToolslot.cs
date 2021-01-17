using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDToolslot : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Tool Slot}: ";
    private bool DEBUG_init = true;

    /* --- Components --- */
    public Image toolIcon;
    public ToolTip toolTip;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

}
