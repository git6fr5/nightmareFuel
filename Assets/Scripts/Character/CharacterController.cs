using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    /* ----- Info ----- */
    // This script controls the character
    // Links the state, movement, and animation
    // Parses any input or automatic behaviours


    /* --- Debug --- */
    private string DebugTag = "[NightmareFuel CharacterController]";
    private bool Debug = false;


    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;


    /* --- Unity Methods --- */
    public virtual void Start()
    {

    }

    /* --- Methods --- */
    public virtual void DeathFlag()
    {
        if (isDead) { gameObject.SetActive(false); }
    }

    public virtual void Damage(float damage)
    {
        currHealth = currHealth - damage;
        if (currHealth < 0)
        {
            isDead = true;
        }
    }
}
