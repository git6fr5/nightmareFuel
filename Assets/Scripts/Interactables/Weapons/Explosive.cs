using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : Weapon
{

    /* --- Additional Variables --- */
    public Exploder exploderPrefab;
    public float throwSpeed = 40f;
    public float throwRange = 10f;
    public float explosionRadius = 5f;
    public Transform[] trace;
    public Signal signal;
    public float arcHeight = 2f;
    public Particle throwParticle;

    private bool hasThrown = false;

    /* --- Overridden Unity Methods --- */

    /* --- Overridden Methods --- */
    public override void Attack()
    {
        Throw();
    }

    public override void Point()
    {
        //if (!Input.GetMouseButtonDown(0)) { return; }
        if (holderState.targetPosition == null) { return; }
        if (isAttacking)
        {
            for (int i = 0; i < trace.Length; i++)
            {
                trace[i].gameObject.SetActive(false);
            }
            return;
        }
        holderMovement.stickyDirection = true;

        Vector2 dir = holderState.targetPosition - holderState.transform.position;
        if (dir.x < 0.01f && holderMovement.facingRight) { holderMovement.Flip(); }
        else if (dir.x > 0.01f && !holderMovement.facingRight) { holderMovement.Flip(); }

        Vector2 A = holderState.targetPosition;
        Vector2 B = holderState.transform.position;

        if (Vector2.Distance(A, B) > throwRange) { A = (A - B).normalized * throwRange + B; }

        signal.Activate((Vector3)A, 1f); 

        Vector2[] v = new Vector2[] { A, B, new Vector2((A.x + B.x)/ 2, A.y + arcHeight) };
        holderSkeleton.hand.transform.eulerAngles = Vector3.zero;

        for (int i = 0; i < trace.Length; i++)
        {
            float t = ((float)i) / trace.Length;
            t = 0.05f * t + (1 - t) * 0.95f;
            float x = A.x * t  + (1 - t) * B.x;
            print(t);
            trace[i].gameObject.SetActive(true);
            trace[i].position = new Vector3(x, LagrangeInterpolation(x, v), 0);
        }
    }

    float LagrangeInterpolation(float x, Vector2[] v)
    {
        float y = 0f;
        for (int i = 0; i < v.Length; i++)
        {
            float num = v[i].y;
            float denom = 1f;
            for (int j = 0; j < v.Length; j++)
            {
                if (i != j)
                {
                    num = num * (x - v[j].x);
                    denom = denom * (v[i].x - v[j].x);
                }
            }
            y = y + num / denom;
        }
        return y;
    }

    /* --- Additional Methods --- */
    void Throw()
    {
        if (isBackSwinging)
        {
        }
        else if (isSwinging)
        {
            if (!hasThrown)
            {
                // The exploder
                Exploder exploder = Instantiate(exploderPrefab, skeleton.head.transform.position, Quaternion.identity, null).GetComponent<Exploder>();
                exploder.SetPath(holderState.targetPosition, holderState.transform.position, arcHeight, throwSpeed);
                exploder.gameObject.SetActive(true);
                hasThrown = true;
            }
        }
        else if (isResetting)
        {
            hasThrown = false;
        }
    }
}
