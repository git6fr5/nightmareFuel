using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public AudioSource audioSource;
    public AudioClip hurtAudio;
    public AudioClip deathAudio;
    public AudioClip aggroAudio;
    public AudioClip idleAudio;
    public AudioClip collectAudio;

    // Material
    public SpriteRenderer spriteRenderer;
    public Material idleMaterial;
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
    }

    public void SetAudio()
    {
        return;
    }

    public void SetLightIntensity()
    {
        float intensity = 0f;
        for (int i = 0; i < lights.Count; i++)
        {
            float _intensity = 1 / Mathf.Log( Vector2.Distance(transform.position, lights[i].transform.position) + 1f);
            intensity = intensity + _intensity;
        }
        spriteRenderer.material.SetFloat("_LightIntensity", intensity);
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
