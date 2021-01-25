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
    public Light2D vision;
    public Collider2D hitbox;
    public Collider2D hull;
    public SpriteRenderer spriteRenderer;

    /* --- Info --- */
    public bool isClient;
    public Sprite portrait;

    /* --- Stats --- */
    public int maxHealth = 1;
    public int currHealth = 1;
    public float attackTime = 0.5f;
    public bool isAttacking = false;

    public float depth = 0;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        //if (isClient) { hud.gameObject.SetActive(true); hud.Inspect(this); }
        //if (isClient) { vision.gameObject.SetActive(true); }
        depth = transform.position.y + hull.offset.y;
    }

    void Update()
    {
        depth = transform.position.y + hull.offset.y;
        //print(name + ": " + depth.ToString());
    }

    void OnMouseDown()
    {
    }
}
