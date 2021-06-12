using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterRenderer characterRenderer;
    public CharacterMovement characterMovement;
    public Sound spawnSound;
    public Sound interludeSound;

    /* --- Internal Variables --- */

    // Signal
    public Signal signal;
    public Transform spawnerBall;
    float t = 0;

    // Spawned Object
    public GameObject spawnPrefab;
    public int maxMobs = 50;
    private List<Mob> mobs = new List<Mob>();

    // Settings
    public float spawnInterval = 3f;
    private float initSpawnInterval;
    public float spawnIntervalDecreasePerMinute = 1f;
    public float spawnRadius = 8f;
    private float bufferRadius = 4f;
    public float interludeInterval = 15f;

    /* --- Unity Methods --- */
    void Start()
    {
        StartCoroutine(IESpawnerThinker(spawnInterval));
        StartCoroutine(IESpawnerInterlude(interludeInterval));
        initSpawnInterval = spawnInterval;
    }

    void FixedUpdate()
    {
        spawnInterval = GetSpawnInterval();

        if (spawnerBall.gameObject.activeSelf)
        {
            SpawnerBallParabola();
        }
    }

    /* --- Coroutines --- */
    private IEnumerator IESpawnerThinker(float delay)
    {
        yield return new WaitForSeconds(delay);

        mobs = GetMobList();
        if (mobs.Count < maxMobs)
        {
            Vector3 spawnLocation = GetSpawnLocation();
            StartCoroutine(IEMobSpawner(spawnInterval / 2, spawnLocation));
        }

        yield return StartCoroutine(IESpawnerThinker(delay));
    }

    private IEnumerator IEMobSpawner(float delay, Vector3 location)
    {
        // Play the sound
        spawnSound.Play();

        // Activate the signal
        signal.Activate(location + (Vector3)spawnPrefab.GetComponent<CharacterRenderer>().hull.offset, spawnInterval / 5f);
        SpawnerBallActivate(true);
  
        yield return new WaitForSeconds(delay);

        // Spawn the Mob
        Mob mob = SpawnMob(location);
        mobs.Add(mob);

        // Deactivate the signal
        signal.Deactivate();
        SpawnerBallActivate(false);

        yield return null;
    }

    private IEnumerator IESpawnerInterlude(float delay)
    {
        yield return new WaitForSeconds(delay);

        interludeSound.Play();
        StartCoroutine(IESpawnerInterlude(interludeInterval));

        yield return null;
    }


    /* --- Methods --- */
    Mob SpawnMob(Vector3 location)
    {
        // Create the mob
        Mob mob = Instantiate(spawnPrefab, location, Quaternion.identity, transform).GetComponent<Mob>();
        mob.gameObject.SetActive(true);
        
        // Fire the spawning particles
        mob.GetComponent<CharacterRenderer>().skeleton.root.Attach(characterRenderer.particles[0].skeleton.root);
        characterRenderer.particles[0].transform.SetParent(transform); // incase the mob gets insta destroyed, we dont want to destroy the particle as well
        characterRenderer.particles[0].Fire();

        return mob;
    }

    void SpawnerBallActivate(bool activate) 
    {
        spawnerBall.localPosition = Vector3.zero;
        spawnerBall.gameObject.SetActive(activate);
        t = 0;
    }

    float GetSpawnInterval()
    {
        float _spawnInterval = initSpawnInterval - GameRules.gameTime * spawnIntervalDecreasePerMinute / 60f;
        if (spawnInterval < 0.1f) { spawnInterval = 0.1f; }
        return _spawnInterval;
    }

    private List<Mob> GetMobList()
    {
        List<Mob> _mobs = new List<Mob>();
        for (int i = 0; i < mobs.Count; i++)
        {
            if (mobs[i] != null) { _mobs.Add(mobs[i]); }
        }
        return _mobs;
    }
    
    private void SpawnerBallParabola()
    {
        float T = spawnInterval / 2f;

        Vector2 s = signal.transform.position;
        Vector2[] v = new Vector2[] { s, Vector2.zero, new Vector2(s.x / 1.5f, s.y + 5) };
        t = t + Time.fixedDeltaTime ;
        float x = s.x * Mathf.Sqrt(t / T);

        spawnerBall.position = new Vector3(x, LagrangeInterpolation(x, v), 0);
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

    private Vector3 GetSpawnLocation()
    {
        Vector3 center = transform.position;
        float magnitude = Random.Range(bufferRadius, spawnRadius);
        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        Vector3 randomVector = direction * magnitude;
        Vector3 spawnLocation = randomVector + center;
        return spawnLocation;
    }

}
