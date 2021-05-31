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

    public float awakeInterval = 1f;

    public float idleMinInterval = 0.5f;
    public float idleMaxInterval = 3f;
    public float idleSpeed = 1f;

    public float stunDuration = 0.2f;
    public float stunForce = 25f;

    /* --- Unity Methods --- */
    void Start()
    {
        // Cache the player transform
        playerTransform = GameObject.FindWithTag(playerTag).transform;

        // Initialize the co-routines
        StartCoroutine(IEAggroFlag(awakeInterval));
        StartCoroutine(IEMoveFlag(awakeInterval));
    }

    void Update()
    {
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

    private IEnumerator IEAggroFlag(float delay)
    {
        yield return new WaitForSeconds(delay);

        float thinkInterval = AggroFlag();
        StartCoroutine(IEAggroFlag(thinkInterval));

        yield return null;
    }

    public virtual float AggroFlag()
    {
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        if (characterState.stateDict[CharacterState.State.aggro] && distance > deAggroRadius)
        {
            characterMovement.speed = idleSpeed;
            characterState.Aggro(false);
        }
        else if (!characterState.stateDict[CharacterState.State.aggro] && distance < aggroRadius)
        {
            characterMovement.speed = aggroSpeed;
            characterState.Aggro(true);
        }
        return aggroMaxInterval;
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
        targetState.Damage(stunDuration, characterState.attackDamage);
        targetState.Stun(stunDuration, stunForce, targetState.transform.position - transform.position);
    }

}
