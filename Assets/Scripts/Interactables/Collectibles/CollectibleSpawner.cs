using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    /* --- Components --- */
    public Collectible[] collectibles;
    public PoisonCircle poisonCircle;

    /* --- Internal Variables ---*/
    [Range(0.2f, 0.8f)] public float innerBoundRatio = 0.6f;

    /* --- Unity Methods --- */
    void FixedUpdate()
    {
        CheckTimers();
    }


    /* --- Methods --- */
    void CheckTimers()
    {
        for (int i = 0; i < collectibles.Length; i++)
        {
            if (!collectibles[i].gameObject.activeSelf) { Timer(collectibles[i]); }
        }
    }

    void Timer(Collectible collectible)
    {
        collectible.elapsedTime = collectible.elapsedTime + Time.fixedDeltaTime;
        if (collectible.elapsedTime >= collectible.spawnTime)
        {
            Spawn(collectible);
            collectible.elapsedTime = 0f;
        }
    }

    void Spawn(Collectible collectible)
    {
        float worldBound = poisonCircle.circleCollider.radius * innerBoundRatio;
        float magnitude = Random.Range(worldBound, worldBound);
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector3 randomVector = direction * magnitude;
        collectible.transform.position = randomVector;
        collectible.gameObject.SetActive(true);
    }
}
