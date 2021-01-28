using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Poison}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */

    /* --- Internal Variables --- */
    private float baseDamage = 0.05f;
    private float attackTime = 0.5f;


    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        StartCoroutine(IEPoisonEffect(attackTime));
    }

    private IEnumerator IEPoisonEffect(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (transform.parent.GetComponent<CharacterState>() != null)
        {
            transform.parent.GetComponent<CharacterState>().Damage(baseDamage);
        }
        StartCoroutine(IEPoisonEffect(attackTime));

        yield return null;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer));
        if (collider.tag == "Poison Cloud")
        {
            Destroy(gameObject);
        }
    }
}
