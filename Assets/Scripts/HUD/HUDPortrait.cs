using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPortrait : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Character Portrait}: ";
    private bool DEBUG_init = true;

    /* --- Components --- */
    public Image portraitImage;
    public Text characterName;
    public Image borderImage;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }
}
