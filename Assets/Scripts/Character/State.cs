using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class State : Character
{

    /*--- Components ---*/
    public Collider2D hitbox;
    public Collider2D hull;

    /* --- Internal Variables --- */
    [HideInInspector] public float maxHealth = 1f;
    [HideInInspector] public float currHealth = 1f;
    [HideInInspector] public bool isHurt = false;
    [HideInInspector] public bool isDead = false;

    /*--- Unity Methods ---*/
    void Update()
    {
    }

    void OnMouseDown()
    {
    }

    /*--- Methods ---*/
    public void Damage(float damage)
    {
        currHealth = currHealth - damage;
        if (currHealth < 0)
        {
            isDead = true;
        }
        animation.SetEffect("Damaged");
    }
}
