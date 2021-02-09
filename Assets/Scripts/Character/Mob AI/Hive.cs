using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Hive}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    public GameObject beePrefab;

    /* --- Internal Variables --- */
    private float startInterval = 3f;
    private float spawnInterval = 1.5f;
    private float spawnRadius = 10f;
    private float bufferRadius = 2f;

    private List<Bee> bees = new List<Bee>();
    private int maxBees = 50;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        // Start the Bee Spawner
        StartCoroutine(IEBeeSpawner(startInterval));
        characterAnimation.skeleton.head.Attach(characterAnimation.particles[0].skeleton.root);
        characterAnimation.particles[0].Activate(false);
    }

    /* --- Methods --- */
    private IEnumerator IEBeeSpawner(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bees.Count < maxBees)
        {
            Vector3 spawnLocation = RandomSpawnLocation();
            Bee bee = Instantiate(beePrefab, spawnLocation, Quaternion.identity, transform).GetComponent<Bee>();
            bee.gameObject.SetActive(true);
            bees.Add(bee);
            characterAnimation.particles[0].Fire();
        }
        StartCoroutine(IEBeeSpawner(spawnInterval));

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
