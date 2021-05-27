using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {Bee}: ";
    private bool DEBUG_init = false;

    /* --- Components --- */
    public CharacterState characterState;
    public CharacterAnimation characterAnimation;
    public CharacterMovement characterMovement;

    public LayerMask playerLayer;
    public LayerMask mobLayer;

    /* --- Internal Variables --- */
    private float moveDuration = 0.2f;

    private float bobbleThreshold = 0f;
    private float maxRadius = 30f;

    private Transform aggroTarget;
    private float aggroRadiusMax = 5f;
    private float aggroRadiusMin = 3f;

    private bool isAggroing = false;

    private Vector3 spawnPoint;
    private Vector3 controlPoint;
    private Vector3 controlPoint1;
    private Vector3 controlPoint2;
    
    private float baseSpeed = 8f;
    private float aggroSpeed = 15f;

    private float baseDamage = 0.1f;

    private float volume = 0.3f;
    private float gruntDelayMin = 2.0f;
    private float gruntDelayMid = 3.0f;
    private float gruntDelayMax = 10.0f;

    //private PoisonCloud poisonCloud;

    /* --- Unity Methods --- */
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated for " + gameObject.name); }

        characterMovement.speed = baseSpeed;
        characterState.attackDamage = baseDamage;
        characterAnimation.audioSource.volume = volume;

        ControlPoint();

        StartCoroutine(IEBeeMove(moveDuration));
        StartCoroutine(IEBeeGrunt(0));

        //poisonCloud = GameObject.FindGameObjectsWithTag("Poison Cloud")[0].GetComponent<PoisonCloud>();

    }

    void Update()
    {
        if (isAggroing)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, aggroRadiusMin, mobLayer);
            foreach (Collider2D _collider in colliders)
            {
                if (_collider.gameObject.GetComponent<Bee>())
                {
                    Bee neighbouringBee = _collider.gameObject.GetComponent<Bee>();
                    neighbouringBee.characterMovement.speed = aggroSpeed;
                    neighbouringBee.isAggroing = true;
                    neighbouringBee.aggroTarget = aggroTarget;
                }
            }
        }

        maxRadius = 10f;  //poisonCloud.radius * 2 / 3;
        CheckControlPoint();
        //print(maxIdleCircleRadius);
    }

    void LateUpdate()
    {
        if (characterState.isDead)
        {
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer));

        if (colliderLayer == playerLayer)
        {
            Attack(collider.gameObject.GetComponent<CharacterState>());

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, aggroRadiusMax, mobLayer);
            foreach (Collider2D _collider in colliders)
            {
                if (_collider.gameObject.GetComponent<Bee>())
                {
                    Bee neighbouringBee = _collider.gameObject.GetComponent<Bee>();
                    neighbouringBee.characterMovement.speed = aggroSpeed;
                    neighbouringBee.isAggroing = true;
                    neighbouringBee.aggroTarget = collider.transform;
                }
            }

            characterState.isDead = true;
        }
    }

    /* --- Methods --- */
    void Attack(CharacterState targetState)
    {
        targetState.Damage(characterState.attackDamage);
    }

    private IEnumerator IEBeeMove(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get a new direction for the bee to move in
        if (isAggroing && aggroTarget != null)
        {
            Vector2 hypotenuse = new Vector2(aggroTarget.position.x - transform.position.x, aggroTarget.position.y - transform.position.y);
            Vector2 hypotenuseNorm = hypotenuse.normalized;
            Vector2 tangent = new Vector2(hypotenuseNorm.y, -hypotenuseNorm.x);
            if (Random.Range(0f, 1f) < bobbleThreshold || hypotenuse.magnitude > aggroRadiusMin) // have to bobble if too far away
            {
                characterMovement.horizontalMove = hypotenuseNorm.x;
                characterMovement.verticalMove = hypotenuseNorm.y;
            }
            else
            {
                characterMovement.horizontalMove = tangent.x;
                characterMovement.verticalMove = tangent.y;

            }
            StartCoroutine(IEBeeMove(moveDuration));
        }
        else if (!isAggroing || aggroTarget == null)
        {
            Vector2 hypotenuse = new Vector2(controlPoint.x - transform.position.x, controlPoint.y - transform.position.y);
            Vector2 hypotenuseNorm = hypotenuse.normalized;
            Vector2 tangent = new Vector2(hypotenuse.y, -hypotenuse.x);

            if (hypotenuse.magnitude > aggroRadiusMax) // have to bobble if too far away
            {
                characterMovement.horizontalMove = hypotenuseNorm.x;
                characterMovement.verticalMove = hypotenuseNorm.y;
            }
            else
            {
                characterMovement.horizontalMove = tangent.x;
                characterMovement.verticalMove = tangent.y;
            }

            StartCoroutine(IEBeeMove(moveDuration));
        }

        yield return null;
    }

    private IEnumerator IEBeeGrunt(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Get a new direction for the zombie to move in
        if (isAggroing)
        {
            characterAnimation.aggro = true;
            characterAnimation.PlaySound();
            StartCoroutine(IEBeeGrunt(Random.Range(gruntDelayMin, gruntDelayMid)));
        }
        else if (!isAggroing)
        {
            characterAnimation.aggro = false;
            characterAnimation.PlaySound();
            StartCoroutine(IEBeeGrunt(Random.Range(gruntDelayMid, gruntDelayMax)));
        }

        yield return null;
    }

    private void ControlPoint()
    {
        controlPoint = new Vector3(Random.Range(-maxRadius, maxRadius), Random.Range(-maxRadius, maxRadius), 0);
    }

    private void CheckControlPoint()
    {
        if (controlPoint.magnitude > maxRadius)
        {
            controlPoint = controlPoint / 1.1f;
        }
    }

}
