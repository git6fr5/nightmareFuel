using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {CharacterMovement}: ";
    private bool DEBUG_init = true;

    /* --- Components --- */
    public Rigidbody2D body;

    /* --- Internal Variables ---*/
    private float speed = 20f;
    private Vector3 velocity = Vector3.zero;
    private float movementSmoothing = 0.05f;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
    }

    void Update()
    {
        Move();
    }

    /* --- Methods --- */
    void Move()
    {
        // Get the input from the player
        float horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        float verticalMove = Input.GetAxisRaw("Vertical") * speed;

        // Apply the movement
        Vector3 targetVelocity = new Vector2(horizontalMove, verticalMove);
        body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref velocity, movementSmoothing);
    }
}
