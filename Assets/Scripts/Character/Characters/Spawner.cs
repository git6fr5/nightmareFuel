using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterRenderer characterRenderer;
    public CharacterMovement characterMovement;

    /* --- Internal Variables --- */
    
    // Spawned Object
    public GameObject spawnPrefab;
    public int maxMobs = 50;
    private List<Mob> mobs = new List<Mob>();

    // Settings
    public float spawnInterval = 3f;
    public float spawnRadius = 4f;
    private float bufferRadius = 2f;

    /* --- Unity Methods --- */
    void Start()
    {
        // Start the Zombie Spawner
        StartCoroutine(IEMobSpawner(spawnInterval));
        characterRenderer.skeleton.root.Attach(characterRenderer.particles[0].skeleton.root);
        characterRenderer.particles[0].Activate(false);
    }

    /* --- Methods --- */

    private IEnumerator IEMobSpawner(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (mobs.Count < maxMobs)
        {
            Vector3 spawnLocation = RandomSpawnLocation();
            Mob mob = Instantiate(spawnPrefab, spawnLocation, Quaternion.identity, transform).GetComponent<Mob>();
            mob.gameObject.SetActive(true);
            mobs.Add(mob);
            characterRenderer.particles[0].Fire();
        }
        StartCoroutine(IEMobSpawner(spawnInterval));

        yield return null;
    }

    private Vector3 RandomSpawnLocation()
    {
        Vector3 center = transform.position;
        float magnitude = Random.Range(bufferRadius, spawnRadius);
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector3 randomVector = direction * magnitude;
        Vector3 spawnLocation = randomVector + center;
        return spawnLocation;
    }

}
