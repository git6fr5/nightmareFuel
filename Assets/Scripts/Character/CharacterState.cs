﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterState : MonoBehaviour
{

    /*--- Components ---*/
    public enum State { idle, mobile, aggro, attacking, stunned, hurt, dead }
    public Dictionary<State, bool> stateDict = new Dictionary<State, bool>();
    public Collider2D hitbox;
    public Rigidbody2D body;
    public Slider health;
    public Emote emote;

    /* --- Internal Variables --- */
    public float maxHealth = 1f;
    public float currHealth = 1f;
    public float attackDamage = 0.1f;
    public string enemyTag;

    /*--- Unity Methods ---*/
    void Start()
    {
        SetHealth();
        SetStatus();
        if (emote) { emote.SetEmoteDict(); }
    }

    void Update()
    {
        Health();
        Motion();
        Status();
    }

    void OnMouseDown()
    {
        // outline
    }

    /*--- Methods ---*/
    void SetHealth()
    {
        if (health != null) { health.maxValue = maxHealth; }
    }

    void SetStatus()
    {
        foreach (State _state in Enum.GetValues(typeof(State)))
        {
            print(_state);
            stateDict.Add(_state, false);    
        }
    }

    void Health()
    {
        if (health != null) { health.value = currHealth; }
    }

    void Motion()
    {
        Vector2 velocity = body.velocity;
        if (Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y) > 0.01f)
        {
            stateDict[State.mobile] = true;
            return;
        }
        stateDict[State.mobile] = false;
    }

    void Status()
    {
        /*foreach (State _state in Enum.GetValues(typeof(State)))
        {
            print(stateDict[_state]);
        }*/
        print(stateDict[State.hurt]);
    }

    public void Damage(float hurtDuration, float damage)
    {
        if (stateDict[State.hurt] == true) { return; }

        currHealth = currHealth - damage;
        if (currHealth <= 0)
        {
            StartCoroutine(IEDeathBuffer(2f));
            emote.OverrideEmote(Emote.Emoticon.skull, 1.5f);
            return;
        }

        stateDict[State.hurt] = true;
        Coroutine hurt = StartCoroutine(IEHurtBuffer(hurtDuration));
        emote.SetEmote(Emote.Emoticon.heartbreak, 1f);

    }

    public void Stun(float stunDuration, float forceMagnitude, Vector2 direction)
    {
        if (stateDict[State.stunned] == true) { return; } // stuns don't stack at all

        Vector2 force = forceMagnitude * direction.normalized;
        body.velocity = force;
        stateDict[State.stunned] = true;
        StartCoroutine(IEStunBuffer(stunDuration));
    }

    public void Aggro(bool _aggro)
    {
        stateDict[State.aggro] = _aggro;
        if (_aggro)
        {
            emote.SetEmote(Emote.Emoticon.exclamation, 1f);
        }
    }

    private IEnumerator IEDeathBuffer(float delay)
    {
        stateDict[State.dead] = true;
        hitbox.enabled = false;

        yield return new WaitForSeconds(delay);

        //gameObject.SetActive(false);
        Destroy(gameObject);

        yield return null;
    }

    private IEnumerator IEHurtBuffer(float delay)
    {
        yield return new WaitForSeconds(delay);

        stateDict[State.hurt] = false;

        yield return null;
    }

    private IEnumerator IEStunBuffer(float delay)
    {
        yield return new WaitForSeconds(delay);

        stateDict[State.stunned] = false;

        yield return null;
    }
}
