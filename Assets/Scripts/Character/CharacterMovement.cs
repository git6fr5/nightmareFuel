using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {CharacterMovement}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public Rigidbody2D body;
    public Hand forehand;
    public Hand offhand;

    public CharacterAnimation characterAnimation;
    public CharacterState characterState;

    /* --- Internal Variables ---*/
    [HideInInspector] public float speed = 5f;
    [HideInInspector] public float horizontalMove = 0f;
    [HideInInspector] public float verticalMove = 0f;
    private Vector3 velocity = Vector3.zero;
    private float movementSmoothing = 0.05f;
    private bool facingRight = true;
    [HideInInspector] public bool stickyDirection = false;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
    }

    void FixedUpdate()
    {
        if (characterState.isMobile) { Move(); }
    }

    /* --- Methods --- */
    void Move()
    {
        if (!stickyDirection)
        {
            if (horizontalMove < 0 && facingRight) { Flip(); }
            else if (horizontalMove > 0 && !facingRight) { Flip(); }
        }

        // Apply the movement
        Vector3 targetVelocity = new Vector2(horizontalMove, verticalMove).normalized * speed;
        body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref velocity, movementSmoothing);

        characterAnimation.speed = Mathf.Abs(horizontalMove) + Mathf.Abs(verticalMove);
    }

    void Flip()
    {
        // Switch the way the player is labelled as facing
        facingRight = !facingRight;

        // Flip the player transform
        transform.Rotate(0f, 180f, 0f);

        if (!facingRight)
        {
            characterState.spriteRenderer.material.SetFloat("_isFlipped", -1);
        }
        else
        {
            characterState.spriteRenderer.material.SetFloat("_isFlipped", 1);
        }

        if (forehand) { forehand.Flip(); }
        if (offhand) { offhand.Flip(); }
    }
}