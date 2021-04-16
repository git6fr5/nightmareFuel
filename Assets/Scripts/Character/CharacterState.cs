using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterState : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {CharacterState}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public HUD hud;
    public Light2D shadow;
    public Collider2D hitbox;
    public Collider2D hull;
    public Transform hand;
    public SpriteRenderer spriteRenderer;

    /* --- Internal Variables --- */
    //public bool isClient;
    public Sprite portrait;

    [HideInInspector] public float maxHealth = 1f;
    [HideInInspector] public float currHealth = 1f;

    [HideInInspector] public float attackDamage = 0.5f;
    [HideInInspector] public float attackTime = 0.5f;
    [HideInInspector] public bool isAttacking = false;

    [HideInInspector] public bool isDead = false;

    [HideInInspector] public float depth = 0;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        CheckDepth();
    }

    void Update()
    {
        CheckDepth();
    }

    void OnMouseDown()
    {
    }

    /*--- Methods ---*/
    public void CheckDepth()
    {
        depth = transform.position.y + hull.offset.y;
    }

    public void Damage(float damage)
    {
        currHealth = currHealth - damage;
        if (currHealth < 0)
        {
            isDead = true;
        }
    }
}
