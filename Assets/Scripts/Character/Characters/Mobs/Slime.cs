using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Mob
{

    [HideInInspector] public int depth = 1;
    public int maxDepth = 3;
    bool hasSplit = false;

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
