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
    public bool isLevelActivateButton;
    public bool isNextSelectButton;
    public bool isPrevSelectButton;
    public LevelSelect levelSelector;
    public string activationString = "";

    /* --- Internal Variables --- */

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    /* --- Methods --- */
    public void Activate()
    {
        if (isLevelActivateButton)
        {
            if (activationString != "")
            {
                SceneManager.LoadScene(activationString, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(levelSelector.selectedLevel, LoadSceneMode.Single);
            }
            //print(levelSelector.selectedLevel);
        }

        if (isNextSelectButton) 
        {
            levelSelector.SelectLevel(levelSelector.levelIndex + 1);
        }

        if (isPrevSelectButton)
        {
            levelSelector.SelectLevel(levelSelector.levelIndex - 1);
        }
    }
}
