using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Projectile
{
    public Particle explodeParticle;
    public LayerMask opaqueLayer;
    public Explosive explosive;
    public Sound explodeSound;

    protected Vector2 target;
    protected Vector2 origin;
    protected float arcHeight;
    protected float throwTime;
    protected float t = 0f;

    void FixedUpdate()
    {
        if (throwTime > 0f)
        {
            ThrowParabola();
        }
        Effect();
    }

    public void SetPath(Vector2 _target, Vector2 _origin, float _arcHeight, float throwSpeed)
    {
        target = _target;
        origin = _origin;
        arcHeight = _arcHeight;
        throwTime = Vector2.Distance(target, origin) / throwSpeed;
        t = 0f;
        StartCoroutine(IEExplode(throwTime));
    }

    private void ThrowParabola()
    {
        float T = throwTime;

        Vector2 A = target;
        Vector2 B = origin;

        Vector2[] v = new Vector2[] { A, B, new Vector2((A.x + B.x) / 2, A.y + arcHeight) };
        t = t + Time.fixedDeltaTime;
        float x = A.x * t / T + (1 - t / T) * B.x;

        transform.position = new Vector3(x, LagrangeInterpolation(x, v), 0);
        transform.RotateAround(transform.position, Vector3.forward, 2f);
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

    private IEnumerator IEExplode(float delay)
    {
        yield return new WaitForSeconds(delay);

        Explode();

        yield return null;
    }

    public virtual void Explode()
    {
        throwTime = 0f;
        GetComponent<SpriteRenderer>().enabled = false;
        explodeParticle.Fire();
        explodeSound.Play();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosive.explosionRadius, opaqueLayer);
        print(colliders.Length);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].tag == explosive.holderState.enemyTag && colliders[i] == colliders[i].GetComponent<CharacterState>().hitbox)
            {
                CharacterState targetState = colliders[i].GetComponent<CharacterState>();
                targetState.Knockback(explosive.stunDuration, explosive.stunForce, targetState.transform.position - transform.position);
            }
        }

        StartCoroutine(IEDestroy(explodeParticle.clip.length));
    }

    public virtual void Effect()
    {
        // do effect
    }

    protected IEnumerator IEDestroy(float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(gameObject);

        yield return null;
    }
}
