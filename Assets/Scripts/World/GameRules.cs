using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRules : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {GameRules}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/

    /* --- Internal Variables --- */
    [HideInInspector] public static float gameDuration = 60f; // in seconds

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }
}
