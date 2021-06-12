using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushOrb : Bullet
{
    public Vector2 minScale;
    public Vector2 maxScale;
    private int depth = 0;

    void Update()
    {
        transform.localScale = transform.localScale + (Vector3)(maxScale - minScale) * Time.deltaTime / maxLifeTime;
    }

    public override void Collide(Collider2D collider)
    {
        print("colliders tag is " + collider.tag);
        if (collider.tag == gun.holderState.enemyTag && collider == collider.GetComponent<CharacterState>().hitbox)
        {
            hitSound.PlayAdditively();
            CharacterState targetState = collider.GetComponent<CharacterState>();
            targetState.Damage(gun.stunDuration, gun.attackDamage);
            targetState.Knockback(gun.stunDuration, gun.stunForce, transform.right);

            if (depth < 3)
            {
                Range range = gun.GetComponent<Range>();
                PushOrb bullet1 = Instantiate(gameObject, transform.position, Quaternion.identity, null).GetComponent<PushOrb>();
                bullet1.transform.localScale = minScale;
                bullet1.gameObject.SetActive(true);
                bullet1.body.velocity = transform.up * range.bulletSpeed;
                bullet1.maxLifeTime = maxLifeTime / 2;
                bullet1.depth = depth + 1;

                PushOrb bullet2 = Instantiate(gameObject, transform.position, Quaternion.identity, null).GetComponent<PushOrb>();
                bullet2.transform.localScale = minScale;
                bullet2.gameObject.SetActive(true);
                bullet2.body.velocity = -transform.up * range.bulletSpeed;
                bullet2.maxLifeTime = maxLifeTime / 2;
                bullet2.depth = depth + 1;
            }
            
            if (destroyOnHit)
            {
                Destroy(gameObject);
            }

        }
    }
}
