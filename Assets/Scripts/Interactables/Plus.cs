using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Plus : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Plus}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public LayerMask playerLayer;
    public Rigidbody2D body;
    public Light2D shadow;

    /* --- Internal Variables --- */
    [HideInInspector] public float plusValue = 0f;
    private Vector3 shadowPosition; // we want this to be stationary
    private Quaternion shadowRotation; // we want this to be stationary
    private float shadowFactor = 0.999f; // shrink shadow as we go up
    private float shadowDiff = -0.002f; // the increment
    private Vector3 initVelocity = new Vector3(0f, 0.3f, 0f); // bob up

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        body.velocity = initVelocity;
        body.AddTorque(10f);
        StartCoroutine(IEPlusBob(0.5f));
        shadowPosition = shadow.transform.position;
        shadowRotation = shadow.transform.rotation;

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer));

        if (colliderLayer == playerLayer)
        {

            collider.gameObject.GetComponent<CharacterAnimation>().collect = true;
            collider.gameObject.GetComponent<CharacterAnimation>().PlaySound();
            if (collider.gameObject.GetComponent<CharacterAnimation>()) { print("made collect true"); }
            Activate();
        }
    }

    void Update()
    {
        shadow.pointLightOuterRadius = shadow.pointLightOuterRadius * shadowFactor;
        shadow.transform.position = shadowPosition;
        shadow.transform.rotation = shadowRotation;
    }

    /* --- Methods --- */
    void Activate()
    {
        PoisonCloud poisonCloud = GameObject.FindGameObjectsWithTag("Poison Cloud")[0].GetComponent<PoisonCloud>();
        poisonCloud.radius = poisonCloud.radius + (plusValue / Time.fixedDeltaTime) * poisonCloud.radiusIncrement;
        if (poisonCloud.radius > poisonCloud.maxRadius)
        {
            poisonCloud.radius = poisonCloud.maxRadius;
        }
        Destroy(gameObject);
    }

    private IEnumerator IEPlusBob(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Bob in the opposite direction
        shadowDiff = -shadowDiff;
        shadowFactor = shadowFactor + shadowDiff;
        body.velocity = -body.velocity;
        StartCoroutine(IEPlusBob(0.5f));

        yield return null;
    }
}
