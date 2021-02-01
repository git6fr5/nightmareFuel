using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plus : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Plus}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public LayerMask playerLayer;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer));

        if (colliderLayer == playerLayer)
        {
            Activate();
        }
    }

    /* --- Methods --- */
    void Activate()
    {
        PoisonCloud poisonCloud = GameObject.FindGameObjectsWithTag("Poison Cloud")[0].GetComponent<PoisonCloud>();
        poisonCloud.radius = poisonCloud.radius + 40;
        if (poisonCloud.radius > poisonCloud.maxRadius)
        {
            poisonCloud.radius = poisonCloud.maxRadius;
        }
        Destroy(gameObject);
    }
}
