using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterState : MonoBehaviour
{

    /*--- Components ---*/
    public enum State { idle, mobile, aggro, attacking, knockback, paralyzed, hurt, dead }
    public Dictionary<State, bool> stateDict = new Dictionary<State, bool>();
    public Collider2D hitbox;
    public Rigidbody2D body;
    public Slider health;
    public Emote emote;
    public Weapon[] weapons;

    /* --- Internal Variables --- */
    public float maxHealth = 1f;
    public float currHealth = 1f;
    public float attackDamage = 0.1f;
    public float deathDuration = 2f;
    [HideInInspector] public Weapon equippedWeapon;
    public Vector3 targetPosition;
    public string enemyTag;
    private Vector2 knockbackForce;


    /*--- Unity Methods ---*/
    void Awake()
    {
        if (emote) { emote.SetEmoteDict(); }
        SetStatus();
    }

    void Start()
    {
        SetHealth();
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
        stateDict[State.mobile] = false;
        if (stateDict[State.knockback])
        {
            body.velocity = knockbackForce;
            return;
        }

        Vector2 velocity = body.velocity;
        if (Mathf.Abs(velocity.x) + Mathf.Abs(velocity.y) > 0.01f)
        {
            stateDict[State.mobile] = true;
            return;
        }    }

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
            StartCoroutine(IEDeathBuffer(deathDuration));
            emote.OverrideEmote(Emote.Emoticon.skull, deathDuration * 0.95f);
            return;
        }

        stateDict[State.hurt] = true;
        Coroutine hurt = StartCoroutine(IEHurtBuffer(hurtDuration));
        emote.SetEmote(Emote.Emoticon.heartbreak, 1f);

    }

    public void Knockback(float knockbackDuration, float forceMagnitude, Vector2 direction)
    {
        if (stateDict[State.knockback] == true) { return; } // stuns don't stack at all

        knockbackForce = forceMagnitude * direction.normalized;
        stateDict[State.knockback] = true;
        StartCoroutine(IEKnockbackBuffer(knockbackDuration));
    }

    public void Paralyze(float paralyzeDuration)
    {
        if (stateDict[State.paralyzed] == true) { return; } // don't stack at all

        emote.SetEmote(Emote.Emoticon.lightning, paralyzeDuration);
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        stateDict[State.paralyzed] = true;
        StartCoroutine(IEParalyzeBuffer(paralyzeDuration));

    }

    public void Aggro(bool _aggro)
    {
        stateDict[State.aggro] = _aggro;
        if (_aggro && emote)
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

    private IEnumerator IEKnockbackBuffer(float delay)
    {
        yield return new WaitForSeconds(delay);

        body.velocity = Vector3.zero;
        knockbackForce = Vector2.zero;
        stateDict[State.knockback] = false;

        yield return null;
    }

    private IEnumerator IEParalyzeBuffer(float delay)
    {
        yield return new WaitForSeconds(delay);

        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        stateDict[State.paralyzed] = false;

        yield return null;
    }
}
