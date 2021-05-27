using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightSource : MonoBehaviour
{

    private Vector3 initLocalPosition;
    private float flutterRadius = 0.05f;
    private float flutterSpeed = 0.1f;
    private Vector3 displacement = new Vector3(0f, 1.2f, 0f);

    /* --- Internal Variables --- */
    [HideInInspector] public float maxRadius = 2f;
    [HideInInspector] public float radius = 1.5f;
    [HideInInspector] public float trail = 0.2f;

    [HideInInspector] public float tintMax = 0.2f;
    [HideInInspector] public float tintMin = 0.01f;

    [HideInInspector] public float glowMax = 0f;
    [HideInInspector] public float glowMin = -2.5f;

    [HideInInspector] public float radiusIncrement;

    [SerializeField] public LayerMask lightInteractableLayers;
    [SerializeField] public Collider2D lightCollider;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Flutter", flutterSpeed);
        initLocalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] lightInterables = Physics2D.OverlapCircleAll(transform.position, 40f * 0.2f, lightInteractableLayers);
        for (int i = 0; i < lightInterables.Length; i++)
        {
            print(lightInterables[i].name);
        }
    }

    IEnumerator Flutter(float elapsedTime)
    {
        yield return new WaitForSeconds(elapsedTime);

        transform.localPosition = initLocalPosition + new Vector3(Random.Range(-flutterRadius, flutterRadius), Random.Range(-flutterRadius, flutterRadius), 0);
        StartCoroutine("Flutter", flutterSpeed);

        yield return null;
    }
}
