using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    /* --- Components --- */
    public enum Type { healthVial, poisonVial, plusVial }
    public Rigidbody2D body;


    /* --- Internal Variables ---*/

    // Properties
    [HideInInspector] public bool isCollectible = true;
    public Type type;
    
    // Spawning
    public float spawnTime = 10f;
    public float elapsedTime = 0f;

    // Motion
    private Vector3 floatVelocity = new Vector3(0f, 0.5f, 0f); // bob up
    private float floatDuration = 0.5f;

    // Signal
    public Signal signal;
    public Vector3 signalOffset;

    /* --- Unity Methods --- */
    void Start()
    {
        Float();
    }

    void Update()
    {

    }

    void OnEnable()
    {
        signal.Activate(transform.position + signalOffset, floatDuration * 2);
    }

    /* --- Methods --- */
    public void Activate(CharacterState characterState)
    {
        Effect(characterState);
        gameObject.SetActive(false);
    }

    void Float()
    {
        body.velocity = floatVelocity;
        StartCoroutine(IEFloat(0.5f));
    }


    /* --- Virtual Methods --- */
    public virtual void Effect(CharacterState characterState)
    {

    }


    /* --- Coroutines --- */
    private IEnumerator IEFloat(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Bob in the opposite direction
        body.velocity = -body.velocity;
        StartCoroutine(IEFloat(floatDuration));

        yield return null;
    }
}
