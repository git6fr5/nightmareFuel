using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {CharacterState}: ";
    private bool DEBUG_init = true;

    /*--- Components ---*/
    public HUD hud;

    /* --- Info --- */
    public Sprite portrait;

    /* --- Stats --- */
    public int maxHealth = 1;
    public int currHealth = 1;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
    }

    void OnMouseDown()
    {
    }
}
