using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    /* --- Components --- */
    public Skeleton skeleton;
    public Sound activateSound;


    /* --- Internal Variables ---*/

    // Properties
    public bool isEquipped = false;

    // Motion
    public float backSwingTime = 0.5f;
    public float swingTime = 0.5f;
    public float resetTime = 0.5f;

    public float attackDamage = 0.5f;
    public float stunDuration = 0.2f;
    public float stunForce = 40f;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isBackSwinging = false;
    [HideInInspector] public bool isSwinging = false;
    [HideInInspector] public bool isResetting = false;

    public CharacterState holderState;
    public CharacterMovement holderMovement;
    public Skeleton holderSkeleton;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    /* --- Unity Methods --- */
    void Start()
    {
    }

    void Update()
    {
        if (isEquipped)
        {
            Point();
            if (isAttacking)
            {
                Attack();
            }
        }      
    }

    void OnEnable()
    {
        isAttacking = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        /*print("colliders tag is " + collider.tag);
        if (holderState!= null && collider.tag == holderState.enemyTag && isSwinging && collider == collider.GetComponent<CharacterState>().hitbox)
        {
            CharacterState targetState = collider.GetComponent<CharacterState>();
            targetState.Damage(stunDuration, attackDamage);
            targetState.Stun(stunDuration, stunForce, targetState.transform.position - holderState.transform.position);
        }*/
    }

    /* --- Methods --- */
    public void Activate()
    {
        activateSound.PlayAdditively();
        StartCoroutine(StartAttack());
    }

    public void Equip(CharacterState _holderState, CharacterMovement _holderMovement, Skeleton _holderSkeleton)
    {
        // Attach the weapon
        _holderSkeleton.hand.Attach(skeleton.root);
        transform.localRotation = Quaternion.identity;

        // Set the parameters
        holderSkeleton = _holderSkeleton;
        holderState = _holderState;
        holderMovement = _holderMovement;
        holderState.Aggro(true);

        // Adjust the internal settings
        gameObject.SetActive(true);
        holderState.equippedWeapon = this;
        isEquipped = true;
    }

    public void DeEquip()
    {
        if (holderState != null && holderState.equippedWeapon == this)
        {
            holderState.equippedWeapon = null;
        }

        /*isSwinging = false;
        isBackSwinging = false;
        isResetting = false;*/
        isAttacking = false;
        isEquipped = false;

        if (originalPosition != null && originalRotation != null)
        {
            transform.localPosition = originalPosition;
            transform.localRotation = originalRotation;
        }
        gameObject.SetActive(false);
    }


    /* --- Virtual Methods --- */
    public virtual void Attack()
    {
        // do attack
    }

    public virtual void Point()
    {
        // point   
    }

    /* --- Coroutines --- */
    private IEnumerator StartAttack()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        isAttacking = true;
        isBackSwinging = true;
        
        yield return StartCoroutine(BackSwing(backSwingTime));
    }

    private IEnumerator BackSwing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBackSwinging = false;
        isSwinging = true;
        
        yield return StartCoroutine(Swing(swingTime));

    }

    private IEnumerator Swing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isSwinging = false;
        isResetting = true;
        
        yield return StartCoroutine(Reset(resetTime)); ;
    }

    private IEnumerator Reset(float delay)
    {
        yield return new WaitForSeconds(delay);

        isResetting = false;
        isAttacking = false;

        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        yield return null;
    }


}
