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

    /* --- Internal Variables --- */
    private string collectibleTag = "Collectible";

    /* --- Unity Methods --- */
    void Start()
    {
        characterRenderer.skeleton.root.Attach(characterRenderer.particles[0].skeleton.root);
    }

    void Update()
    {
        MoveFlag();
        WeaponFlag();
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
            hud.hudScore.gameObject.SetActive(false);
        }
    }

    void CollectFlag(Collider2D collider2D)
    {
        GameObject _object = collider2D.gameObject;
        if (_object.tag == collectibleTag && _object.GetComponent<Collectible>())
        {
            Collect(_object.GetComponent<Collectible>());
        }
    }

    void Collect(Collectible collectible)
    {
        collectible.Activate(characterState);
    }

    void WeaponFlag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ActivateWeapon();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SwitchWeapon();
        }
    }

    void ActivateWeapon()
    {
        if (!characterState.equippedWeapon.isAttacking)
        {
            characterState.equippedWeapon.Activate();
        }
    }

    void SwitchWeapon()
    {
        if (characterState.weapons.Length == 0) { return; }
        if (characterState.equippedWeapon == null) 
        { 
            characterState.weapons[0].Equip(characterState, characterMovement, characterRenderer.skeleton);
            return;
        }
        if (characterState.equippedWeapon.isAttacking) { return; }

        int index = 0;
        for (int i = 0; i < characterState.weapons.Length; i++)
        {
            if (characterState.weapons[i] == characterState.equippedWeapon)
            {
                index = (i + 1) % characterState.weapons.Length;
                break;
            }
        }

        characterState.equippedWeapon.DeEquip();
        characterState.weapons[index].Equip(characterState, characterMovement, characterRenderer.skeleton);
    }

}
