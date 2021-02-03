using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkEye : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {PinkEye}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    public LayerMask playerLayer;

    /* --- Internal Variables --- */
    private float aggroMoveDuration = 3f;
    private float minMoveDuration = 0.2f;
    private float maxMoveDuration = 1f;

    private Transform aggroTarget;
    private float aggroRadius = 8f;
    private float deAggroRadius = 10f;
    private bool isAggroing = false;

    private float baseSpeed = 0.2f;
    private float aggroSpeed = 8f;

    private float baseDamage = 0.1f;

    private float volume = 0.3f;
    private float gruntDelayMin = 2.0f;
    private float gruntDelayMid = 3.0f;
    private float gruntDelayMax = 10.0f;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }

        characterMovement.speed = baseSpeed;
        characterState.attackDamage = baseDamage;
        characterAnimation.audioSource.volume = volume;

        StartCoroutine(IEPinkEyeMove(minMoveDuration));
        StartCoroutine(IEPinkEyeGrunt(0));
    }

    void Update()
    {
        CheckAggro();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer));

        if (colliderLayer == playerLayer)
        {
            Attack(collider.gameObject.GetComponent<CharacterState>());
        }
    }

    /* --- Methods --- */
    void Attack(CharacterState targetState)
    {
        targetState.Damage(characterState.attackDamage);
    }

    private IEnumerator IEPinkEyeMove(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get a new direction for the zombie to move in
        if (isAggroing && aggroTarget != null)
        {
            if (Mathf.Abs(aggroTarget.position.x - transform.position.x) >= Mathf.Abs(aggroTarget.position.y - transform.position.y))
            {
                characterMovement.horizontalMove = 1;
                characterMovement.verticalMove = 0;
            }
            else
            {
                characterMovement.horizontalMove = 0;
                characterMovement.verticalMove = 1;
            }

            if (aggroTarget.position.x < transform.position.x)
            {
                characterMovement.horizontalMove = -characterMovement.horizontalMove;
            }
            if (aggroTarget.position.y < transform.position.y)
            {
                characterMovement.verticalMove = -characterMovement.verticalMove;
            }
            StartCoroutine(IEPinkEyeMove(Random.Range(maxMoveDuration, aggroMoveDuration)));
        }
        else if (!isAggroing || aggroTarget == null)
        {
            characterMovement.horizontalMove = Random.Range(-1, 1);
            characterMovement.verticalMove = Random.Range(-1, 1);
            StartCoroutine(IEPinkEyeMove(Random.Range(minMoveDuration, maxMoveDuration)));
        }

        yield return null;
    }

    private IEnumerator IEPinkEyeGrunt(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get a new direction for the zombie to move in
        if (isAggroing)
        {
            characterAnimation.aggro = true;
            characterAnimation.PlaySound();
            StartCoroutine(IEPinkEyeGrunt(Random.Range(gruntDelayMin, gruntDelayMid)));
        }
        else if (!isAggroing)
        {
            characterAnimation.aggro = false;
            characterAnimation.PlaySound();
            StartCoroutine(IEPinkEyeGrunt(Random.Range(gruntDelayMid, gruntDelayMax)));
        }

        yield return null;
    }

    void CheckAggro()
    {
        if (isAggroing && aggroTarget != null)
        {
            if (Vector2.Distance(transform.position, aggroTarget.position) > deAggroRadius)
            {
                characterMovement.speed = baseSpeed;
                characterMovement.horizontalMove = 0;
                characterMovement.verticalMove = 0;
                isAggroing = false;
                aggroTarget = null;
            }
        }
        else if (!isAggroing || aggroTarget == null)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, aggroRadius, playerLayer);
            if (colliders.Length > 0)
            {
                characterMovement.speed = aggroSpeed;
                isAggroing = true;
                aggroTarget = colliders[0].transform;
            }
        }
    }
}
