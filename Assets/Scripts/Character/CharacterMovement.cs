using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    /* --- Components --- */
    public Rigidbody2D body;

    /* --- Internal Variables ---*/
    [HideInInspector] public bool isMobile = true;
    [HideInInspector] public float speed = 5f;
    [HideInInspector] public float horizontalMove = 0f;
    [HideInInspector] public float verticalMove = 0f;
    private Vector3 velocity = Vector3.zero;
    private float movementSmoothing = 0.05f;
    private bool facingRight = true;

    /* --- Unity Methods --- */
    void FixedUpdate()
    {
        if (isMobile) { Move(); }
    }

    /* --- Methods --- */
    void Move()
    {
        if (horizontalMove < 0 && facingRight) { Flip(); }
        else if (horizontalMove > 0 && !facingRight) { Flip(); }

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
    }
}