using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningRod : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {LightningRod}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    public GameObject pinkEyePrefab;

    /* --- Internal Variables --- */
    private float spawnInterval = 3f;
    private float spawnRadius = 4f;
    private float bufferRadius = 2f;

    private List<PinkEye> pinkEyes = new List<PinkEye>();
    private int maxPinkEyes = 50;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }
        // Start the Zombie Spawner
        StartCoroutine(IEPinkEyeSpawner(spawnInterval));
        characterAnimation.skeleton.head.Attach(characterAnimation.particles[0].skeleton.root);
        characterAnimation.particles[0].Activate(false);
    }

    /* --- Methods --- */
    private IEnumerator IEPinkEyeSpawner(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (pinkEyes.Count < maxPinkEyes)
        {
            Vector3 spawnLocation = RandomSpawnLocation();
            PinkEye pinkEye = Instantiate(pinkEyePrefab, spawnLocation, Quaternion.identity, transform).GetComponent<PinkEye>();
            pinkEye.gameObject.SetActive(true);
            pinkEyes.Add(pinkEye);
            characterAnimation.particles[0].Fire();
        }
        StartCoroutine(IEPinkEyeSpawner(spawnInterval));

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
