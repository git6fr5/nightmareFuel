using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{

    private Vector3 initLocalPosition;

    // Flutter
    private float flutterRadius = 0.05f;
    private float flutterSpeed = 0.1f;
    private Vector3 displacement = new Vector3(0f, 0f, 0f);
    public Color color;

    /* --- Internal Variables --- */
    public GameObject shadowPrefab;
    public LayerMask opaqueLayer;
    public CircleCollider2D areaOfEffect;
    public float centerIntensity = 1f;

    // Start is called before the first frame update
    void Start()
    {
        initLocalPosition = transform.localPosition;
        StartCoroutine("Flutter", flutterSpeed);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        print(name);
        if (collider.GetComponent<CharacterRenderer>() && collider.GetType() == typeof(CapsuleCollider2D))
        {
            print("enter");
            CharacterRenderer renderer = collider.GetComponent<CharacterRenderer>();
            renderer.AddLight(this);
            renderer.CreateShadow(this, shadowPrefab);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        print(collider.name);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<CharacterRenderer>() && collider.GetType() == typeof(CapsuleCollider2D))
        {
            print("exit");
            CharacterRenderer renderer = collider.GetComponent<CharacterRenderer>();
            renderer.RemoveLight(this);
            renderer.RemoveShadow(this);
        }
    }

    IEnumerator Flutter(float delay)
    {
        yield return new WaitForSeconds(delay);

        transform.localPosition = initLocalPosition + new Vector3(Random.Range(-flutterRadius, flutterRadius), Random.Range(-flutterRadius, flutterRadius), 0);
        StartCoroutine("Flutter", flutterSpeed);

        yield return null;
    }
}
