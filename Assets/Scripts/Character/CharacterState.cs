using System.Collections;
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
    public Slider healthSlider;
    public Emote emote;

    /* --- Internal Variables --- */
    public float maxHealth = 1f;
    public float currHealth = 1f;
    public float attackDamage = 0.1f;

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
        if (healthSlider != null) { healthSlider.maxValue = maxHealth; }
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
        if (healthSlider != null) { healthSlider.value = currHealth; }
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
        stateDict[State.hurt] = true;
        StartCoroutine(IEHurtBuffer(hurtDuration));
        emote.SetEmote(Emote.Emoticon.heartbreak, hurtDuration);
        if (currHealth <= 0) { stateDict[State.dead] = true; }
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
