using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    // collectibles
    [HideInInspector] public string collectibleTag = "Collectible";


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
        DeathFlag();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        CollectFlag(collider2D);
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

    void DeathFlag()
    {
        if (characterState.isDead)
        {
            characterState.hud.hudGameOver.gameObject.SetActive(true);
            characterState.hud.hudTimer.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void CollectFlag(Collider2D collider2D)
    {
        GameObject _object = collider2D.gameObject;
        if (_object.tag == collectibleTag && _object.GetComponent<Collectible>())
        {
            CollectCollectible(_object.GetComponent<Collectible>());
        }
    }

    void CollectCollectible(Collectible collectible)
    {
        collectible.Activate();
    }

}
