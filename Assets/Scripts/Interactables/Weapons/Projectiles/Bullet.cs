﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{

    public Rigidbody2D body;
    public Weapon gun;
    public bool destroyOnHit = true;
    public float maxLifeTime = 2f;
    public Sound hitSound;
    public Particle flightParticle;
    public float torque = 0f;

    void Start()
    {
        StartCoroutine(IELifetime(maxLifeTime));
        if (flightParticle != null) { flightParticle.Activate(true); }
        //transform.RotateAround(transform.position, Vector3.forward, 90f);
        if (body)
        {
            transform.right = body.velocity.normalized;
            body.AddTorque(torque);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Collide(collider);
    }

    public virtual void Collide(Collider2D collider)
    {
        print("colliders tag is " + collider.tag);
        if (collider.tag == gun.holderState.enemyTag && collider == collider.GetComponent<CharacterState>().hitbox)
        {
            hitSound.PlayAdditively();
            CharacterState targetState = collider.GetComponent<CharacterState>();
            targetState.Damage(gun.stunDuration, gun.attackDamage);
            targetState.Knockback(gun.stunDuration, gun.stunForce, targetState.transform.position - gun.transform.position);
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator IELifetime(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);

        yield return null;
    }
}
