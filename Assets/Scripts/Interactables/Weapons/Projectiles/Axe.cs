using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Melee
{
    /* --- Additional Variables --- */
    [HideInInspector] protected float backSwingAngleRate;
    [HideInInspector] protected float swingAngleRate;
    [HideInInspector] protected float resetAngleRate;

    public float backSwingAngle = 180f;
    public float swingAngle = -180f;

    private bool hasSwung = false;

    /* --- Overridden Unity Methods --- */

    /* --- Overridden Methods --- */
    public override void Attack()
    {
        Slash();
    }

    /* --- Additional Methods --- */
    void Slash()
    {
        if (!hasSwung)
        {
            CalculateRotationRate();
            hasSwung = true;
        }

        float rotationFactor = Time.deltaTime * transform.right.x;
        if (isBackSwinging)
        {
            holderMovement.stickyDirection = true;
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * backSwingAngleRate);
        }
        else if (isSwinging)
        {
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * swingAngleRate);
        }
        else if (isResetting)
        {
            holderMovement.stickyDirection = false;
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * resetAngleRate);
            hasSwung = false;
        }
    }

    void CalculateRotationRate()
    {
        backSwingAngleRate = backSwingAngle / backSwingTime;
        swingAngleRate = (swingAngle - backSwingAngle) / swingTime;
        resetAngleRate = -swingAngle / resetTime;
    }
}
