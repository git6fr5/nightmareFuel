using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;


    /* --- Unity Methods --- */
    void Start()
    {
        characterAnimation.skeleton.root.Attach(characterAnimation.particles[0].skeleton.root);
    }

    void Update()
    {
        MoveFlag();
    }

    void LateUpdate()
    {
        if (characterState.isDead)
        {
            characterState.hud.hudGameOver.gameObject.SetActive(true); 
            characterState.hud.hudTimer.gameObject.SetActive(false); 
            gameObject.SetActive(false);
        }
    }

    /* --- Methods --- */
    void MoveFlag()
    {
        // Get the input from the player
        characterMovement.horizontalMove = Input.GetAxisRaw("Horizontal");
        characterMovement.verticalMove = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(characterMovement.horizontalMove) + Mathf.Abs(characterMovement.verticalMove) > 0)
        {
            characterAnimation.particles[0].Activate(true);
        }
        else
        {
            characterAnimation.particles[0].Activate(false);
        }
    }
}
