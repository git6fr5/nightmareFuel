using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullOrb : Grenade
{
    bool effect = false;

    public override void Explode()
    {
        throwTime = 0f;
        GetComponent<SpriteRenderer>().enabled = false;
        if (explodeParticle != null) { explodeParticle.Fire(); }
        explodeSound.Play();

        effect = true;

        float length = explosive.stunDuration;
        //if (explodeParticle != null) { length = explodeParticle.clip.length; }
        StartCoroutine(IEAfterEffect(length));
    }

    private IEnumerator IEAfterEffect(float delay)
    {
        yield return new WaitForSeconds(delay);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosive.explosionRadius, opaqueLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == explosive.holderState.enemyTag && colliders[i] == colliders[i].GetComponent<CharacterState>().hitbox)
            {
                CharacterState targetState = colliders[i].GetComponent<CharacterState>();
                //targetState.Damage(explosive.stunDuration, explosive.attackDamage);
                targetState.Paralyze(explosive.stunDuration);
            }
        }
        Destroy(gameObject);

        yield return null;
    }

    public override void Effect()
    {
        if (effect)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosive.explosionRadius, opaqueLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == explosive.holderState.enemyTag && colliders[i] == colliders[i].GetComponent<CharacterState>().hitbox)
                {
                    CharacterState targetState = colliders[i].GetComponent<CharacterState>();
                    //targetState.Damage(explosive.stunDuration, explosive.attackDamage);
                    float dist = Vector2.Distance(targetState.transform.position, transform.position);
                    Vector2 dir = targetState.transform.position - transform.position;
                    targetState.Knockback(Time.deltaTime, dist * explosive.stunForce, -dir);
                }
            }
        }
    }
}
