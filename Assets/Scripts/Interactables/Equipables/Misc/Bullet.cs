using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Rigidbody2D body;
    public Equipable gun;
    public bool destroyOnHit = true;
    public float maxLifeTime = 2f;

    void Start()
    {
        StartCoroutine(IELifetime(maxLifeTime));
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        print("colliders tag is " + collider.tag);
        if (collider.tag == gun.holder.enemyTag && collider == collider.GetComponent<CharacterState>().hitbox)
        {
            CharacterState targetState = collider.GetComponent<CharacterState>();
            targetState.Damage(gun.stunDuration, gun.attackDamage);
            targetState.Stun(gun.stunDuration, gun.stunForce, targetState.transform.position - transform.position);
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
