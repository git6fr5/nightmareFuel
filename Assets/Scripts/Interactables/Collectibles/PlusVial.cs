using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlusVial : Collectible
{

    /* --- Internal Variables --- */
    public int plusVialValue = 3;


    /* --- Overridden Methods --- */
    public override void Effect(CharacterState characterState)
    {
        HUD hud = GameObject.FindGameObjectsWithTag("HUD")[0].GetComponent<HUD>();
        hud.hudScore.AddPoints(plusVialValue);
    }
}