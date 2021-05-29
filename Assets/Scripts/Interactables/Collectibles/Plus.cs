using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        body.velocity = initVelocity;
        body.AddTorque(10f);
        StartCoroutine(IEPlusBob(0.5f));

    }

    /* --- Methods --- */
    public override void Activate()
    {
        /*LightSource lightSource = GameObject.FindGameObjectsWithTag("Light Source")[0].GetComponent<LightSource>();
        lightSource.radius = lightSource.radius + (plusValue / Time.fixedDeltaTime) * lightSource.radiusIncrement;
        if (lightSource.radius > lightSource.maxRadius)
        {
            lightSource.radius = lightSource.maxRadius;
        }*/
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
