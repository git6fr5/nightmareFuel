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

    // Signal
    public Transform signal;
    public Transform signalBall;
    public Transform signalRing;
    public Vector3 maxSignalRingScale;
    public Vector3 minSignalRingScale;
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

    /* --- Unity Methods --- */
    void Start()
    {
        // Start the Zombie Spawner
        StartCoroutine(IESpawnerThinker(spawnInterval));
        initSpawnInterval = spawnInterval;
    }

    void FixedUpdate()
    {
        spawnInterval = initSpawnInterval - GameRules.gameTime * spawnIntervalDecreasePerMinute / 60f;
        if (spawnInterval < 0.1f) { spawnInterval = 0.1f; }

        if (signal.gameObject.activeSelf)
        {
            SignalRingFlux();
            SignalBallParabola();
        }
    }
    /* --- Methods --- */

    private IEnumerator IESpawnerThinker(float delay)
    {
        yield return new WaitForSeconds(delay);

        List<Mob> _mobs = new List<Mob>();
        for (int i = 0; i < mobs.Count; i++)
        {
            if (mobs[i] != null) { _mobs.Add(mobs[i]); }
        }
        mobs = _mobs;
        if (mobs.Count < maxMobs)
        {
            Vector3 spawnLocation = RandomSpawnLocation();
            StartCoroutine(IEMobSpawner(spawnInterval / 2, spawnLocation));
        }
        StartCoroutine(IESpawnerThinker(spawnInterval));

        yield return null;
    }

    private void SignalSpawn(Vector3 location, bool _signal)
    {
        if (!_signal)
        {
            signal.gameObject.SetActive(false);
            t = 0;
            return;
        }
        signal.position = location + (Vector3)spawnPrefab.GetComponent<CharacterRenderer>().hull.offset;
        //signalBall.position = characterRenderer.skeleton.head.transform.position;

        signal.gameObject.SetActive(true);
        signalRing.localScale = maxSignalRingScale;
    }

    void SignalRingFlux()
    {
        Vector3 gradient = maxSignalRingScale - minSignalRingScale;
        signalRing.localScale = signalRing.localScale - gradient * spawnInterval / 5f * Time.fixedDeltaTime;
        if (signalRing.localScale.x < minSignalRingScale.x)
        {
            signalRing.localScale = maxSignalRingScale;
        }
    }

    void SignalBallParabola()
    {

        float T = spawnInterval / 2f;

        Vector2 s = signal.position;
        Vector2[] v = new Vector2[] { s, Vector2.zero, new Vector2(s.x / 1.5f, s.y + 5) };
        t = t + Time.fixedDeltaTime ;
        float x = s.x * Mathf.Sqrt(t / T);

        signalBall.position = new Vector3(x, LagrangeInterpolation(x, v), 0);
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

    private IEnumerator IEMobSpawner(float delay, Vector3 location)
    {
        SignalSpawn(location, true);

        yield return new WaitForSeconds(delay);

        Mob mob = Instantiate(spawnPrefab, location, Quaternion.identity, transform).GetComponent<Mob>();
        mob.gameObject.SetActive(true);
        mobs.Add(mob);
        mob.GetComponent<CharacterRenderer>().skeleton.root.Attach(characterRenderer.particles[0].skeleton.root);
        characterRenderer.particles[0].Fire();
        
        SignalSpawn(location, false);

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
