using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHealthbar : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Health Bar}: ";
    private bool DEBUG_init = true;

    /* --- Components --- */
    public Slider healthSlider;
    public Image fill;
    public Image borderImage;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }
}
