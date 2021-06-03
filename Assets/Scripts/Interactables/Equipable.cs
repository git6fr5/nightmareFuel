using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipable : MonoBehaviour
{

    /* --- Components --- */
    public enum Type { melee, range }
    public enum Slot { root, head, hand }
    public Rigidbody2D body;
    public Skeleton skeleton;


    /* --- Internal Variables ---*/

    // Properties
    public bool isEquipped = false;
    public Type type;
    public Slot slot;

    // Motion
    private Vector3 floatVelocity = new Vector3(0f, 0.3f, 0f); // bob up
    private float floatDuration = 0.5f;

    public float backSwingTime = 0.5f;
    public float swingTime = 0.5f;
    public float resetTime = 0.5f;

    public float backSwingAngle = 180f;
    public float swingAngle = -180f;

    public float attackDamage = 0.5f;
    public float stunDuration = 0.2f;
    public float stunForce = 40f;

    [HideInInspector] protected float backSwingAngleRate;
    [HideInInspector] protected float swingAngleRate;
    [HideInInspector] protected float resetAngleRate;

    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isBackSwinging = false;
    [HideInInspector] public bool isSwinging = false;
    [HideInInspector] public bool isResetting = false;

    public CharacterState holder;
    public CharacterMovement holderControls;
    public Skeleton holderSkeleton;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    /* --- Unity Methods --- */
    void Start()
    {
        Float();
    }

    void Update()
    {
        if (isEquipped && !isAttacking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartAttack();
            }
            if (type == Type.range)
            {
                Point();
            }
        }
        if (isAttacking)
        {
            Attack();
        }
        
    }

    void Point()
    {

        holderControls.stickyDirection = true;

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - holder.transform.position;
        if (dir.x < 0.01f && holderControls.facingRight) { holderControls.Flip(); }
        else if (dir.x > 0.01f && !holderControls.facingRight) { holderControls.Flip(); }

        float angle = 3 * Mathf.Round( Mathf.Atan(dir.y / dir.x) * 180f / Mathf.PI / 3);

        int flip = 0;
        if (!holderControls.facingRight) { angle = -angle; flip = 1; }

        transform.eulerAngles = Vector3.forward * angle + flip * Vector3.up * 180f;
        if (slot == Slot.hand)
        {
            holderSkeleton.hand.Attach(skeleton.root);
        }
        if (slot == Slot.head)
        {
            holderSkeleton.head.Attach(skeleton.root);
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        print("colliders tag is " + collider.tag);
        if (holder != null && collider.tag == holder.enemyTag && isSwinging && collider == collider.GetComponent<CharacterState>().hitbox)
        {
            CharacterState targetState = collider.GetComponent<CharacterState>();
            targetState.Damage(stunDuration, attackDamage);
            targetState.Stun(stunDuration, stunForce, targetState.transform.position - holder.transform.position);
        }
    }

    /* --- Methods --- */
    public void Activate(CharacterState _holder, CharacterMovement _holderControls, Skeleton characterSkeleton)
    {
        if (slot == Slot.hand)
        {
            characterSkeleton.hand.Attach(skeleton.root);
        }
        if (slot == Slot.head)
        {
            characterSkeleton.head.Attach(skeleton.root);
        }
        transform.localRotation = Quaternion.identity;
        holderSkeleton = characterSkeleton;
        holder = _holder;
        holderControls = _holderControls;
        holder.Aggro(true);
        //body.isKinematic = true;
        Destroy(body);
        isEquipped = true;
    }

    void Float()
    {
        body.velocity = floatVelocity;
        StartCoroutine(IEFloat(0.5f));
    }

    public virtual void Attack()
    {

    }

    void StartAttack()
    {
        CalculateRotationRate();
        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;

        isAttacking = true;
        isBackSwinging = true;    
        StartCoroutine(BackSwing(backSwingTime));
    }

    void CalculateRotationRate()
    {
        backSwingAngleRate = backSwingAngle / backSwingTime;
        swingAngleRate = (swingAngle - backSwingAngle) / swingTime;
        resetAngleRate = -swingAngle / resetTime;
    }


    /* --- Virtual Methods --- */



    /* --- Coroutines --- */
    private IEnumerator IEFloat(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (body != null)
        {
            // Bob in the opposite direction
            body.velocity = -body.velocity;
            StartCoroutine(IEFloat(floatDuration));
        }

        yield return null;
    }

    private IEnumerator BackSwing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBackSwinging = false;
        isSwinging = true;
        StartCoroutine(Swing(swingTime));

        yield return null;
    }

    private IEnumerator Swing(float delay)
    {
        yield return new WaitForSeconds(delay);

        isSwinging = false;
        isResetting = true;
        StartCoroutine(Reset(resetTime));

        yield return null;
    }

    private IEnumerator Reset(float delay)
    {
        yield return new WaitForSeconds(delay);

        isBackSwinging = false;
        isAttacking = false;

        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;

        yield return null;
    }


}
