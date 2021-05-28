using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Mob
{

    /* --- Overridden Methods --- */
    public override float MoveFlag()
    {
        // Get a new direction for the character to move in
        float thinkInterval = idleMinInterval;
        if (characterState.stateDict[CharacterState.State.aggro])
        {
            characterMovement.horizontalMove = Random.Range(0, 4);
            characterMovement.verticalMove = Random.Range(0, 4);
            if (playerTransform.position.x < transform.position.x)
            {
                characterMovement.horizontalMove = -characterMovement.horizontalMove;
            }
            if (playerTransform.position.y < transform.position.y)
            {
                characterMovement.verticalMove = -characterMovement.verticalMove;
            }
            thinkInterval = Random.Range(aggroMinInterval, aggroMaxInterval);
        }
        else
        {
            characterMovement.horizontalMove = Random.Range(-3, 4);
            characterMovement.verticalMove = Random.Range(-3, 4);
            thinkInterval = Random.Range(idleMinInterval, idleMaxInterval);
        }
        return thinkInterval;
    }

}
