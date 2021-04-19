using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightSource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lightPos = Camera.main.WorldToScreenPoint(transform.position);
        lightPos = new Vector3( lightPos.x / Camera.main.pixelWidth, lightPos.y / Camera.main.pixelHeight, 0);

        Shader.SetGlobalVector("_LightWorldPosition", transform.position);
        Shader.SetGlobalVector("_LightSourcePosition", lightPos);
    }
}
