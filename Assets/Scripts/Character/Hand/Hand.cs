using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Hand}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public CharacterState characterState;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D body;

    /* --- Internal Variables ---*/
    private Transform tempParent;
    private Vector2 originalPos;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        tempParent = transform.parent;
    }

    void Update()
    {
        spriteRenderer.sortingOrder = characterState.spriteRenderer.sortingOrder;
    }

    /* --- Methods --- */
    public void Flip()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, -pos.z);
    }

    public void BeginAttack()
    {
        body.velocity = new Vector3(10, 0, 0);
        originalPos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        transform.parent = null;
        StartCoroutine(Attacking(characterState.attackTime));
    }

    private IEnumerator Attacking(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.parent = tempParent;
        body.velocity = new Vector3(0, 0, 0);

        transform.localPosition = new Vector3(originalPos.x, originalPos.y, transform.localPosition.z);

        yield return null;
    }
}
