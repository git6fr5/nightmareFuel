using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    /* --- Debug --- */
    private string DebugTag = "[DungeonCrawler Weapon]: ";
    private bool DEBUG_init = false;


    /* --- Components --- */
    public Collider2D hitBox;
    public Sprite portrait;
    public Transform handle;
    public CharacterState controllerState;
    public Player controller;

    /* --- Internal Variables ---*/

    // stats
    [HideInInspector] public int maxDurability = 1;
    [HideInInspector] public int durability;
    [HideInInspector] public float attackDamageBonus = 0.5f;

    // swing
    [HideInInspector] protected float backSwingTime = 0.5f;
    [HideInInspector] protected float backSwingAngle = 180f;
    [HideInInspector] protected float swingTime = 0.5f;
    [HideInInspector] protected float swingAngle = -180f;
    [HideInInspector] protected float resetTime = 0.5f;
    [HideInInspector] protected float backSwingAngleRate;
    [HideInInspector] protected float swingAngleRate;
    [HideInInspector] protected float resetAngleRate;

    // states
    [HideInInspector] public bool isCollectible = true;
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isBackSwinging = false;
    [HideInInspector] public bool isSwinging = false;
    [HideInInspector] public bool isResetting = false;

    // resetting
    [HideInInspector] public Vector3 originalPosition;
    [HideInInspector] public Quaternion originalRotation;

    // targetting
    [HideInInspector] public string mobTag = "Mob";


    /* --- Unity Methods --- */
    public virtual void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        SetStats();
        CalculateRotationRate();
    }

    public virtual void Update()
    {
        AttackSwing();
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D collider2D)
    {
    }

    public virtual void OnTriggerStay2D(Collider2D collider2D)
    {
        if (isSwinging) { CheckAttack(collider2D); }
    }

    public virtual void OnTriggerExit2D(Collider2D collider2D)
    {
    }

    /* --- Methods --- */
    public virtual void CheckAttack(Collider2D collider2D)
    {
        GameObject _object = collider2D.gameObject;
        if (_object.tag == mobTag)
        {
            Attack(_object.GetComponent<CharacterState>());
        }
    }

    public virtual void Attack(CharacterState targetState)
    {
        print("attacking a mob");
        targetState.Damage(controllerState.attackDamage + attackDamageBonus);
    }

    public virtual void StartAttack()
    {
        isAttacking = true;
        isBackSwinging = true;
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        controller.characterMovement.stickyDirection = true;
        hitBox.enabled = true;
        StartCoroutine(BackSwing(backSwingTime));
    }

    public virtual void AttackSwing()
    {
    }

    public virtual void SetStats()
    {
        durability = maxDurability;
    }

    protected IEnumerator BackSwing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBackSwinging = false;
        isSwinging = true;
        StartCoroutine(Swing(swingTime));

        yield return null;
    }

    protected IEnumerator Swing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isSwinging = false;
        isResetting = true;
        StartCoroutine(Reset(resetTime));

        yield return null;
    }

    protected IEnumerator Reset(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBackSwinging = false;
        isAttacking = false;

        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        controller.characterMovement.stickyDirection = false;
        hitBox.enabled = false;

        yield return null;
    }

    protected void CalculateRotationRate()
    {
        backSwingAngleRate = backSwingAngle / backSwingTime;
        swingAngleRate = (swingAngle - backSwingAngle) / swingTime;
        resetAngleRate = -swingAngle / resetTime;
    }
}