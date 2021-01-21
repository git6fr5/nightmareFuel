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
    public CharacterAnimation characterAnimation;
    public CharacterState characterState;

    /* --- Internal Variables ---*/
    private float speed = 5f;
    private Vector3 velocity = Vector3.zero;
    private float movementSmoothing = 0.05f;
    private bool facingRight = true;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
    }

    void Update()
    {
        if (characterState.isClient) { Move(); }

        if (Input.GetKeyDown("p"))
        {
            print(characterAnimation.particles[0].skeleton);
            characterAnimation.skeleton.root.Attach(characterAnimation.particles[0].skeleton.root);
            characterAnimation.particles[0].gameObject.SetActive(true);

        }
    }

    /* --- Methods --- */
    void Move()
    {
        // Get the input from the player
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        if (horizontalMove < 0 && facingRight) { Flip(); }
        else if (horizontalMove > 0 && !facingRight) { Flip(); }
        float verticalMove = Input.GetAxisRaw("Vertical");

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
    }
}
