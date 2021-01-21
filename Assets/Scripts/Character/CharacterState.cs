using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterState : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {CharacterState}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public HUD hud;
    public Light2D vision;

    /* --- Info --- */
    public bool isClient;
    public Sprite portrait;

    /* --- Stats --- */
    public int maxHealth = 1;
    public int currHealth = 1;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        //if (isClient) { hud.gameObject.SetActive(true); hud.Inspect(this); }
        //if (isClient) { vision.gameObject.SetActive(true); }
    }

    void OnMouseDown()
    {
    }
}
