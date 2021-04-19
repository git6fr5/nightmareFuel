﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{

    /* --- Overridden Variables ---*/
    public Axe()
    {
        backSwingTime = 0.2f;
        swingTime = 0.1f;
        resetTime = 0.4f;

        backSwingAngle = 270f;
        swingAngle = -200f;

        attackDamageBonus = 0.5f;
    }

    /* --- Additional Variables --- */

    /* --- Overridden Unity Methods --- */

    /* --- Overridden Methods --- */
    public override void AttackSwing()
    {
        if (isAttacking)
        {
            SlashSwing();
        }
    }

    /* --- Additional Methods --- */
    void SlashSwing()
    {
        float rotationFactor = Time.deltaTime * transform.right.x;
        if (isBackSwinging)
        {
            transform.RotateAround(handle.position, Vector3.forward, rotationFactor * backSwingAngleRate);
        }
        else if (isSwinging)
        {
            transform.RotateAround(handle.position, Vector3.forward, rotationFactor * swingAngleRate);
        }
        else if (isResetting)
        {
            transform.RotateAround(handle.position, Vector3.forward, rotationFactor * resetAngleRate);
        }
    }
}