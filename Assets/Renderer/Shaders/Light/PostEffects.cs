using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffects : MonoBehaviour
{
    // Renderer
    public Material[] postEffectMaterials;
    void OnRenderImage( RenderTexture source, RenderTexture destination)
    {
        for (int i = 0; i < postEffectMaterials.Length; i++)
        {
            Graphics.Blit(source, destination, postEffectMaterials[i]);
        }
    }
}
