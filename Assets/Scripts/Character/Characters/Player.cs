using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterRenderer characterRenderer;
    public CharacterMovement characterMovement;
    public HUD hud;

    // collectibles
    [HideInInspector] public string collectibleTag = "Collectible";


    /* --- Unity Methods --- */
    void Start()
    {
        characterRenderer.skeleton.root.Attach(characterRenderer.particles[0].skeleton.root);
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
        if (characterState.stateDict[CharacterState.State.stunned]) { return; }

        characterMovement.horizontalMove = Input.GetAxisRaw("Horizontal");
        characterMovement.verticalMove = Input.GetAxisRaw("Vertical");
    }

    void DeathFlag()
    {
        if (characterState.stateDict[CharacterState.State.dead])
        {
            hud.hudGameOver.gameObject.SetActive(true);
            hud.hudTimer.gameObject.SetActive(false);
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
        collectible.Activate(characterState);
    }

}
