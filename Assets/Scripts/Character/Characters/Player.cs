﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterRenderer characterRenderer;
    public CharacterMovement characterMovement;
    public Camera cam;
    public HUD hud;

    /* --- Internal Variables --- */
    public Sound collectSound;
    private string collectibleTag = "Collectible";

    /* --- Unity Methods --- */
    void Start()
    {
        characterRenderer.skeleton.root.Attach(characterRenderer.particles[0].skeleton.root);
        if (characterState.weapons.Length > 0) { characterState.weapons[0].Equip(characterState, characterMovement, characterRenderer.skeleton); }
    }

    void Update()
    {
        MoveFlag();
        HurtFlag();
        LowHealthFlag();
        WeaponFlag();
        TargetFlag();
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
    }

    void TargetFlag()
    {
        characterState.targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void HurtFlag()
    {
        /*if (characterState.stateDict[CharacterState.State.hurt])
        {
            float shakeMagnitude = 0.5f;
            cam.GetComponent<CinemachineBrain>().enabled = false;
            cam.transform.position = cam.transform.position + Random.insideUnitSphere * shakeMagnitude;
        }
        else
        {
            cam.GetComponent<CinemachineBrain>().enabled = true;
        }*/
    }

    void LowHealthFlag()
    {
        /*if (characterState.currHealth / characterState.maxHealth < 0.3)
        {
            PostEffects postEffect = cam.GetComponent<PostEffects>();
            postEffect.enabled = true;
            postEffect.index = 0;
        }*/
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
        collectSound.Play();
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
        if (characterState.equippedWeapon != null && !characterState.equippedWeapon.isAttacking)
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
