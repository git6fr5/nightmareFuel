using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CharacterState : MonoBehaviour
{

    /*--- Components ---*/
    public Collider2D hitbox;
    public Collider2D hull;

    /* --- Internal Variables --- */
    [HideInInspector] public float maxHealth = 1f;
    [HideInInspector] public float currHealth = 1f;
    [HideInInspector] public bool isHurt = false;
    [HideInInspector] public bool isDead = false;

    [HideInInspector] public float depth = 0;

    /*--- Unity Methods ---*/
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
