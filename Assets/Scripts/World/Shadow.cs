using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{

    public SpriteRenderer shadowRenderer;
    [HideInInspector] public CharacterRenderer characterRenderer;
    [HideInInspector] public LightSource lightSource;
    private Material shadowMat;

    void Update()
    {
        CharacterMovement characterMovement = characterRenderer.characterMovement;
        if (!characterMovement.facingRight)
        {
            shadowRenderer.material.SetFloat("_isFlipped", -1);
        }
        else
        {
            shadowRenderer.material.SetFloat("_isFlipped", 1);
        }
        shadowRenderer.sprite = characterRenderer.spriteRenderer.sprite;
        shadowRenderer.material.SetVector("_LightWorldPosition", lightSource.transform.position);

    }

    public void CreateMaterial()
    {
        shadowMat = new Material(Shader.Find("NightmareFuel/ShadowShader"));
        shadowRenderer.material = shadowMat;
    }

    public void SetOffset(Vector2 hullOffset)
    {
        shadowRenderer.material.SetFloat("_HullXOffset", -hullOffset.x);
        shadowRenderer.material.SetFloat("_HullYOffset", -hullOffset.y);
    }

    public void SetSource(LightSource _lightSource)
    {
        lightSource = _lightSource;
    }

    public void SetRenderer(CharacterRenderer _characterRenderer)
    {
        characterRenderer = _characterRenderer;
    }
}
