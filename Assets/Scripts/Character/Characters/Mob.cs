﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterRenderer characterRenderer;
    public CharacterMovement characterMovement;
    public Collectible drop;

    /* --- Internal Variables --- */

    // Aggro Variables
    protected Transform playerTransform;

    // Settings
    public float healthIncreasePerMinute = 0.3f;

    public float aggroMinInterval = 0.2f;
    public float aggroMaxInterval = 0.5f;
    public float aggroSpeed = 3f;
    public float aggroSpeedIncreasePerMinute = 1f;
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
        playerTransform = GameObject.FindWithTag(characterState.enemyTag).transform;

        // Increase the speed
        aggroSpeed += GameRules.gameTime * aggroSpeedIncreasePerMinute / 60f;
        characterState.maxHealth += GameRules.gameTime * healthIncreasePerMinute / 60f;
        characterState.currHealth = characterState.maxHealth;

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
        if (collider.tag == characterState.enemyTag)
        {
            AttackFlag(collider.GetComponent<CharacterState>());
        }
    }

    /* --- Methods --- */
    void DeathFlag()
    {
        if (characterState.stateDict[CharacterState.State.dead])
        {
            drop.gameObject.SetActive(true);
            drop.transform.SetParent(null);
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

        if (characterState.stateDict[CharacterState.State.stunned]) 
        {
            StartCoroutine(IEMoveFlag(aggroMinInterval));
            yield return null;
        }

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
