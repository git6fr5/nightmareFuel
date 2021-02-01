using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Button}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public bool isSceneButton;
    public string activationString;

    /* --- Internal Variables --- */

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    /* --- Methods --- */
    public void Activate()
    {
        if (isSceneButton) { SceneManager.LoadScene(activationString, LoadSceneMode.Single); }
    }
}
