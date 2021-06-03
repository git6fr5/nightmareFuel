using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonVial : Collectible
{

    /* --- Internal Variables --- */
    public float poisonVialValue = 3f;


    /* --- Overridden Methods --- */
    public override void Effect(CharacterState characterState)
    {
        PoisonCircle poisonCircle = GameObject.FindGameObjectsWithTag("Poison Circle")[0].GetComponent<PoisonCircle>();
        poisonCircle.circleCollider.radius = poisonCircle.circleCollider.radius + (poisonVialValue / Time.fixedDeltaTime) * poisonCircle.radiusIncrement;
        if (poisonCircle.circleCollider.radius > poisonCircle.initRadius)
        {
            poisonCircle.circleCollider.radius = poisonCircle.initRadius;
        }
    }
}
