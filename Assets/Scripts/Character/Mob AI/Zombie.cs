using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Zombie}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    /* --- Internal Variables --- */
    private float minMoveDuration = 0.5f;
    private float maxMoveDuration = 3f;
    //private float aggroRadius = 2f;
    //private float deAggroRadius = 6f;
    private bool isAggroing = false;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        ZombieSetup();
        StartCoroutine(IEZombieMove(minMoveDuration));
    }

    /* --- Methods --- */
    void ZombieSetup()
    {
        characterMovement.speed = 1f;
    }

    private IEnumerator IEZombieMove(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get a new direction for the zombie to move in
        if (isAggroing)
        {
            
        }
        else if (!isAggroing)
        {
            characterMovement.horizontalMove = Random.Range(-3, 4);
            characterMovement.verticalMove = Random.Range(-3, 4);
        }

        StartCoroutine(IEZombieMove(Random.Range(minMoveDuration, maxMoveDuration)));

        yield return null;
    }
}
