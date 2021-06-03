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
    [HideInInspector] public string equipableTag = "Equipable";
    public List<Equipable> equipment = new List<Equipable>();

    /* --- Unity Methods --- */
    void Start()
    {
        characterRenderer.skeleton.root.Attach(characterRenderer.particles[0].skeleton.root);
    }

    void Update()
    {
        MoveFlag();
        SwitchEquipmentFlag();
    }

    void LateUpdate()
    {
        DeathFlag();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        InteractFlag(collider2D);
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

    void InteractFlag(Collider2D collider2D)
    {
        GameObject _object = collider2D.gameObject;
        if (_object.tag == collectibleTag && _object.GetComponent<Collectible>())
        {
            Collect(_object.GetComponent<Collectible>());
        }
        if (_object.tag == equipableTag && _object.GetComponent<Equipable>())
        {
            Equip(_object.GetComponent<Equipable>());
        }
    }

    void SwitchEquipmentFlag()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SwitchEquipment();
        }
    }

    void SwitchEquipment()
    {
        if (equipment.Count == 0) { return; }
        int index = 0;
        for (int i = 0; i < equipment.Count; i++)
        {
            if (equipment[i].gameObject.activeSelf) 
            { 
                index = (i + 1) % equipment.Count;
                if (equipment[i].isAttacking) { return; }
                break; 
            }
        }
        for (int i = 0; i < equipment.Count; i++)
        {
            equipment[i].gameObject.SetActive(false);
            if (i == index) { equipment[i].gameObject.SetActive(true); }
        }
    }

    void Collect(Collectible collectible)
    {
        collectible.Activate(characterState);
    }

    void Equip(Equipable equipable)
    {
        if (equipment.Count < 4)
        {
            for (int i = 0; i < equipment.Count; i++)
            {
                equipment[i].gameObject.SetActive(false);
            }
            equipable.Activate(characterState, characterMovement, characterRenderer.skeleton);
            equipment.Add(equipable);
            hud.hudEquipment.SetEquipment(equipment);
        }
    }

}
