using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightSource : MonoBehaviour
{

    private Vector3 initLocalPosition = new Vector3(0.38f, 0.445f, 0f);
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



    // Start is called before the first frame update
    void Start()
    {
        radiusIncrement = radius / GameRules.gameDuration * Time.fixedDeltaTime;
        StartCoroutine("Flutter", flutterSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lightPos = Camera.main.WorldToScreenPoint(transform.position - displacement);
        lightPos = new Vector3( lightPos.x / Camera.main.pixelWidth, lightPos.y / Camera.main.pixelHeight, 0);

        Shader.SetGlobalVector("_LightWorldPosition", transform.position - displacement);
        Shader.SetGlobalVector("_LightSourcePosition", lightPos);

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

        //Shader.SetGlobalFloat("_Radius", Mathf.Log(radius));
        Shader.SetGlobalFloat("_Radius", Mathf.Sqrt(radius) * radius / (radius + 0.2f));

        Shader.SetGlobalFloat("_TintIntensity", (tintMax * Mathf.Sqrt(radius / maxRadius) * radius / (radius + 0.2f) + tintMin));
        Shader.SetGlobalFloat("_GlowRadius", (-glowMin * radius/ (maxRadius- trail)) + glowMin);

    }

    IEnumerator Flutter(float elapsedTime)
    {
        yield return new WaitForSeconds(elapsedTime);

        transform.localPosition = initLocalPosition + new Vector3(Random.Range(-flutterRadius, flutterRadius), Random.Range(-flutterRadius, flutterRadius), 0);
        StartCoroutine("Flutter", flutterSpeed);

        yield return null;
    }
}
