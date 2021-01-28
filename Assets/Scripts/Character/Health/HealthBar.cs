using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {HealthBar}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public Slider slider;
    public CharacterState characterState;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        slider.maxValue = characterState.maxHealth;
    }

    void Update()
    {
        transform.right = Vector3.right;
        slider.value = characterState.currHealth;
    }

    /* --- Methods ---*/
}
