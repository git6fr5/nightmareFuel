using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : Weapon
{

    /* --- Additional Variables --- */
    public Bullet bulletPrefab;
    public float bulletSpeed = 40f;
    public float kickBackForce;
    public Particle muzzleFlare;
    public float kickBackDuration;

    private bool hasFired = false;

    /* --- Overridden Unity Methods --- */

    /* --- Overridden Methods --- */
    public override void Attack()
    {
        Fire();
    }

    public override void Point()
    {

        if (holderState.targetPosition == null) { return; }
        holderMovement.stickyDirection = true;

        Vector2 dir = holderState.targetPosition - holderState.transform.position;
        if (dir.x < 0.01f && holderMovement.facingRight) { holderMovement.Flip(); }
        else if (dir.x > 0.01f && !holderMovement.facingRight) { holderMovement.Flip(); }

        float angle = 3 * Mathf.Round(Mathf.Atan(dir.y / dir.x) * 180f / Mathf.PI / 3);

        int flip = 0;
        if (!holderMovement.facingRight) { angle = -angle; flip = 1; }

        holderSkeleton.hand.transform.eulerAngles = Vector3.forward * angle + flip * Vector3.up * 180f;
        //transform.localRotation = skeleton.root.transform.localRotation;
        //skeleton.root.transform.rotation = Quaternion.identity;
    }

    /* --- Additional Methods --- */
    void Fire()
    {
        float rotationFactor = Time.deltaTime * transform.right.x;
        if (isBackSwinging)
        {
        }
        else if (isSwinging)
        {
            if (!hasFired) 
            {
                // The muzzle flare
                skeleton.head.Attach(muzzleFlare.skeleton.root);
                muzzleFlare.Fire();

                // Knockback the player
                holderState.Knockback(kickBackDuration, kickBackForce, -transform.right);

                // The bullet
                Bullet bullet = Instantiate(bulletPrefab, skeleton.head.transform.position, Quaternion.identity, null).GetComponent<Bullet>();
                bullet.gameObject.SetActive(true);
                bullet.body.velocity = transform.right * bulletSpeed;
                hasFired = true;
            }
            //transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * swingAngleRate);
        }
        else if (isResetting)
        {
            hasFired = false;
            //transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * resetAngleRate);
        }
    }
}
