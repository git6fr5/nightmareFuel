using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tombstone : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Tombstone}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    public GameObject zombiePrefab;

    /* --- Internal Variables --- */
    private float spawnInterval = 3f;
    private float spawnRadius = 4f;
    private float bufferRadius = 2f;

    private List<Zombie> zombies = new List<Zombie>();
    private int maxZombies = 50;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        // Start the Zombie Spawner
        StartCoroutine(IEZombieSpawner(spawnInterval));
        characterAnimation.skeleton.root.Attach(characterAnimation.particles[0].skeleton.root);
        characterAnimation.particles[0].Activate(false);

        characterState.maxHealth = 100f;
        characterState.currHealth = 100f;
    }

    void Update()
    {
        DeathFlag();
    }

    /* --- Methods --- */
    void DeathFlag()
    {
        if (characterState.isDead)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator IEZombieSpawner(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (zombies.Count < maxZombies)
        {
            Vector3 spawnLocation = RandomSpawnLocation();
            Zombie zombie = Instantiate(zombiePrefab, spawnLocation, Quaternion.identity, transform).GetComponent<Zombie>();
            zombie.gameObject.SetActive(true);
            zombies.Add(zombie);
            characterAnimation.particles[0].Fire();
        }
        StartCoroutine(IEZombieSpawner(spawnInterval));

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
