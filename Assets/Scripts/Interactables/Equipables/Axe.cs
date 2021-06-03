using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Equipable
{
    /* --- Additional Variables --- */

    /* --- Overridden Unity Methods --- */

    /* --- Overridden Methods --- */
    public override void Attack()
    {
        Slash();
    }

    /* --- Additional Methods --- */

    void Slash()
    {
        float rotationFactor = Time.deltaTime * transform.right.x;
        if (isBackSwinging)
        {
            holderControls.stickyDirection = true;
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * backSwingAngleRate);
        }
        else if (isSwinging)
        {
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * swingAngleRate);
        }
        else if (isResetting)
        {
            holderControls.stickyDirection = false;
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * resetAngleRate);
        }
    }
}
