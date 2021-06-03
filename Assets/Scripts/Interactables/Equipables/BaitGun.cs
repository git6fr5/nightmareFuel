using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitGun : Equipable
{

    public Bullet bulletPrefab;
    public float bulletSpeed = 40f;

    private bool hasFired = false;
    /* --- Additional Variables --- */

    /* --- Overridden Unity Methods --- */

    /* --- Overridden Methods --- */
    public override void Attack()
    {
        Fire();
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
                Bullet bullet = Instantiate(bulletPrefab, skeleton.head.transform.position, Quaternion.identity, null).GetComponent<Bullet>();
                bullet.gameObject.SetActive(true);
                bullet.body.velocity = transform.right * bulletSpeed;
                hasFired = true;
            }
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * swingAngleRate);
        }
        else if (isResetting)
        {
            hasFired = false;
            transform.RotateAround(skeleton.root.transform.position, Vector3.forward, rotationFactor * resetAngleRate);
        }
    }
}
