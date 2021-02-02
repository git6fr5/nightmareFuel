using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTimer : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {HUDTimer}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public Text timerText;

    /* --- Internal Variables --- */
    private float time = 0f;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    void Update()
    {
        if (time < 10) 
        {
            timerText.text = "00" + Mathf.Floor(time).ToString();
        }
        else if (time < 100)
        {
            timerText.text = "0" + Mathf.Floor(time).ToString();
        }
        else if (time < 1000)
        {
            timerText.text = Mathf.Floor(time).ToString();
        }
        else if (time >= 1000)
        {
            timerText.text = "999";
        }
    }

    void FixedUpdate()
    {
        time = time + Time.fixedDeltaTime;
    }


}
