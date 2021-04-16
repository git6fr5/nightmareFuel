using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PoisonCloud : MonoBehaviour
{
    /* --- Debug --- */
    private string DebugTag = "[Entaku Island] {CharacterState}: ";
    private bool DEBUG_init = false;

    /*--- Components ---*/
    public CircleCollider2D cloudCollider;

    public GameObject poisonPrefab;

    /* --- Internal Variables --- */
    [HideInInspector] public float maxRadius = 40f;
    [HideInInspector] public float radius = 36f;
    [HideInInspector] public float trail = 5f;

    [HideInInspector] public float radiusIncrement;

    /*--- Unity Methods ---*/
    void Start()
    {
        if (DEBUG_init) { print(DebugTag + "Activated"); }
        //print(GameRules.gameDuration);

        cloudCollider.radius = radius;
        Shader.SetGlobalFloat("_Radius", radius / maxRadius);

        radiusIncrement = radius / GameRules.gameDuration * Time.fixedDeltaTime;
    }

    void FixedUpdate()
    {
        if (radius > trail)
        {
            radius = radius - radiusIncrement;
        }
        if (radius < trail)
        {
            radius = trail;
        }

        cloudCollider.radius = radius;
        Shader.SetGlobalFloat("_Radius", radius / maxRadius);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        LayerMask colliderLayer = LayerMask.GetMask(LayerMask.LayerToName(collider.gameObject.layer));
        print(collider.gameObject.name);
        Poison poison = Instantiate(poisonPrefab, collider.transform.position, Quaternion.identity, collider.transform).GetComponent<Poison>();
    }
}
