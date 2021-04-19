using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostEffects : MonoBehaviour
{
    // Renderer
    public Material[] postEffectMaterials;
    public int index;
    void OnRenderImage( RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, postEffectMaterials[index]);
    }
}
