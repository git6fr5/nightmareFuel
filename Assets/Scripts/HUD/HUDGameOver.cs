using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDGameOver : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {HUDGameOver}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public Text scoreText;
    public Text timerText;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        scoreText.text = "Score: " + timerText.text;
    }
}
