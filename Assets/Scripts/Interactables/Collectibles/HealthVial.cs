using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVial : Collectible
{

    /* --- Internal Variables --- */
    public float healthVialValue = 0.2f;


    /* --- Overridden Methods --- */
    public override void Effect(CharacterState characterState)
    {
        characterState.currHealth = characterState.currHealth + healthVialValue;
        if (characterState.currHealth > characterState.maxHealth)
        {
            characterState.currHealth = characterState.maxHealth;
        }
    }
}
