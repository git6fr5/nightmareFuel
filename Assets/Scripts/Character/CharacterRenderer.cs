using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRenderer : MonoBehaviour
{

    /* --- Components --- */

    // Components
    public CharacterState characterState;
    public CharacterMovement characterMovement;

    // Perspective
    [HideInInspector] public float depth = 0;

    // Animation
    public Animator animator;
    public AnimationClip idleAnim;
    public AnimationClip runningAnim;
    public AnimationClip hurtAnim;
    public AnimationClip deathAnim;

    // Audio
    public Sound hurtSound;
    public Sound deathSound;
    public Sound aggroSound;
    public Sound idleSound;
    public Sound collectSound;

    // Material
    public SpriteRenderer spriteRenderer;
    public Material idleMaterial;
    public Material aggroMaterial;
    public Material hurtMaterial;
    public Material deathMaterial;

    // Model
    public Skeleton skeleton;
    public Particle[] particles;
    public Collider2D hull;
    public List<Shadow> shadows = new List<Shadow>();
    public List<LightSource> lights = new List<LightSource>();

    // Overlay
    public RectTransform overhead;
    public RectTransform hud;

    /* --- Internal Variables --- */

    // Controls
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public bool hurt = false;
    [HideInInspector] public bool death = false;
    [HideInInspector] public bool aggro = false;
    [HideInInspector] public bool collect = false;

    /* --- Unity Methods --- */
    void Update()
    {
        SetDepth();
        SetAnimation();
        SetMaterial();
        SetLightIntensity();
        SetAudio();
        SetOverlay();
    }

    public void SetDepth()
    {
        depth = transform.position.y + hull.offset.y;
    }

    public void SetAnimation()
    {
        if (characterState.stateDict[CharacterState.State.mobile] && runningAnim)
        {
            animator.Play(runningAnim.name);
            return;
        }
        animator.Play(idleAnim.name);
        return;
    }

    public void SetMaterial()
    {
        if (!characterMovement.facingRight)
        {
            spriteRenderer.material.SetFloat("_isFlipped", -1);
        }
        else
        {
            spriteRenderer.material.SetFloat("_isFlipped", 1);
        }

        // death
        if (characterState.stateDict[CharacterState.State.dead] && deathMaterial)
        {
            spriteRenderer.material = deathMaterial;
            return;
        }
        // hurt
        if (characterState.stateDict[CharacterState.State.hurt] && hurtMaterial)
        {
            spriteRenderer.material = hurtMaterial;
            return;
        }
        // aggro
        if (characterState.stateDict[CharacterState.State.aggro] && aggroMaterial)
        {
            //spriteRenderer.material = aggroMaterial;
            //return;
        }
        if (spriteRenderer.material != idleMaterial)
        {
            spriteRenderer.material = idleMaterial;
        }
        return;
    }

    public void SetOverlay()
    {
        if (overhead != null) 
        { 
            overhead.rotation = Quaternion.identity; 
        }
        if (hud != null)
        {
            hud.rotation = Quaternion.identity;
        }
        return;
    }

    public void SetAudio()
    {
        // death
        if (characterState.stateDict[CharacterState.State.dead] && deathSound)
        {
            deathSound.PlayAndDestroy(0.5f);
            return;
        }
        // hurt
        if (characterState.stateDict[CharacterState.State.hurt] && hurtSound)
        {
            hurtSound.Play();
            return;
        }
        return;
    }

    public void SetLightIntensity()
    {
        /*float intensity = 0f;
        Color color = new Color(0, 0, 0, 1);
        for (int i = 0; i < lights.Count; i++)
        {
            float dist = Vector2.Distance(transform.position, lights[i].transform.position);
            float radius = lights[i].areaOfEffect.radius;
            float maxIntensity = lights[i].centerIntensity;
            float _intensity = Mathf.Exp(-dist / (radius / 4));
            intensity = intensity + _intensity;
            color = lights[i].color * Mathf.Pow(_intensity, 0.1f) + color;
        }
        spriteRenderer.material.SetFloat("_LightIntensity", intensity);
        spriteRenderer.material.SetColor("_LightColor", color);*/
    }

    public void AddLight(LightSource source)
    {
        lights.Add(source);
    }

    public void RemoveLight(LightSource source)
    {
        if (lights.Contains(source)) { lights.Remove(source); }
    }

    public void CreateShadow(LightSource source, GameObject shadowPrefab)
    {
        for (int i = 0; i < shadows.Count; i++)
        {
            if (shadows[i].lightSource == source)
            {
                return;
            }
        }
        Shadow shadow = Instantiate(shadowPrefab, transform, false).GetComponent<Shadow>();
        shadow.transform.localPosition = Vector3.zero;
        shadow.gameObject.SetActive(true);

        shadow.CreateMaterial();
        shadow.SetRenderer(this);
        shadow.SetOffset(hull.offset);
        shadow.SetSource(source);

        shadows.Add(shadow);
    }

    public void RemoveShadow(LightSource source)
    {
        Shadow _deleteShadow = null;
        foreach( Shadow shadow in shadows)
        {
            if (shadow.lightSource == source)
            {
                _deleteShadow = shadow;
            }
        }
        shadows.Remove(_deleteShadow);
        Destroy(_deleteShadow.gameObject);
    }

}
