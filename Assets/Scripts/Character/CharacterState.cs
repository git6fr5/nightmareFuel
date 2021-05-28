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
    public Collider2D hull;
    public Rigidbody2D body;
    public Slider healthSlider;

    /* --- Internal Variables --- */
    public float maxHealth = 1f;
    public float currHealth = 1f;
    public float attackDamage = 0.1f;
    private float hurtBuffer = 0.2f;

    /*--- Unity Methods ---*/
    void Start()
    {
        SetHealth();
        SetStatus();
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
        if (Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y) > 0.1f)
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

    public void TakeDamage(float damage)
    {
        if (stateDict[State.hurt] == true) { return; }

        currHealth = currHealth - damage;
        stateDict[State.hurt] = true;
        StartCoroutine(IEHurtBuffer(hurtBuffer));
        if (currHealth <= 0) { stateDict[State.dead] = true; }
    }

    private IEnumerator IEHurtBuffer(float delay)
    {
        yield return new WaitForSeconds(delay);

        stateDict[State.hurt] = false;

        yield return null;
    }
}
