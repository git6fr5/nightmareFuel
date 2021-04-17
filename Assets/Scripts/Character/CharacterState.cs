using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterState : MonoBehaviour
{

    /* ----- Info ----- */
    // This script controls the basic state of a character
    // Holds all necessary variables required such as
    // current health, movespeed, etc
    // It also holds all the necessary components here    


    /* --- Debug --- */
    private string DebugTag = "[NightmareFuel CharacterState]";
    private bool Debug = false;


    /*--- Components ---*/

    // rendering
    public SpriteRenderer spriteRenderer; // renders the characters sprite

    // hud
    public HUD hud; // the pl
    public Sprite portrait; // th

    // collisions
    public Collider2D hitbox; // controls trigger effects
    public Collider2D hull; // controls percieved position and actual volume
    public Transform hand; // 

    /* --- Internal Variables --- */

    // stats
    public float maxHealth = 1f;
    [HideInInspector] public float currHealth = 1f;
    public float baseAttackDamage = 0.5f;
    public float attackTime = 0.5f;

    // states
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool isDead = false;

    // rendering
    [HideInInspector] public float depth = 0;

    // equipment
    [HideInInspector] public Weapon equippedWeapon;


    /*--- Unity Methods ---*/
    void Start()
    {
        CheckDepth();
    }

    void Update()
    {
        CheckDepth();
    }

    /*--- Methods ---*/
    public void CheckDepth()
    {
        depth = transform.position.y + hull.offset.y;
    }
}
