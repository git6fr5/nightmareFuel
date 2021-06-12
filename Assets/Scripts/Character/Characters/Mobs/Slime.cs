using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Mob
{

    [HideInInspector] public int depth = 1;
    public int maxDepth = 3;
    bool hasSplit = false;
    Vector2 targetPoint;

    public float tackleDamage;
    public float tackleDuration;
    public float tackleForce;

    /* --- Overridden Methods --- */
    public override float MoveFlag()
    {
        // Get a new direction for the character to move in
        float thinkInterval = idleMinInterval;

        PoisonCircle poisonCircle = GameObject.FindGameObjectsWithTag("Poison Circle")[0].GetComponent<PoisonCircle>();
        if (Vector2.Distance(transform.position, Vector2.zero) > poisonCircle.circleCollider.radius)
        {
            characterMovement.horizontalMove = -(int)transform.position.x;
            characterMovement.verticalMove = -(int)transform.position.y;
            return thinkInterval;
        }

        if (targetPoint == null || Random.Range(0f, 1f) < 0.1f || Vector2.Distance(targetPoint, Vector2.zero) > poisonCircle.circleCollider.radius || Vector2.Distance(targetPoint, Vector2.zero) > 2f)
        {
            targetPoint = Random.insideUnitCircle * poisonCircle.circleCollider.radius * 4 / 5;
        }
        characterMovement.horizontalMove = (targetPoint - (Vector2)transform.position).x;
        characterMovement.verticalMove = (targetPoint - (Vector2)transform.position).y;

        return thinkInterval;
    }

    public override void CollideFlag(Collider2D collider)
    {
        CharacterState targetState = collider.GetComponent<CharacterState>();
        targetState.Damage(tackleDuration / depth, tackleDamage);
        targetState.Knockback(tackleDuration / depth, tackleForce / depth, targetState.transform.position - transform.position);
    }

    public override void DeathFlag()
    {
        if (characterState.stateDict[CharacterState.State.dead])
        {
            if (depth == maxDepth && drop.transform.parent != null)
            {
                drop.gameObject.SetActive(true);
                drop.transform.SetParent(null);
                drop.transform.localScale = new Vector2(1f, 1f);
            }
            else if (depth < maxDepth && !hasSplit)
            {
                Split(2);
                hasSplit = true;
            }
        }
    }

    void Split(int numSplits)
    {
        for (int i = 0; i < numSplits; i++)
        {
            Slime slime = Instantiate(gameObject, transform.position + (Vector3)Random.insideUnitCircle, Quaternion.identity, null).GetComponent<Slime>();
            slime.gameObject.SetActive(true);
            slime.characterState.stateDict[CharacterState.State.dead] = false;
            slime.characterState.emote.ForceEmoteOff();
            slime.characterState.hitbox.enabled = true;
            slime.depth = depth + 1;
            slime.transform.localScale = transform.localScale / 2f;
            slime.characterState.maxHealth = 0.8f * characterState.maxHealth;
            slime.characterState.currHealth = 0.8f * characterState.currHealth;
        }
    }
}
