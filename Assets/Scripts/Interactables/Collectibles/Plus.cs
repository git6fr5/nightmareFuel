﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : Collectible
{
    /* --- Debug --- */

    /*--- Components ---*/
    public Rigidbody2D body;

    /* --- Internal Variables --- */
    [HideInInspector] public float plusValue = 0f;
    private Vector3 initVelocity = new Vector3(0f, 0.3f, 0f); // bob up

    /*--- Unity Methods ---*/
    public override void Start()
    {
        body.velocity = initVelocity;
        body.AddTorque(10f);
        StartCoroutine(IEPlusBob(0.5f));

    }

    /* --- Methods --- */
    public override void Activate(CharacterState characterState)
    {
        PoisonCircle poisonCircle = GameObject.FindGameObjectsWithTag("Poison Circle")[0].GetComponent<PoisonCircle>();
        poisonCircle.circleCollider.radius = poisonCircle.circleCollider.radius + (plusValue / Time.fixedDeltaTime) * poisonCircle.radiusIncrement;
        if (poisonCircle.circleCollider.radius > poisonCircle.initRadius)
        {
            poisonCircle.circleCollider.radius = poisonCircle.initRadius;
        }
        Destroy(gameObject);
    }

    private IEnumerator IEPlusBob(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Bob in the opposite direction
        body.velocity = -body.velocity;
        StartCoroutine(IEPlusBob(0.5f));

        yield return null;
    }
}
