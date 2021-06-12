using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Mob
{

    /* --- Overridden Methods --- */
    public override float MoveFlag()
    {
        // Get a new direction for the character to move in
        float thinkInterval = idleMinInterval;

        PoisonCircle poisonCircle = GameObject.FindGameObjectsWithTag("Poison Circle")[0].GetComponent<PoisonCircle>();
        if (Vector2.Distance(transform.position, Vector2.zero) > poisonCircle.circleCollider.radius)
        {
            characterMovement.horizontalMove = -(int)transform.position.x;
            characterMovement.verticalMove = -(int)transform.position.y;
            return thinkInterval;
        }

        // Get a new direction for the zombie to move in
        if (characterState.stateDict[CharacterState.State.aggro])
        {
            if (Mathf.Abs(playerTransform.position.x - transform.position.x) >= Mathf.Abs(playerTransform.position.y - transform.position.y))
            {
                characterMovement.horizontalMove = 1;
                characterMovement.verticalMove = 0;
            }
            else
            {
                characterMovement.horizontalMove = 0;
                characterMovement.verticalMove = 1;
            }

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
            characterMovement.horizontalMove = Random.Range(-1, 1);
            characterMovement.verticalMove = Random.Range(-1, 1);
        }

        return thinkInterval;
    }

}
