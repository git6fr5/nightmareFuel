using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPanel : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Status}: ";
    private bool DEBUG_init = true;

    /*--- Components ---*/
    public CharacterState cState;
    public HealthBar healthBar;
    public CharacterPortrait characterPortrait;
    public ToolSlot toolSlot;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }
}
