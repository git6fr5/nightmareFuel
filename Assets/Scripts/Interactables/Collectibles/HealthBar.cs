using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Collectible
{
    /* --- Debug --- */

    /*--- Components ---*/
    public Rigidbody2D body;

    /* --- Internal Variables --- */
    [HideInInspector] public float healthValue = 0.5f;
    private Vector3 initVelocity = new Vector3(0f, 0.3f, 0f); // bob up

    /*--- Unity Methods ---*/
    public override void Start()
    {
        body.velocity = initVelocity;
        StartCoroutine(IEHealthBob(0.5f));

    }

    /* --- Methods --- */
    public override void Activate(CharacterState characterState)
    {
        characterState.currHealth = characterState.currHealth + healthValue;
        if (characterState.currHealth > characterState.maxHealth)
        {
            characterState.currHealth = characterState.maxHealth;
        }
        Destroy(gameObject);
    }

    private IEnumerator IEHealthBob(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Bob in the opposite direction
        body.velocity = -body.velocity;
        StartCoroutine(IEHealthBob(0.5f));

        yield return null;
    }
}
