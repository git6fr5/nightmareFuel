using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterRenderer characterRenderer;
    public CharacterMovement characterMovement;


    /* --- Internal Variables --- */

    // Aggro Variables
    protected Transform playerTransform;
    protected string playerTag = "Player";
    
    // Settings
    public float aggroMinInterval = 0.2f;
    public float aggroMaxInterval = 0.5f;
    public float aggroSpeed = 3f;
    public float aggroRadius = 8f;
    public float deAggroRadius = 20f;

    public float idleMinInterval = 0.5f;
    public float idleMaxInterval = 3f;
    public float idleSpeed = 1f;

    public float knockbackDuration = 0.3f;
    public float knockbackHeight = 0.5f;

    /* --- Unity Methods --- */
    void Start()
    {
        // Cache the player transform
        playerTransform = GameObject.FindWithTag(playerTag).transform;

        // Initialize the co-routines
        StartCoroutine(IEMoveFlag(idleMinInterval));
    }

    void Update()
    {
        AggroFlag();
        DeathFlag();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == playerTag)
        {
            AttackFlag(collider.GetComponent<CharacterState>());
        }
    }

    /* --- Methods --- */
    void DeathFlag()
    {
        if (characterState.stateDict[CharacterState.State.dead])
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void AggroFlag()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (characterState.stateDict[CharacterState.State.aggro] && distance > deAggroRadius)
        {
            characterMovement.speed = idleSpeed;
            characterState.stateDict[CharacterState.State.aggro] = false;
        }
        else if (distance < aggroRadius)
        {
            characterMovement.speed = aggroSpeed;
            characterState.stateDict[CharacterState.State.aggro] = true;
        }
    }

    private IEnumerator IEMoveFlag(float delay)
    {
        yield return new WaitForSeconds(delay);

        float thinkInterval = MoveFlag();
        StartCoroutine(IEMoveFlag(thinkInterval));

        yield return null;
    }

    public virtual float MoveFlag()
    {
        return idleMinInterval;
    }

    public virtual void AttackFlag(CharacterState targetState)
    {
        targetState.TakeDamage(characterState.attackDamage);
    }

}
