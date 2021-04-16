using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    // weapons
    [HideInInspector] public string weaponTag = "Weapon";
    [HideInInspector] public Weapon equippedWeapon;

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
        AttackFlag();
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

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        CheckCollect(collider2D);
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

    void AttackFlag()
    {
        if (Input.GetKeyDown(KeyCode.Space) && equippedWeapon && !equippedWeapon.isAttacking)
        {
            equippedWeapon.StartAttack();
        }
    }

    void CheckCollect(Collider2D collider2D)
    {
        GameObject _object = collider2D.gameObject;
        if (_object.tag == weaponTag && _object.GetComponent<Weapon>())
        {
            CollectWeapon(_object.GetComponent<Weapon>());
        }
        if (_object.tag == collectibleTag && _object.GetComponent<Collectible>())
        {
            CollectCollectible(_object.GetComponent<Collectible>());
        }
    }

    void CollectWeapon(Weapon weapon)
    {
        if (!weapon.isCollectible) { return; }
        if (!equippedWeapon)
        {
            Equip(weapon);
        }
    }

    void CollectCollectible(Collectible collectible)
    {
        collectible.Activate();
    }

    void Equip(Weapon weapon)
    {
        //print("equipping");
        equippedWeapon = weapon;
        weapon.transform.parent = characterState.hand;
        weapon.gameObject.SetActive(true);
        AdjustHandle(weapon);
    }

    void AdjustHandle(Weapon weapon)
    {
        weapon.transform.localPosition = -weapon.handle.localPosition;
    }

}
