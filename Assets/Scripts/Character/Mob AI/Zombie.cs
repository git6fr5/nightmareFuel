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

    public LayerMask playerLayer;

    /* --- Internal Variables --- */
    private float aggroMoveDuration = 0.2f;
    private float minMoveDuration = 0.5f;
    private float maxMoveDuration = 3f;

    private Transform aggroTarget;
    private float aggroRadius = 8f;
    private float deAggroRadius = 20f;
    private bool isAggroing = false;

    private float baseSpeed = 1f;
    private float aggroSpeed = 3f;

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

        StartCoroutine(IEZombieMove(minMoveDuration));
        StartCoroutine(IEZombieGrunt(0));
    }

    void Update()
    {
        CheckAggro();
        DeathFlag();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer));

        if (colliderLayer == playerLayer && collider.gameObject.GetComponent<CharacterState>())
        {
            Attack(collider.gameObject.GetComponent<CharacterState>());
        }
    }

    /* --- Methods --- */
    void DeathFlag()
    {
        if (characterState.isDead)
        {
            gameObject.SetActive(false);
        }
    }

    void Attack(CharacterState targetState)
    {
        targetState.Damage(characterState.attackDamage);
    }

    private IEnumerator IEZombieMove(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get a new direction for the zombie to move in
        if (isAggroing && aggroTarget != null)
        {
            characterMovement.horizontalMove = Random.Range(0, 4);
            characterMovement.verticalMove = Random.Range(0, 4);
            if (aggroTarget.position.x < transform.position.x)
            {
                characterMovement.horizontalMove = -characterMovement.horizontalMove;
            }
            if (aggroTarget.position.y < transform.position.y)
            {
                characterMovement.verticalMove = -characterMovement.verticalMove;
            }
            StartCoroutine(IEZombieMove(Random.Range(aggroMoveDuration, minMoveDuration)));
        }
        else if (!isAggroing || aggroTarget == null)
        {
            characterMovement.horizontalMove = Random.Range(-3, 4);
            characterMovement.verticalMove = Random.Range(-3, 4);
            StartCoroutine(IEZombieMove(Random.Range(minMoveDuration, maxMoveDuration)));
        }

        yield return null;
    }

    private IEnumerator IEZombieGrunt(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get a new direction for the zombie to move in
        if (isAggroing)
        {
            characterAnimation.aggro = true;
            characterAnimation.PlaySound();
            StartCoroutine(IEZombieGrunt(Random.Range(gruntDelayMin, gruntDelayMid)));
        }
        else if (!isAggroing)
        {
            characterAnimation.aggro = false;
            characterAnimation.PlaySound();
            StartCoroutine(IEZombieGrunt(Random.Range(gruntDelayMid, gruntDelayMax)));
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
